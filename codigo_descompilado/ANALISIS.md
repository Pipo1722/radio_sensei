# Análisis del código de OpenGD77 CPS

Guía para orientarse en el código descompilado de `OpenGD77CPS.exe` (build 2026-07-13).
Unas 57 800 líneas de C# en 123 archivos, aplicación WinForms para .NET Framework 4.8.

## 1. Arquitectura

```
MainWindow.Main()                  punto de entrada → abre MainForm
└── DMR/MainForm.cs                ventana principal (MDI + DockPanel), árbol del codeplug,
    │                              abrir/guardar .g77, menús
    ├── Formularios de edición     uno por sección del codeplug (ChannelForm, ZoneForm,
    │   (DMR/*Form.cs)             ContactForm, ...). Cada uno contiene:
    │                                – un struct con el layout binario exacto en memoria
    │                                  (p. ej. ChannelForm.ChannelOne)
    │                                – una clase contenedora estática `data`
    │                                – la UI que lee/escribe ese struct
    ├── DMR/OpenGD77Form.cs        TODA la comunicación con la radio (lectura/escritura del
    │                              codeplug, backups, calibración, pantallazos, voice prompts...)
    ├── DMR/CalibrationForm.cs     editor de calibración (+ CalibrationData.cs)
    ├── DMR/FirmwareLoader*.cs     actualización de firmware (MK22, STM32 por DFU, DM32)
    └── Settings.cs                utilidades globales: mapa de memoria, límites de banda,
                                   conversiones BCD, carga de idiomas, helpers de UI
```

**Flujo de datos.** Todo el codeplug vive en memoria como structs estáticos (`ChannelForm.data`,
`ZoneForm.data`, ...).
- `MainForm.DataToByte()` (`DMR/MainForm.cs:3226`) serializa todos los structs a un búfer
  de 128 KB.
- `MainForm.ByteToData()` (`DMR/MainForm.cs:3382`) hace lo contrario.
- Ese búfer es a la vez el archivo `.g77` y la imagen que se escribe en la radio.

## 2. Modelos soportados

`MainForm.RadioInfoPlatformEnum` (`DMR/MainForm.cs:31`): GD-77, GD-77S, DM-1801, RD-5R,
DM-1801A, MD-9600, MD-UV380, MD-380, DM-1701 (BGR y RGB), MD-2017, DM-32 y UV-008.
Por familia de hardware hay tres variantes: `RadioTypeMK22` (NXP), `RadioTypeSTM32` y `RadioTypeC7000`.

## 3. Lógica de radiofrecuencia

### Límites de banda — `Settings.cs:1491` (`UpdateFreqRangesForRadioType`)

Los valores están en **MHz**. El índice 0 es UHF y el 1 es VHF:

| Modelo | Banda 0 (UHF) | Banda 1 (VHF) |
|---|---|---|
| RD-5R | 380–564 | 80–282 |
| DM-32, UV-008 | 740–1120 | 16–560 |
| Resto (por defecto) | 380–564 | 127–282 |

Estos límites solo afectan a la **validación en la CPS**. Lo que la radio transmite de verdad
lo decide su firmware. Abrir los límites aquí no amplía el rango real del hardware.

Funciones relacionadas en `Settings.cs`:
- `checkFrequecyIsInValidRange`: valida y propone la frecuencia válida más cercana.
- `GetCommonBandIndex`: comprueba que RX y TX están en la misma banda.
- `RoundToNearestStep` y `RoundUpToValidStep`: redondean a los pasos de 2,5 kHz (`FREQ_STEP_1 = 250`)
  y 6,25 kHz (`FREQ_STEP_2 = 625`), en unidades de 10 Hz.

### Formato de frecuencia

Las frecuencias se guardan como **BCD de 8 dígitos en unidades de 10 Hz**. Por ejemplo,
145,500 MHz es `0x14550000`. Ver `ChannelForm.ChannelOne.RxFreq`/`RxFreqDec`/`RxFreqBcd` y las
funciones `Settings.Bcd32ToDecimal`/`DecimalToBcd32`. Los tonos CTCSS/DCS van en BCD de 16 bits
(`Bcd16ToDecimal`).

### Canal — `DMR/ChannelForm.cs:132` (`struct ChannelOne`)

Es el layout binario exacto de un canal, en orden:
- nombre de 16 bytes
- `rxFreq` y `txFreq`
- modo, potencia, TOT
- tonos RX y TX
- grupo RX, color code, sistema APRS, contacto
- banderas y squelch

- Modos (`ChModeE`): FM, DMR, FM broadcast y AM.
- Potencias (`LibreDMR_Power_E`, `DMR/ChannelForm.cs:24`): sin override, 50 mW, 250 mW, 500 mW,
  750 mW, 1 W, 2 W, 3 W, 4 W, 5 W y 5 W+.
