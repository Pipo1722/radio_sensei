# Renombrador (Roslyn)

Renombra símbolos del proyecto descompilado (`method_12`, `Class13`, parámetros con nombres
basura...) con análisis semántico. Cada referencia se actualiza, aunque haya decenas de
`method_0` en clases distintas. Por eso no se debe usar `sed` para esto.

## Uso

```bash
herramientas/renombrador/renombrar.sh herramientas/renombrador/mapas/mi-mapa.txt
```

En VS Code: *Terminal → Run Task → "renombrar símbolos"*.

El script:
1. compila la herramienta;
2. pide a MSBuild las referencias reales del proyecto;
3. aplica el mapa;
4. recompila `OpenGD77CPS`.

Si un renombrado rompe la compilación, **no escribe nada**. Requisitos: .NET SDK 8, bash y Python 3.
En Windows, ejecútalo desde Git Bash o WSL.

## Formato del mapa

```
Tipo => NuevoTipo                    # renombra el tipo y su archivo .cs
Tipo::miembro => nuevoNombre         # campo, método, propiedad o tipo anidado
Tipo::metodo(parametro) => nuevo     # parámetro o parámetro genérico
DMR.ZoneForm::method_1 => Init       # nombre completo si hay ambigüedad
```

Los mapas de `mapas/01..03` ya están aplicados y sirven de registro de lo hecho. Volver a
ejecutarlos falla, porque los nombres antiguos ya no existen. Para seguir, parte de `mapas/nuevo.txt`.

Para encontrar candidatos:

```bash
rg -n '\b(s?method_[0-9]+)\(' codigo_descompilado -g '*.cs'
```
