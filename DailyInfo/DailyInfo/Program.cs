using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static string idezetekPath = "idezetek.txt";
    static string todoPath = "todo.txt";

    static async Task Main()
    {
        string aktualisVaros = BetoltVaros();
        Koszontes();
        await IdőjárásKonzolraAsync(aktualisVaros);

        Console.WriteLine("\nMai névnapok:");
        MaiNevnapKiiras();

        Console.WriteLine("\nMai születésnapok:");
        Szuletesnapok();

        Console.WriteLine("\nKözelgő születésnapok:");
        KozelgoSzuletesnapok();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Napi információk ===\n");

            List<string> idezetek = BetoltIdzetek();

            if (idezetek.Count > 0)
            {
                Random rand = new Random();
                string napiIdezet = idezetek[rand.Next(idezetek.Count)];
                Console.WriteLine("Mai idézet:");
                Console.WriteLine($"\"{napiIdezet}\"\n");
            }
            else
            {
                Console.WriteLine("Nincsenek idézetek.\n");
            }

            await IdőjárásKonzolraAsync(aktualisVaros);

            Console.WriteLine("\nMai névnapok:");
            MaiNevnapKiiras();

            Console.WriteLine("\nMai születésnapok:");
            Szuletesnapok();

            Console.WriteLine("\nKözelgő születésnapok:");
            KozelgoSzuletesnapok();

            Console.WriteLine("\n1. Új idézet hozzáadása");
            Console.WriteLine("2. TODO lista");
            Console.WriteLine("3. Időjárás új lekérdezés");
            Console.WriteLine("4. Város módosítása");
            Console.WriteLine("5. Visszaszámláló események kezelése");
            Console.WriteLine("6. Születésnapok kezelése");
            Console.WriteLine("7. Kilépés");
            Console.Write("\nVálasztásod: ");
            string valasz = Console.ReadLine();

            switch (valasz)
            {
                case "1": UjIdezetHozzaadasa(); break;
                case "2": TodoMenu(); break;
                case "3": await IdőjárásKonzolraAsync(aktualisVaros); break;
                case "4": VarosModositas(); aktualisVaros = BetoltVaros(); break;
                case "5": VisszaszamlaloMenu(); break;
                case "6": SzuletesnapMenu(); break;
                case "7": return;

                default: Console.WriteLine("Érvénytelen választás."); break;
            }

            Console.WriteLine("\nNyomj Entert a folytatáshoz...");
            Console.ReadLine();
        }
    }


    static List<string> BetoltIdzetek()
    {
        if (File.Exists(idezetekPath))
            return new List<string>(File.ReadAllLines(idezetekPath));
        return new List<string>();
    }

    static void UjIdezetHozzaadasa()
    {
        Console.Write("\nÍrd be az új idézetet: ");
        string ujIdezet = Console.ReadLine();

        File.AppendAllText(idezetekPath, ujIdezet + Environment.NewLine);
        Console.WriteLine("Idézet mentve!");
    }

    static void TodoMenu()
    {
        while (true) {
            Console.WriteLine("\n=== TODO lista ===");
            Console.WriteLine("1. Feladat hozzáadása");
            Console.WriteLine("2. Feladatok listázása");
            Console.WriteLine("3. Feladat törlése");
            Console.WriteLine("4. Vissza");

            Console.Write("Választásod: ");
            string valasz = Console.ReadLine();

            switch (valasz)
            {
                case "1":
                    Console.Write("Új feladat: ");
                    string feladat = Console.ReadLine();
                    File.AppendAllText(todoPath, feladat + Environment.NewLine);
                    Console.WriteLine("Feladat mentve!");
                    break;

                case "2":
                    if (File.Exists(todoPath)) {
                        List<string> feladatok = new List<string>(File.ReadAllLines(todoPath));
                        Console.WriteLine("\nAktuális feladatok:");
                        for (int i = 0; i < feladatok.Count; i++)
                            Console.WriteLine($"{i + 1}. {feladatok[i]}");
                    } else {
                        Console.WriteLine("Nincsenek feladatok.");
                    }
                    break;

                case "3":
                    if (File.Exists(todoPath)) {
                        List<string> torlendo = new List<string>(File.ReadAllLines(todoPath));
                        Console.Write("Melyik feladatot töröljem (szám)? ");
                        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= torlendo.Count) {
                            torlendo.RemoveAt(index - 1);
                            File.WriteAllLines(todoPath, torlendo);
                            Console.WriteLine("Feladat törölve.");
                        } else Console.WriteLine("Érvénytelen szám.");
                    } else {
                        Console.WriteLine("Nincs mit törölni.");
                    }
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Érvénytelen választás.");
                    break;
            }
        }
    }

    static async Task IdőjárásKonzolraAsync(string varos)
    {
        string apiKey = "YOUR_API_KEY";
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={varos}&appid={apiKey}&units=metric&lang=hu";

        HttpClient kliens = new HttpClient();
        try {
            string valasz = await kliens.GetStringAsync(url);
            JsonDocument doc = JsonDocument.Parse(valasz);
            using (doc)
            {
                string leiras = doc.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
                double homerseklet = doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble();

                Console.WriteLine($"Mai időjárás {varos} városban:");
                Console.WriteLine($"Hőmérséklet: {homerseklet}°C, állapot: {leiras}");
            }
        } catch (Exception ex) {
            Console.WriteLine("Nem sikerült lekérni az időjárást: " + ex.Message);
        }
    }

    static string varosPath = "varos.txt";

    static string BetoltVaros()
    {
        if (File.Exists(varosPath))
            return File.ReadAllText(varosPath).Trim();

        Console.Write("Kérlek add meg a városod nevét az időjáráshoz: ");
        string varos = Console.ReadLine().Trim();
        File.WriteAllText(varosPath, varos);
        return varos;
    }

    static void VarosModositas()
    {
        Console.Write("\nAdd meg az új város nevét: ");
        string ujVaros = Console.ReadLine().Trim();
        File.WriteAllText(varosPath, ujVaros);
        Console.WriteLine("Város frissítve!");
    }

    static void MaiNevnapKiiras()
    {
        string jsonPath = "nevnapok.json";
        if (!File.Exists(jsonPath)) {
            Console.WriteLine("A névnapok fájl nem található.");
            return;
        }

        string json = File.ReadAllText(jsonPath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        JsonDocument doc = JsonDocument.Parse(json);

        string maiDatum = DateTime.Now.ToString("MM-dd");
        List<string> nevnaposok = new List<string>();

        foreach (var elem in doc.RootElement.GetProperty("names").EnumerateArray()) {
            string nev = elem[0].GetString();
            string[] dates = elem[1].GetString().Split(',');
            foreach (string date in dates)
            {
                string trimmedDate = date.Trim();
                string datum = DateTime.Parse(trimmedDate).ToString("MM-dd");
                if (datum == maiDatum)
                {
                    nevnaposok.Add(nev);
                    break;
                }
            }
        }

        if (nevnaposok.Count > 0) {
            Console.WriteLine(string.Join(", ", nevnaposok));
        } else {
            Console.WriteLine("Ma nincs névnapos a listában.");
        }
    }
    static void VisszaszamlaloEsemennyel()
    {
        string path = "esemeny.json";
        if (!File.Exists(path))
        {
            Console.WriteLine("Nincs esemény megadva (esemeny.json).");
            return;
        }

        try {
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            List<Esemeny> esemenyek = JsonSerializer.Deserialize<List<Esemeny>>(json, options);

            if (esemenyek == null || esemenyek.Count == 0) {
                Console.WriteLine("Nincsenek események a fájlban.");
                return;
            }

            Console.WriteLine("\n=== Események listája ===");
            foreach (var esemeny in esemenyek) {
                if (string.IsNullOrWhiteSpace(esemeny.Nev) || esemeny.Datum == default) {
                    Console.WriteLine("Hibás vagy hiányos esemény adatok.");
                    continue;
                }

                int napok = (esemeny.Datum - DateTime.Today).Days;
                if (napok >= 0) {
                    Console.WriteLine($"{esemeny.Nev} eseményig még {napok} nap van hátra.");
                } else {
                    Console.WriteLine($"{esemeny.Nev} esemény már megtörtént ({Math.Abs(napok)} napja).");
                }
            }
        } catch (JsonException ex) {
            Console.WriteLine($"Hiba történt az események feldolgozása során: {ex.Message}");
        }
    }

    class Esemeny
    {
        public string Nev { get; set; }
        public DateTime Datum { get; set; }
    }

    static void Szuletesnapok()
    {
        string jsonPath = "szemelyek.json";
        if (!File.Exists(jsonPath))
        {
            Console.WriteLine("Nincs születésnap fájl (szemelyek.json).");
            return;
        }

        try {
            string json = File.ReadAllText(jsonPath);
            List<Dictionary<string, string>> szemelyek = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

            if (szemelyek == null || szemelyek.Count == 0)
            {
                Console.WriteLine("Nincs adat a születésnap fájlban.");
                return;
            }

            string mai = DateTime.Today.ToString("MM-dd");
            List<string> maSzuletesnaposok = new List<string>();

            foreach (var ember in szemelyek)
            {
                if (ember.TryGetValue("name", out string nev) && ember.TryGetValue("birthday", out string birthday))
                {
                    string datum = DateTime.Parse(birthday).ToString("MM-dd");
                    if (datum == mai)
                    {
                        maSzuletesnaposok.Add(nev);
                    }
                }
            }

            if (maSzuletesnaposok.Count > 0)
            {
                Console.WriteLine("Ma van születésnapja: " + string.Join(", ", maSzuletesnaposok));
            } else {
                Console.WriteLine("Ma nincs születésnapos.");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Hiba történt a fájl feldolgozása során: {ex.Message}");
        }
    }
    static void VisszaszamlaloMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Visszaszámláló események ===");
            Console.WriteLine("1. Esemény hozzáadása");
            Console.WriteLine("2. Események megtekintése");
            Console.WriteLine("3. Vissza");

            Console.Write("Választásod: ");
            string valasz = Console.ReadLine();

            switch (valasz)
            {
                case "1":
                    Console.Write("Esemény neve: ");
                    string nev = Console.ReadLine();
                    Console.Write("Esemény dátuma (YYYY-MM-DD): ");
                    string datum = Console.ReadLine();

                    try
                    {
                        var ujEsemeny = new Esemeny
                        {
                            Nev = nev,
                            Datum = DateTime.Parse(datum)
                        };

                        string path = "esemeny.json";
                        List<Esemeny> esemenyek = new List<Esemeny>();

                        if (File.Exists(path))
                        {
                            string json = File.ReadAllText(path);

                            // Check if the JSON is an array or a single object
                            if (json.TrimStart().StartsWith("["))
                            {
                                esemenyek = JsonSerializer.Deserialize<List<Esemeny>>(json) ?? new List<Esemeny>();
                            }
                            else
                            {
                                var singleEsemeny = JsonSerializer.Deserialize<Esemeny>(json);
                                if (singleEsemeny != null)
                                    esemenyek.Add(singleEsemeny);
                            }
                        }

                        esemenyek.Add(ujEsemeny);

                        string ujJson = JsonSerializer.Serialize(esemenyek, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(path, ujJson);

                        Console.WriteLine("Esemény mentve!");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Hibás dátumformátum. Próbáld újra.");
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Hiba történt a JSON feldolgozása során: {ex.Message}");
                    }
                    break;

                case "2":
                    VisszaszamlaloEsemennyel();
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Érvénytelen választás.");
                    break;
            }
        }
    }
    static void SzuletesnapMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Születésnapok ===");
            Console.WriteLine("1. Személy hozzáadása");
            Console.WriteLine("2. Vissza");

            Console.Write("Választásod: ");
            string valasz = Console.ReadLine();

            switch (valasz)
            {
                case "1":
                    Console.Write("Személy neve: ");
                    string nev = Console.ReadLine();
                    Console.Write("Születésnap (YYYY-MM-DD): ");
                    string szuletesnap = Console.ReadLine();
                    string jsonPath = "szemelyek.json";

                    try
                    {
                        List<Dictionary<string, string>> szemelyek = new List<Dictionary<string, string>>();

                        if (File.Exists(jsonPath))
                        {
                            string json = File.ReadAllText(jsonPath);
                            szemelyek = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json) ?? new List<Dictionary<string, string>>();
                        }

                        szemelyek.Add(new Dictionary<string, string> {
                        { "name", nev },
                        { "birthday", szuletesnap }
                    });

                        string ujJson = JsonSerializer.Serialize(szemelyek, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(jsonPath, ujJson);

                        Console.WriteLine("Születésnap mentve!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Hiba történt a mentés során: {ex.Message}");
                    }
                    break;

                case "2":
                    return;

                default:
                    Console.WriteLine("Érvénytelen választás.");
                    break;
            }
        }
    }

    static void Koszontes()
    {
        int ora = DateTime.Now.Hour;

        if (ora >= 5 && ora < 12) {
            Console.WriteLine("Jó reggelt Kincső!");
        } else if (ora >= 12 && ora < 18) {
            Console.WriteLine("Szép délutánt Kincső!");
        } else {
            Console.WriteLine("Jó estét Kincső!");
        }
    }
    static void KozelgoSzuletesnapok()
    {
        string jsonPath = "szemelyek.json";
        if (!File.Exists(jsonPath)) {
            Console.WriteLine("Nincs születésnap fájl (szemelyek.json).");
            return;
        }

        try {
            string json = File.ReadAllText(jsonPath);
            List<Dictionary<string, string>> szemelyek = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

            if (szemelyek == null || szemelyek.Count == 0) {
                Console.WriteLine("Nincs adat a születésnap fájlban.");
                return;
            }

            DateTime today = DateTime.Today;
            DateTime nextWeek = today.AddDays(7);
            List<string> kozelgoSzuletesnaposok = new List<string>();

            foreach (var ember in szemelyek) {
                if (ember.TryGetValue("name", out string nev) && ember.TryGetValue("birthday", out string birthday)) {
                    DateTime szuletesnap = DateTime.Parse(birthday);
                    DateTime ideiSzuletesnap = new DateTime(today.Year, szuletesnap.Month, szuletesnap.Day);

                    if (ideiSzuletesnap >= today && ideiSzuletesnap <= nextWeek) {
                        kozelgoSzuletesnaposok.Add($"{nev} ({ideiSzuletesnap:MM-dd})");
                    }
                }
            }

            if (kozelgoSzuletesnaposok.Count > 0) {
                Console.WriteLine("Közelgő születésnapok: " + string.Join(", ", kozelgoSzuletesnaposok));
            } else {
                Console.WriteLine("Nincs közelgő születésnap.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba történt a fájl feldolgozása során: {ex.Message}");
        }
    }

}