- Tonos: la lista `Settings.TONES_LIST` (`Settings.cs:1523`) tiene 50 CTCSS y los DCS en
  formato `Dnnn N/I`.

Capacidades (constantes `CNT_*`):

| Elemento | Cantidad |
|---|---|
| Canales | 1024 (`ChannelForm.CNT_CH`), en 8 grupos de 128 |
| Zonas | 68, con 80 canales cada una (`ZoneForm`) |
| Contactos DMR | 1024 |
| Contactos DTMF | 63 |
| Listas RX | 76, con 32 contactos cada una |
| Listas de escaneo | 64 |
| Canales VFO | 4 |
| Mensajes de texto | 32 |

### Calibración — `DMR/CalibrationData.cs`, `DMR/CalibrationForm.cs`

`CalibrationData` contiene:
- 16 `PowerSettings`
- el ajuste fino del oscilador de referencia (`DACOscRefTune`)
- los umbrales de squelch (`Mute*`) y RSSI
- bloques que nadie ha identificado todavía (`UnknownBlock*`, `*_NOTCONFIRMED`)

Ubicación en la radio: `0xF000` en MK22, `0x10000` en STM32 y `0x7C00` con el protocolo
USB oficial. **Escribir valores incorrectos aquí puede dejar la radio fuera de frecuencia o de
potencia.** Haz siempre un backup antes.

## 4. Mapa de memoria del codeplug

Definido en el constructor estático de `Settings` (`Settings.cs:1641` y siguientes).
Los offsets son dentro del búfer de 128 KB:

| Dirección | Contenido |
|---|---|
| `0x0080` (128) | Información del dispositivo |
| `0x00E0` (224) | Ajustes generales (incluye la contraseña de programación) |
| `0x0108` / `0x0110` | Botones laterales / One Touch |
| `0x0128` | Mensajes de texto |
| `0x1370` | Cifrado |
| `0x13F8` / `0x1400` | Señalización básica / DTMF |
| `0x1588` | Sistemas APRS |
| `0x1788` | Contactos DMR (y escaneo básico) |
| `0x2F88` | Contactos DTMF |
| `0x3738` / `0x3740` | Zonas: básico / lista |
| `0x3780` | Canales (primer grupo) |
| `0x6000` | Últimos canales usados por zona (OpenGD77) |
| `0x7518` – `0x7590` | Pantalla de arranque, teclas numéricas, menú, VFO |
| `0x8000` | Zonas extendidas |
| `0xAF10` | Listas de escaneo extendidas |
| `0xB1B0` | Canales extendidos (7 grupos) |
| `0x17620` | Contactos DMR extendidos |
| `0x1D620` | Listas RX extendidas |
| `0x1EE60` – `0x20000` | Datos personalizados de OpenGD77 (temas, etc.) |

Para leer de la radio, `OpenGD77Form.cs:1680` (`READ_CODEPLUG`) copia estos tramos:

| Origen en la radio | Rango del búfer | Medio |
|---|---|---|
| EEPROM | `0x0080–0x6000` | EEPROM (flash en STM32) |
| EEPROM | últimos canales usados | EEPROM |
| EEPROM | `0x7500–0xB000` | EEPROM (flash en STM32) |
| flash `0x7B000` | `0xB000–0x1EE60` | flash |
| flash `0x0` | `0x1EE60–0x20000` | flash (datos personalizados) |

La escritura (`WRITE_CODEPLUG`) recorre los mismos tramos.

## 5. Protocolo USB/serie (OpenGD77)

El puerto se configura a 115200 8N1 (`OpenGD77Form.cs`). Cada orden lleva un byte inicial:

| Comando | Formato | Uso |
|---|---|---|
| `'C'` (0x43) | `C, n, args…` | Control. `0` = entrar en modo CPS, `1` = limpiar pantalla, `2` = texto (y, tamaño, alineación), `3` = refrescar pantalla, `5` + `7` = cerrar la sesión. `6, x` = acciones: `6,3` se envía antes de leer, `6,4` antes de escribir y `6,2` + `6,1` al terminar una escritura. Por el uso, parecen el indicador de lectura/escritura y "guardar ajustes y reiniciar" (inferido, sin confirmar) |
| `'R'` (0x52) | `R, modo, addr32, len16` | Leer. El modo es el valor de `CommsDataMode`: 1 = flash, 2 = EEPROM, 5 = ROM de la MCU, 6 = pantallazo, 9 = info de la radio… |
| `'W'` (0x57) | `W, sub, …` | Escribir. `sub`: 1 = preparar sector de 4 KB, 2 = enviar datos, 3 = grabar sector, 4 = escribir EEPROM, 7 = escribir WAV |

