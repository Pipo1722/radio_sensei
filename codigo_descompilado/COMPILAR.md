# OpenGD77CPS — código descompilado

Generado con `ilspycmd` 9.1 a partir de `extraccion_binarios/{app}/OpenGD77CPS.exe`
(instalador `OpenGD77CPSInstaller_E2026.07.13.01.exe`, build 2026-07-13).

## Regenerar la descompilación

```bash
dotnet tool install -g ilspycmd --version 9.1.0.7988
R=~/.nuget/packages/microsoft.netframework.referenceassemblies.net48/1.0.3/build/.NETFramework/v4.8
A="extraccion_binarios/{app}"
ilspycmd -p -o codigo_descompilado -r "$A" -r "$R" -r "$R/Facades" \
  --languageversion CSharp7_3 "$A/OpenGD77CPS.exe"
```

Pasar las reference assemblies de .NET Framework 4.8 (`-r`) es imprescindible: sin
ellas ILSpy resuelve los tipos contra .NET 8 y genera código que no compila
(`((Form)this).Dispose(...)` en lugar de `base.Dispose(...)`, etc.).
Después hay que restaurar este `OpenGD77CPS.csproj`, que ILSpy sobrescribe.

## Compilar

Requiere .NET SDK 8 (funciona en Linux, macOS y Windows):

```bash
cd codigo_descompilado
dotnet build -c Release
```

Salida: `bin/Release/net48/OpenGD77CPS.exe`, junto con los archivos de datos y las
DLL nativas copiadas desde `extraccion_binarios/{app}`. Copia la carpeta entera
a Windows (con .NET Framework 4.8) para ejecutarlo.

Notas:
- Al compilar fuera de Windows las imágenes de los `.resx` se incrustan en formato
  `System.Resources.Extensions`; por eso `System.Resources.Extensions.dll` (y sus
  dependencias) debe acompañar al `.exe`.
- El original incluía 44 recursos `.resources` vacíos que ILSpy no exporta;
  ningún código los lee, así que su ausencia no afecta.
