#!/usr/bin/env bash
# Aplica un mapa de renombrado al proyecto descompilado y comprueba que sigue compilando.
# Uso: herramientas/renombrador/renombrar.sh <mapa.txt>
set -euo pipefail
here="$(cd "$(dirname "$0")" && pwd)"
root="$(cd "$here/../.." && pwd)"
proj="$root/codigo_descompilado"
map="$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"
refs="$(mktemp)"
trap 'rm -f "$refs"' EXIT

dotnet build "$here/renombrador.csproj" -c Release -v quiet -nologo >/dev/null
dotnet msbuild "$proj/OpenGD77CPS.csproj" -t:ResolveAssemblyReferences -getItem:ReferencePath -p:Configuration=Release -nologo \
  | python3 -c "import json,sys; print('\n'.join(i['FullPath'] for i in json.load(sys.stdin)['Items']['ReferencePath']))" > "$refs"
dotnet "$here/bin/Release/net8.0/renombrador.dll" "$proj" "$refs" -- "$map"
dotnet build "$proj/OpenGD77CPS.csproj" -c Release -nologo -v quiet
