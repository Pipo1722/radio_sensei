# radio_sensei

Código fuente C# de **OpenGD77 CPS** (software de programación de radios DMR/FM con firmware
OpenGD77), reconstruido por descompilación del instalador oficial
`OpenGD77CPSInstaller_E2026.07.13.01.exe`. Se puede modificar y recompilar.

```bash
dotnet build radio_sensei.sln -c Release
# → src/OpenGD77CPS/bin/Release/net48/OpenGD77CPS.exe  (copiar la carpeta entera a Windows)
```

## Estructura

```
radio_sensei/
├── src/
│   └── OpenGD77CPS/          Proyecto C# (WinForms, .NET Framework 4.8)
│       ├── DMR/                formularios del codeplug + comunicación con la radio
│       ├── UsbLibrary/         HID USB: bootloader de firmware MK22 (GD-77, DM-1801, RD-5R)
│       ├── Settings.cs         mapa de memoria, límites de banda, utilidades
│       └── OpenGD77CPS.csproj
├── tools/
│   └── renombrador/          Herramienta Roslyn para renombrar símbolos ofuscados
├── docs/
│   ├── ANALISIS.md           Arquitectura, lógica RF, mapa de memoria, protocolo, dónde tocar
│   ├── COMPILAR.md           Compilar, editar en VS Code, regenerar la descompilación
│   └── MEJORAS.md            Mejoras propuestas, por prioridad
├── third_party/
│   └── opengd77cps/          Instalador original y su contenido extraído (no se modifica)
├── .github/workflows/        CI: compila en cada push y publica el .exe como artefacto
├── .vscode/                  Tareas de build, depuración (Windows) y extensiones recomendadas
├── radio_sensei.sln
└── .editorconfig  .gitattributes  .gitignore
```

Equivalencia con la estructura anterior:

| Antes | Ahora |
|---|---|
| `codigo_descompilado/` | `src/OpenGD77CPS/` |
| `extraccion_binarios/{app}` | `third_party/opengd77cps/extracted/app` |
| `herramientas/` | `tools/` |

## Requisitos

| Para… | Necesitas |
|---|---|
| Compilar (Linux, macOS, Windows) | .NET SDK 8+ |
| Ejecutar / depurar | Windows con .NET Framework 4.8 |
| Editar | VS Code con la extensión C# (`ms-dotnettools.csharp`) |
| Renombrar símbolos | .NET SDK 8, bash y Python 3 (en Windows: Git Bash o WSL) |

---

## Notas para quien continúe el proyecto

Léelas antes de tocar nada. Recogen lo que no es evidente al abrir el código.

### Sobre el origen del código
1. **No es el código fuente original.** Sale de descompilar el `.exe` con ILSpy, así que no
   tiene comentarios, y parte de los nombres venían ofuscados del CPS de Radioddity del que
   desciende. Unos 140 se han renombrado. Quedan unos 500 `method_N`/`smethod_N`, sobre todo
   auxiliares de UI. Un nombre genérico no quiere decir que el método no importe.
2. **No vuelvas a descompilar sobre `src/`.** Se perderían las correcciones y los renombrados.
   Si sale un instalador nuevo, descompílalo en otra carpeta y compara (`docs/COMPILAR.md`).
3. **Los `IL_xxxx:` y los `goto`** de `CodeplugComms.cs` son restos de un descompilado que no
   se pudo estructurar. Es código muerto: no lo uses como referencia.

### Antes de modificar
4. **El formato binario es sagrado.** Los structs de los formularios (`ChannelForm.ChannelOne`,
   `ZoneForm.ZoneOne`...) se copian byte a byte al codeplug y a la radio con `Marshal`.
   - Añadir, quitar o reordenar un campo, o cambiar su tipo, desplaza todo lo que viene después.
   - Las direcciones están en el constructor estático de `Settings` (ver `docs/ANALISIS.md` §4).
   - Tras cambiar un struct, comprueba que `Marshal.SizeOf` no ha cambiado, salvo que también
     cambies el mapa de memoria **y** el firmware lo espere.
