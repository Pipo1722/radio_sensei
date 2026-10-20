# OpenGD77CPS — compilar y editar

Código generado con `ilspycmd` 9.1 a partir de `third_party/opengd77cps/extracted/app/OpenGD77CPS.exe`
(instalador `OpenGD77CPSInstaller_E2026.07.13.01.exe`, build 2026-07-13). Desde entonces se ha
modificado a mano: correcciones, renombrados y avisos resueltos. Ver `docs/ANALISIS.md`.

## Compilar

Requiere .NET SDK 8 o superior. Funciona en Linux, macOS y Windows:

```bash
dotnet build radio_sensei.sln -c Release      # desde la raíz del repositorio
```

Salida: `src/OpenGD77CPS/bin/Release/net48/OpenGD77CPS.exe`, junto con los archivos de datos y
las DLL nativas copiadas desde `third_party/opengd77cps/extracted/app`. Para ejecutarlo, copia la carpeta
entera a Windows (con .NET Framework 4.8).

Notas:
- Al compilar fuera de Windows, las imágenes de los `.resx` se incrustan en formato
  `System.Resources.Extensions`. Por eso `System.Resources.Extensions.dll` (y sus dependencias)
  debe acompañar al `.exe`.
- El original incluía 44 recursos `.resources` vacíos que ILSpy no exporta. Ningún código los lee.

## Editar en VS Code

1. Abre la carpeta raíz del repositorio (`radio_sensei/`).
2. Instala las extensiones recomendadas cuando VS Code lo proponga: **C#** (`ms-dotnettools.csharp`)
   y **EditorConfig**.
3. La extensión carga `radio_sensei.sln`. Ya tienes IntelliSense, "Ir a definición" (F12),
   "Buscar todas las referencias" (Shift+F12) y "Renombrar símbolo" (F2).
4. Para compilar usa `Ctrl+Shift+B` (tarea `build`, Debug) o la tarea `build release`.
5. Depurar (F5, configuración "OpenGD77CPS (Windows)") solo funciona en Windows.

El estilo del código es de tabulaciones, UTF-8 sin BOM y fin de línea LF. Lo fija `.editorconfig`.
`formatOnSave` está desactivado para que los diffs muestren solo tus cambios.

## Regenerar la descompilación (¡cuidado!)

Volver a descompilar **sobrescribe todos los cambios** hechos sobre el código: correcciones y
renombrados. Hazlo solo en una carpeta aparte, por ejemplo para comparar con otra versión del
instalador:

```bash
dotnet tool install -g ilspycmd --version 9.1.0.7988
R=~/.nuget/packages/microsoft.netframework.referenceassemblies.net48/1.0.3/build/.NETFramework/v4.8
A="third_party/opengd77cps/extracted/app"
ilspycmd -p -o /tmp/descompilado_nuevo -r "$A" -r "$R" -r "$R/Facades" \
  --languageversion CSharp7_3 "$A/OpenGD77CPS.exe"
```

Pasar las reference assemblies de .NET Framework 4.8 (`-r`) es imprescindible. Sin ellas, ILSpy
resuelve los tipos contra .NET 8 y genera código que no compila (por ejemplo,
`((Form)this).Dispose(...)` en lugar de `base.Dispose(...)`).