- La radio responde repitiendo los dos primeros bytes.
- El tamaño de bloque depende de `getUSBReadBufferSize`/`getUSBWriteBufferSize`.
- Las esperas usan `waitForPortData` (`OpenGD77Form.cs:2855`), con un timeout de unos 750 ms.

## 6. Textos e idiomas

- Toda la UI se traduce desde `Language/CPS/<Idioma>.xml`. Los nodos tienen la forma
  `/Resource/<NombreForm>/Controls/Control[@Id][@Text]`.
- La carga la hacen `Settings.UpdateComponentTextsFromLanguageXmlData` y `LoadCommonString*`.
- **Cambiar textos no requiere recompilar**: basta con editar el XML en la carpeta de salida.
- La ayuda contextual está en `help.xml`.

## 7. Código muerto (no se usa desde ningún sitio)

Nadie instancia estas clases. Están documentadas y no se han borrado, por si se quieren recuperar:

| Archivo | Qué es |
|---|---|
| `CodeplugComms.cs` + `DMR/CommPrgForm.cs` | Protocolo HID del firmware oficial del GD-77 (Radioddity). Se nota que viene de código ofuscado: tiene etiquetas `IL_xxxx` y `goto` |
| `FirmwareUpdate.cs` | Actualizador DFU antiguo. El que se usa es `STM_DFU_FwUpdate.cs` |
| `DMR/WaitForm.cs`, `VoteScanForm.cs`, `PowerPwdForm.cs`, `DmrMarcData.cs` | Formularios y datos heredados del CPS original |
| `CustomListbox.cs`, `Common/AutoClosingMessageBox.cs` | Utilidades sin uso |
| `FirmwareLoaderUI_MK22.downloadStringCompletedCallback` | La "descarga de firmware desde GitHub" para MK22. No está conectada a ningún evento y usa un `WebClientAsync wc` que nunca se crea. Si se conecta tal cual, falla con `NullReferenceException` |

## 8. Problemas corregidos respecto al original

1. **`CalibrationForm`.** Las esperas del puerto serie no tenían timeout, y una respuesta de
   error de la radio dejaba `ReadFlashOrEEPROM` en un bucle infinito. Pasaba durante el backup
   y la restauración de la calibración. Ahora hay un timeout de 5 s y se sale del bucle.
2. **`SetupDiWrap.ComPortNameFromFriendlyNamePrefix`.** Perdía un handle de Windows en cada
   conexión, porque no llamaba a `SetupDiDestroyDeviceInfoList` cuando encontraba el puerto.
3. **`HIDDeviceException`** (antes `GException0`) descartaba el mensaje de error.
4. **`OpenGD77Comms`** se ha eliminado. Era una copia sin usar del código de transferencia, con
   los mismos fallos de bucle infinito.
5. Se resolvieron los 21 avisos del compilador sin cambiar el comportamiento.

## 9. Observaciones

- `Settings.SUPER_PWD = "DT8168"` (`Settings.cs:111`): contraseña maestra heredada del CPS de
  Radioddity.
- `Settings.SaveCodeplugBinary`/`LoadCodeplugBinary` usan `BinaryFormatter`, que es inseguro con
  archivos no confiables. No se usan para abrir `.g77`, que pasa por `ByteToData`.
- Muchos métodos siguen con nombres genéricos (`method_N`, unos 500), sobre todo los auxiliares
  de UI dentro de cada formulario. Ver `herramientas/renombrador` para seguir renombrándolos de
  forma segura.

## 10. Dónde tocar para cambios habituales

| Quiero cambiar… | Archivo / símbolo |
|---|---|
| Límites de frecuencia por modelo | `Settings.UpdateFreqRangesForRadioType` |
| Pasos de frecuencia | `Settings.FREQ_STEP_1/2`, `RoundToNearestStep` |
| Lista de tonos CTCSS/DCS | `Settings.TONES_LIST` (constructor estático) |
| Niveles de potencia | `ChannelForm.LibreDMR_Power_E` + textos en `Language/CPS/*.xml` |
| Campos de un canal | `ChannelForm.ChannelOne` (respetando el layout binario) |
| Nº de canales/zonas/contactos | Constantes `CNT_*`/`NUM_*`. **Requiere que el firmware lo soporte** |
| Mapa de memoria | `Settings` (constructor estático), `OpenGD77Form` `READ_/WRITE_CODEPLUG` |
| Protocolo con la radio | `OpenGD77Form`: `sendCommand`, `ReadFlashOrEEPROMOrROMOrScreengrab`, `WriteFlash`, `WriteEEPROM` |
| Calibración | `CalibrationData`, `CalibrationForm`, `CalibrationBandControl` |
| Textos de la interfaz | `Language/CPS/*.xml` (sin recompilar) |

Para buscar en el código usa `rg` (ripgrep). Por ejemplo:

```bash
rg -n "TxFreq" codigo_descompilado/DMR
rg -n "Power_5W" codigo_descompilado
```
