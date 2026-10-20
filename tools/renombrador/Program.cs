// Renames symbols in the decompiled project with Roslyn, so every reference is updated.
// Usage: renombrador <projectDir> <referencePathsFile> -- <mapFile>
// (renombrar.sh builds the reference list with MSBuild and runs this.)
// Map lines:  Type => NewType            (type rename, also renames Type.cs file)
//             Type::member => newName     (field/method/property/event/nested type)
//             Type::member(param) => new  (parameter or type parameter of a member)
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;

var sep = Array.IndexOf(args, "--");
var projDir = Path.GetFullPath(args[0]);
var refsFile = args[1];
var mapFile = args[sep + 1];

var ws = new AdhocWorkspace();
var refs = File.ReadAllLines(refsFile).Where(l => l.Trim().Length > 0).Select(l => (MetadataReference)MetadataReference.CreateFromFile(l.Trim())).ToList();
var proj = ws.AddProject(ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "p", "OpenGD77CPS", LanguageNames.CSharp,
    compilationOptions: new CSharpCompilationOptions(OutputKind.WindowsApplication, allowUnsafe: true),
    parseOptions: new CSharpParseOptions(LanguageVersion.CSharp7_3), metadataReferences: refs));
foreach (var f in Directory.GetFiles(projDir, "*.cs", SearchOption.AllDirectories).Where(f => !Path.GetRelativePath(projDir, f).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Any(d => d == "obj" || d == "bin")))
    proj = ws.AddDocument(DocumentInfo.Create(DocumentId.CreateNewId(proj.Id), Path.GetFileName(f), loader: TextLoader.From(TextAndVersion.Create(Microsoft.CodeAnalysis.Text.SourceText.From(File.ReadAllText(f)), VersionStamp.Create())), filePath: f)).Project;
var solution = ws.CurrentSolution;

int Errors(Solution s) => s.Projects.First().GetCompilationAsync().Result.GetDiagnostics().Count(d => d.Severity == DiagnosticSeverity.Error);
Console.WriteLine($"errors before: {Errors(solution)}");
foreach (var d in solution.Projects.First().GetCompilationAsync().Result.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).Take(5)) Console.WriteLine("  " + d);

async Task<ISymbol> Find(Solution s, string spec)
{
    var comp = await s.Projects.First().GetCompilationAsync();
    string typeName = spec, member = null, param = null;
    if (spec.Contains("::")) { typeName = spec[..spec.IndexOf("::")]; member = spec[(spec.IndexOf("::") + 2)..]; }
    if (member != null && member.Contains('(')) { param = member[(member.IndexOf('(') + 1)..^1]; member = member[..member.IndexOf('(')]; }
    var simple = typeName.Split('.').Last();
    var types = comp.GetSymbolsWithName(n => n == simple, SymbolFilter.Type).OfType<INamedTypeSymbol>().Where(t => t.Locations.Any(l => l.IsInSource)).ToList();
    if (types.Count > 1) types = types.Where(x => x.ToDisplayString() == typeName).ToList();
    if (types.Count != 1) throw new Exception($"{spec}: {types.Count} types named {typeName}");
    var t = types[0];
    if (member == null) return t;
    var members = t.GetMembers(member).ToList();
    if (members.Count != 1) throw new Exception($"{spec}: {members.Count} members");
    var m = members[0];
    if (param == null) return m;
    if (m is IMethodSymbol ms)
    {
        ISymbol p = ms.Parameters.FirstOrDefault(x => x.Name == param) ?? (ISymbol)ms.TypeParameters.FirstOrDefault(x => x.Name == param);
        if (p != null) return p;
    }
    throw new Exception($"{spec}: param not found");
}

var fileRenames = new List<(string, string)>();
foreach (var raw in File.ReadAllLines(mapFile))
{
    var line = raw.Split('#')[0].Trim();
    if (line.Length == 0) continue;
    var parts = line.Split("=>", StringSplitOptions.TrimEntries);
    var sym = await Find(solution, parts[0]);
    solution = await Renamer.RenameSymbolAsync(solution, sym, new SymbolRenameOptions(RenameOverloads: false, RenameInStrings: false, RenameInComments: false, RenameFile: false), parts[1]);
    if (sym is INamedTypeSymbol && sym.ContainingType == null)
        foreach (var loc in sym.Locations.Where(l => l.IsInSource))
            if (Path.GetFileNameWithoutExtension(loc.SourceTree.FilePath) == sym.Name)
                fileRenames.Add((loc.SourceTree.FilePath, Path.Combine(Path.GetDirectoryName(loc.SourceTree.FilePath), parts[1] + ".cs")));
}
var after = Errors(solution);
Console.WriteLine($"errors after: {after}");
if (after > 0)
{
    foreach (var d in solution.Projects.First().GetCompilationAsync().Result.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).Take(20)) Console.WriteLine(d);
    return 1;
}
int changed = 0;
foreach (var id in solution.GetChanges(ws.CurrentSolution).GetProjectChanges().SelectMany(p => p.GetChangedDocuments()))
{
    var doc = solution.GetDocument(id);
    File.WriteAllText(doc.FilePath, (await doc.GetTextAsync()).ToString());
    changed++;
}
foreach (var (from, to) in fileRenames) File.Move(from, to);
Console.WriteLine($"files changed: {changed}, files renamed: {fileRenames.Count}");
return 0;
