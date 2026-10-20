# Mejoras propuestas

Ordenadas por prioridad. Cada punto indica **por qué** importa y **dónde** está el problema
(rutas relativas a `src/OpenGD77CPS/`). Nada de esta lista está implementado salvo que se diga.
El estado actual: compila con 0 avisos y CI en verde, pero **nunca se ha ejecutado en Windows
ni probado con una radio**.

---

## P0 — Antes de usar un build modificado con una radio real

### 1. Prueba de humo en Windows
Ningún build de este repositorio se ha ejecutado todavía. Procedimiento mínimo:
1. Ejecutar `OpenGD77CPS.exe` del artefacto de CI en Windows con .NET Framework 4.8.
2. Abrir y guardar `DefaultOpenGD77.g77`.
3. Con la radio conectada, **hacer primero un backup de la calibración y otro de la flash**
   (menú OpenGD77). Solo después probar la lectura y escritura del codeplug.
4. Comparar byte a byte un codeplug leído con el CPS oficial y con este build.

### 2. Distinguir el build modificado del oficial
`Properties/AssemblyInfo.cs` conserva la versión oficial (`1.2.9690.20973`, "OpenGD77 CPS").
Un usuario o un informe de error no podrá saber qué build tiene. Propuesta: cambiar
`AssemblyProduct`/`AssemblyTitle` (por ejemplo "OpenGD77 CPS (radio_sensei)") y la versión, y
mostrarlo en `AboutForm`.