5. **La CPS no es el límite real de la radio.** Los límites de banda, la potencia y el número
   de canales que valida la CPS (`Settings.UpdateFreqRangesForRadioType`, constantes `CNT_*`)
   no cambian lo que el firmware acepta. Ampliarlos solo permite escribir valores que la radio
   puede rechazar o malinterpretar.
6. **Las frecuencias van en BCD con unidades de 10 Hz** (145,500 MHz = `0x14550000`). Usa
   siempre `Settings.Bcd32ToDecimal`/`DecimalToBcd32`. No hagas aritmética directa sobre el
   valor guardado.
7. **La calibración es lo más delicado.** `CalibrationForm` escribe en la flash de la radio
   (`0xF000` en MK22, `0x10000` en STM32). Antes de probar cualquier cambio con hardware,
   **haz un backup de la calibración y de la flash completa** desde la propia CPS.
8. **Los textos de la UI están en `Language/CPS/*.xml`**, no en el código. Para cambiar o
   traducir un texto basta con editar el XML: no hace falta recompilar.

### Al trabajar con el código
9. **Renombra con la herramienta, no con buscar y reemplazar.** Hay decenas de `method_0` en
   clases distintas. Usa F2 en VS Code o `tools/renombrador` para cambios en bloque.
   Haz los renombrados en commits separados de los cambios funcionales.
10. **La comunicación con la radio está duplicada.** Hay 5 copias de `setupCommPort` y 2 de
    `sendCommand`. Si cambias el protocolo o los timeouts, revísalas todas
    (`docs/MEJORAS.md` §6). La implementación de referencia es la de `DMR/OpenGD77Form.cs`.
11. **Mantén 0 avisos.** CI compila con `-warnaserror`. Un aviso nuevo rompe el build a
    propósito.
12. **Estilo**: tabulaciones, UTF-8 sin BOM y LF, según `.editorconfig`. `formatOnSave` está
    desactivado para que los diffs muestren solo los cambios reales.
13. **`third_party/` no se toca.** Es la referencia original. El `.csproj` copia de ahí las DLL
    y los datos que necesita el `.exe`.

### Estado actual y pendientes
14. **Nunca se ha ejecutado en Windows ni probado con una radio.** Se verificó que compila y que
    sus metadatos (tipos, referencias, recursos) coinciden con el original. El primer paso de
    quien continúe debería ser la prueba de humo de `docs/MEJORAS.md` §1.
15. **Correcciones respecto al original:**
    - timeouts y un bucle infinito en la calibración
    - una fuga de handles al detectar el puerto COM
    - un mensaje de excepción que se perdía

    Detalle en `docs/ANALISIS.md` §8.
16. **El build modificado tiene la misma versión que el oficial** (`1.2.9690.20973`). Hay que
    cambiarla antes de distribuirlo (`docs/MEJORAS.md` §2).
17. **Restos de la subida inicial ya eliminados:**
    - `install.cmd`, el instalador de *Antigravity CLI*, que no tenía relación con el proyecto.
    - El submódulo roto `OpenGD77CPS/`, que apuntaba al commit `8fbecb6` sin `.gitmodules`.

    Si aparecen en un clon antiguo, se pueden borrar.
18. **Licencia:** comprueba la del CPS de OpenGD77 antes de redistribuir el código o el
    ejecutable (`docs/MEJORAS.md` §3).

## Documentación

- [`docs/ANALISIS.md`](docs/ANALISIS.md): cómo funciona el programa y dónde tocar cada cosa.
- [`docs/COMPILAR.md`](docs/COMPILAR.md): compilar, VS Code y regenerar la descompilación.
- [`docs/MEJORAS.md`](docs/MEJORAS.md): mejoras propuestas, por prioridad.
- [`tools/renombrador/LEEME.md`](tools/renombrador/LEEME.md): renombrado seguro de símbolos.
- [`third_party/opengd77cps/LEEME.md`](third_party/opengd77cps/LEEME.md): origen del material
  original y cómo volver a extraerlo.
