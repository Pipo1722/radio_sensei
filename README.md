# radio_sensei

Código fuente de OpenGD77 CPS reconstruido a partir del instalador
`OpenGD77CPSInstaller_E2026.07.13.01.exe`, listo para modificarlo y recompilarlo.

| Carpeta / archivo | Contenido |
|---|---|
| `extraccion_binarios/` | Archivos del instalador (Inno Setup 6.6.1, extraídos con innounp) |
| `codigo_descompilado/` | Proyecto C# (WinForms, .NET Framework 4.8) que compila con `dotnet build` |
| `codigo_descompilado/ANALISIS.md` | Arquitectura, lógica RF, mapa de memoria, protocolo y dónde tocar |
| `codigo_descompilado/COMPILAR.md` | Cómo compilar y editar en VS Code |
| `herramientas/renombrador/` | Herramienta Roslyn para renombrar símbolos ofuscados de forma segura |
| `radio_sensei.sln` | Solución que abre VS Code |

Inicio rápido:

```bash
dotnet build radio_sensei.sln -c Release
# → codigo_descompilado/bin/Release/net48/OpenGD77CPS.exe
```