### 3. Licencia y procedencia
Antes de redistribuir el ejecutable o el código, hay que comprobar la licencia del CPS de OpenGD77
(https://www.opengd77.com). Si el proyecto publica el código fuente del CPS, partir de él sería
mejor que de esta descompilación: tiene nombres y comentarios originales y es más fácil de
mantener. Este repositorio sigue siendo útil para comparar comportamientos.

### 4. Limpiar la raíz del repositorio
Quedan dos elementos de la subida inicial que no pertenecen al proyecto:
- `install.cmd` es el instalador de **Antigravity CLI** (Google). Descarga y ejecuta `agy.exe`
  desde un servidor externo. No tiene relación con OpenGD77. **Recomendado: borrarlo**, o al menos
  no ejecutarlo.
- `OpenGD77CPS/` es un **submódulo roto**: una referencia al commit `8fbecb6` sin `.gitmodules`.
  Cualquier clon recibe una carpeta vacía. Si contiene algo útil en tu equipo, súbelo como carpeta
  normal. Si no, `git rm --cached OpenGD77CPS`.

---

## P1 — Calidad y mantenibilidad

### 5. Tests de regresión del formato binario
Es el mayor riesgo al modificar structs: un campo de más o de menos desplaza todo el codeplug y
la radio recibe basura. Propuesta: un proyecto `tests/OpenGD77CPS.Tests` (xUnit, `net48`) con:
- **Ida y vuelta**: `ByteToData(DefaultOpenGD77.g77)` → `DataToByte()` debe dar exactamente
  los mismos bytes.
- `Marshal.SizeOf` de cada struct (`ChannelOne`, `ZoneOne`, `Contact`...) fijado como constante.
- Funciones puras de `Settings`: `Bcd32ToDecimal`/`DecimalToBcd32`, `GetCommonBandIndex`,
  `RoundToNearestStep`, `checkFrequecyIsInValidRange`.

Los tests `net48` necesitan Windows. En CI se haría con un job `windows-latest` además del
de Linux.

### 6. Un único módulo de comunicación con la radio
El código de comunicación por puerto serie está **duplicado**:

| Función | Copias | Dónde |
|---|---|---|
| `sendCommand` | 2 | `DMR/OpenGD77Form.cs`, `DMR/CalibrationForm.cs` |
| `setupCommPort` | 5 | `OpenGD77Form`, `CalibrationForm`, `DMRIDForm`, `FirmwareLoader_DM32`, `Extras.OpenGD77/ThemeForm` |
| Lectura/escritura flash/EEPROM | 2 | `OpenGD77Form`, `CalibrationForm` |

Cada copia tiene timeouts distintos. El fallo de bucle infinito corregido estaba solo en una de
ellas. Propuesta: extraer una clase `OpenGD77Protocol` (puerto, comandos `C`/`R`/`W`, timeouts,
reintentos) y que los formularios la usen. De paso, `OpenGD77Form.cs` (3 400 líneas) quedaría en
solo UI.

### 7. Errores que nadie ve
- Hay **59 `catch` vacíos o silenciosos**. Fallos de lectura de datos, idiomas o red se tragan
  sin aviso.
- Hay **42 `Console.WriteLine`** con mensajes de error ("read stopped", "write stopped"...). En una
  aplicación WinForms no hay consola, así que se pierden.
- Ya existe `DMR/Log.cs` (escribe `Log.txt`), pero casi no se usa. Además escribe junto al `.exe`:
  en `Program Files` falla sin permisos de administrador.
- `MainWindow.Main` no registra `Application.ThreadException` ni
  `AppDomain.UnhandledException`. Hay un manejador huérfano (`MainWindow.smethod_0`) que
  probablemente lo hacía en el original.

Propuesta: conectar ese manejador, mover `Log.txt` a `%LOCALAPPDATA%` y sustituir los
`Console.WriteLine` de error por `Log.WriteLine`.

### 8. Seguir renombrando
Quedan unos 500 `method_N`/`smethod_N`, sobre todo auxiliares de UI. Hay que hacerlo siempre con
`tools/renombrador` (nunca con buscar y reemplazar) y en commits separados de los cambios
funcionales, para que la revisión sea fácil.

### 9. Eliminar código muerto
Ver la lista en `docs/ANALISIS.md` §7:
- `CodeplugComms` + `CommPrgForm`
- `FirmwareUpdate`
- `WaitForm`, `VoteScanForm`, `PowerPwdForm`, `DmrMarcData`
- `CustomListbox`, `AutoClosingMessageBox`
- `Settings.SaveCodeplugBinary`/`LoadCodeplugBinary`, que no tienen llamadas

Son unas 3 000 líneas que confunden al leer. Antes de borrar, conviene buscar con `rg` y
comprobar los `.resx`.

---

## P2 — Seguridad

### 10. Descargas por HTTP sin cifrar
Las bases de datos de contactos se descargan por `http://`:
- `DMR/DownloadContactsForm.cs:78` y `:200` (`ham-digital.org`, `radioid.net`)
- `DMR/DMRIDForm.cs:384`

Alguien en la red podría alterar los datos. `radioid.net` ya se usa por HTTPS en otras partes
del código. Propuesta: pasar las tres a `https://`, si los servidores lo soportan.

### 11. `BinaryFormatter`
`Settings.cloneObject` lo usa para copiar structs en memoria (12 llamadas). No es un riesgo
inmediato, porque no lee archivos externos. Pero está obsoleto y **bloquea una migración a
.NET 9+**, donde ya no existe. Propuesta: sustituirlo por copias explícitas o por
`MemberwiseClone` más la copia de los arrays.

### 12. Contraseña maestra en el código
`Settings.SUPER_PWD = "DT8168"` (`Settings.cs:111`), heredada del CPS de Radioddity. Es pública,
pero hay que documentar qué protege y valorar si se mantiene.

---

## P3 — Evolución

### 13. Migrar a .NET 8 (`net8.0-windows`)
Ventajas:
- C# moderno, mejor rendimiento y alto DPI.
- Desaparece el truco de `System.Resources.Extensions`.

Requisitos previos:
- tests (punto 5)
- eliminar `BinaryFormatter` (punto 11)
- comprobar que `WeifenLuo.WinFormsUI.Docking` 3.0 funciona en .NET 8, o actualizarlo
- añadir `System.IO.Ports` como paquete NuGet

### 14. Instalador propio
`third_party/opengd77cps/extracted/install_script.iss` es el script de Inno Setup original.
Propuesta: una copia adaptada en `installer/OpenGD77CPS.iss`, con otro `AppId` para no pisar el
CPS oficial al instalar, y generarla desde CI en un job de Windows.

### 15. Nombres de carpetas y espacios de nombres
Hoy conviven espacios de nombres heredados (`DMR`, `Extras.OpenGD77`, `UsbLibrary`, el global)
con carpetas como `DMR.Properties`. Cuando haya tests, se puede unificar bajo `OpenGD77CPS.*`.
Va en un commit propio, porque toca casi todos los archivos.
