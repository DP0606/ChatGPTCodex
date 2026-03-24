# PX Prototype (.NET + Angular PrimeNG)

Prototip aplikacije koja:

1. prihvaća upload `.px` datoteke,
2. parsira datoteku preko biblioteke `PCAxis.Paxiom`,
3. sprema sadržaj u SQLite bazu.

## Struktura

- `PxPrototype.Api` - ASP.NET Core Web API + EF Core + SQLite
- `PxPrototype.Web` - Angular + PrimeNG frontend za upload i pregled učitanih setova

## Pokretanje backenda

```bash
cd PxPrototype.Api
dotnet restore
dotnet run
```

API endpointi:

- `POST /api/px/upload` (`multipart/form-data`, polje `file`)
- `GET /api/px/datasets`

## Pokretanje frontenda

```bash
cd PxPrototype.Web
npm install
npm start
```

Frontend očekuje backend na `https://localhost:5001`.

## Napomena o logičkom modelu iz DOCX-a

Budući da sadržaj dokumenta `Logicki_model_PX_baza_podataka.docx` nije bio dostupan u repozitoriju, implementiran je generički normalizirani model (`Dataset -> Variables -> Values + Observations`) kao polazna točka. Nakon dostave DOCX modela, entitete/migracije treba uskladiti 1:1 s formalnim modelom.

## NuGet napomena

Ako `dotnet restore` javlja grešku za `PCAxis.Paxiom`, koristi se paket `PCAxis.Core` (sadrži Paxiom API).

