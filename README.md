# Daily Infos - Napi információk konzolos alkalmazás

**Daily Infos** egy C# nyelven írt konzolos alkalmazás, amely minden nap hasznos személyes információkat jelenít meg a felhasználónak. Az alkalmazás támogatja az idézetek kezelését, az időjárás megjelenítését, névnapok és születésnapok listázását, valamint események visszaszámlálását és egy egyszerű TODO listát is tartalmaz.

## Fő funkciók

- 🌤 **Időjárás megjelenítése** az aktuális város alapján (OpenWeather API segítségével)
- 📅 **Mai névnapok listázása** (`nevnapok.json` alapján)
- 🎂 **Születésnap nyilvántartás** és közelgő születésnapok kijelzése (`szemelyek.json`)
- 📆 **Esemény visszaszámláló**, például vizsga vagy szülinap előtt hány nap van hátra (`esemeny.json`)
- ✅ **TODO lista** kezelése (feladat hozzáadás, listázás, törlés)
- 💬 **Napi idézet** véletlenszerűen (`idezetek.txt`), idézet hozzáadás
- 🏙️ Város módosítása és mentése (`varos.txt`)

## Fájlstruktúra

Az alkalmazás működéséhez az alábbi fájlok használatosak:

- `idezetek.txt` – Napi idézetek tárolása
- `todo.txt` – Feladatlista tárolása
- `varos.txt` – Mentett város az időjárás lekérdezéséhez
- `nevnapok.json` – Névnap adatbázis
- `szemelyek.json` – Születésnap adatbázis
- `esemeny.json` – Visszaszámlálásra váró események

## Példaképernyő

```text
=== Napi információk ===

Mai idézet:
"A siker kulcsa a kitartás."

Mai időjárás Budapest városban:
Hőmérséklet: 22°C, állapot: enyhe eső

Mai névnapok:
Gergely, Imola

Mai születésnapok:
Ma van születésnapja: Anna, Balázs

Közelgő születésnapok:
...

1. Új idézet hozzáadása
2. TODO lista
3. Időjárás új lekérdezés
4. Város módosítása
5. Visszaszámláló események kezelése
6. Születésnapok kezelése
7. Kilépés
````

## Használat

1. **API kulcs**: Az időjárás lekérdezéshez regisztrálj az [OpenWeather](https://openweathermap.org/) oldalon, és cseréld ki a `YOUR_API_KEY` értékét a saját kulcsodra a `IdőjárásKonzolraAsync` metódusban.
2. **Futtatás**: Fordítsd és futtasd a programot egy .NET-kompatibilis környezetben:

   ```bash
   dotnet run
   ```

## Követelmények

* [.NET SDK 6.0+](https://dotnet.microsoft.com/)
* Internetkapcsolat az időjárás API-hoz
* A szükséges fájlok (`*.json`, `*.txt`) megléte (üres fájlok indításkor is elegendőek)

## Tervek

* [ ] JSON fájlok validálása induláskor
* [ ] Idézetek kategorizálása
* [ ] GUI verzió (WinForms vagy WPF)

## Készítette

**[Csontos Kincső]**
2025

---

> Ha hasznosnak találtad, bátran csillagozd a projektet! ⭐
