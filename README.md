# Haladó szoftverfejlesztés 2026271 félév.

# Projektek letöltése

A repository heti bontásban tartalmazza a feladatokat és projekteket.

A mappastruktúra így fog alakulni:

```text
.
├── 1-week/
│   ├── Project1/
│   └── Project2/
├── 2-week/
│   ├── ConsoleApp1/
│   └── ConsoleApp2/
└── 3-week/
    └── ExampleProject/
```

Egy héten belül több, egymástól független projekt is található. Ezek lehetnek például különálló .NET Console alkalmazások, saját `.csproj` fájllal.

## Teljes repository klónozása

A legegyszerűbb megoldás a teljes repository klónozása:

```bash
git clone <REPOSITORY_URL>
cd <REPOSITORY_NAME>
```

Ez letölti az összes hetet és az azokhoz tartozó projekteket.

## Csak egy adott hét vagy projekt letöltése

A Git alapértelmezetten nem támogatja egy repository tetszőleges almappájának külön `git clone` paranccsal történő klónozását.

Ha csak egy adott hétre vagy projektre van szükség, használható a Git **sparse-checkout** funkciója.

Például csak a `2-week` mappa letöltéséhez:

```bash
git clone --filter=blob:none --no-checkout <REPOSITORY_URL>
cd <REPOSITORY_NAME>

git sparse-checkout init --cone
git sparse-checkout set 2-week
git checkout
```

Ezután a munkakönyvtárban csak a kiválasztott `2-week` mappa jelenik meg.

Egy konkrét projekt is kiválasztható:

```bash
git sparse-checkout set 2-week/ConsoleApp1
```

Több mappa vagy projekt egyszerre is megadható:

```bash
git sparse-checkout set 1-week/Project1 3-week/ExampleProject
```

## Projekt futtatása

Ha az adott mappa egy önálló .NET projektet tartalmaz, lépj be a projekt könyvtárába:

```bash
cd 2-week/ConsoleApp1
```

Majd futtasd:

```bash
dotnet run
```

Vagy először külön buildelheted:

```bash
dotnet build
dotnet run
```

> **Megjegyzés:** a pontos mappanevek és projektek hetente eltérhetnek. Egy héten belül több egymástól független alkalmazás is lehet.

