# Material original de OpenGD77 CPS

Contenido del instalador oficial `OpenGD77CPSInstaller_E2026.07.13.01.exe`, versión E2026.07.13.01,
publicado en https://www.opengd77.com. **No se modifica**: es la referencia contra la que se
compara el código reconstruido.

| Ruta | Origen en el instalador |
|---|---|
| `OpenGD77CPSInstaller_E2026.07.13.01.exe` | Instalador original (Inno Setup 6.6.1) |
| `extracted/app/` | `{app}`: lo que se instala en `Program Files\OpenGD77CPS` |
| `extracted/driver/wdi-simple.exe` | `{%TEMP}\OpenGD77CommDriver`: instala el driver USB (VID 0x1FC9, PID 0x0094) |
| `extracted/install_script.iss` | Script de Inno Setup reconstruido por innounp |

Lo usa el código:
- `src/OpenGD77CPS/OpenGD77CPS.csproj` referencia `Newtonsoft.Json.dll` y `WeifenLuo.WinFormsUI.Docking.dll`.
- Al compilar, copia el resto de `extracted/app/` a la carpeta de salida.

## Volver a extraer

innoextract 1.9 no soporta Inno Setup 6.6. Usa innounp 2.x
(https://github.com/jrathlev/InnoUnpacker-Windows-GUI) en Windows, o con Wine en Linux:

```bash
wine innounp.exe -x -a -y -d<destino> OpenGD77CPSInstaller_E2026.07.13.01.exe
```

innounp crea carpetas `{app}` y `{%TEMP}`. Aquí se han renombrado a `app/` y `driver/` para que
las rutas sean fáciles de usar en scripts.
