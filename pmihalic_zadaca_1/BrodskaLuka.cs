using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_1.klase;
using pmihalic_zadaca_1.loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

public class BrodskaLukaMain
{
    private static BrodskaLukaMain instancaKlase; 

    public BrodskaLukaMain()
    {
    }
    public static BrodskaLukaMain getInstance()
    {
        if(instancaKlase == null) instancaKlase = new BrodskaLukaMain();
        return instancaKlase;
    }

    public static DateTime virtualnoVrijeme = DateTime.MinValue;
    public int br_greske = 0;
    public static List<Luka> luke = new List<Luka>();
    public static List<Vez> vezovi = new List<Vez>();
    public static List<Brod> brodovi = new List<Brod>();
    public static List<Raspored> raspored = new List<Raspored>();
    public static List<Zahtjev_rezervacije> zahtjevi = new List<Zahtjev_rezervacije>();
    private static Timer timer;

    static void Main(string[] args)
    {
        Boolean ispravno = false;

        if (args.Length > 0) ispravno = true;

        BrodLoader brodLoader = new();

        int pozicija = 0;
        while (args.Length > pozicija)
        {
            switch (args[pozicija])
            {
                case "-l":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            luke = LukaLoader.UcitajLuke(args[pozicija + 1]);
                            Console.WriteLine("--Učitano luka: " + luke.Count);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--Datoteka " + args[pozicija + 1] + " ne postoji!");
                            Environment.Exit(0);
                        }
                        break;
                    }
                case "-v":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            vezovi = VezLoader.UcitajVezove(args[pozicija + 1]);
                            Console.WriteLine("--Učitano vezova: " + vezovi.Count);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--Datoteka " + args[pozicija + 1] + " ne postoji!");
                            Environment.Exit(0);
                        }
                        break;
                    }
                case "-b":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            brodovi = BrodLoader.UcitajBrodove(args[pozicija + 1]);
                            Console.WriteLine("--Učitano brodova: " + brodovi.Count);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--Datoteka " + args[pozicija + 1] + " ne postoji!");
                            Environment.Exit(0);
                        }
                        break;
                    }
                case "-r":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            raspored = RasporedLoader.UcitajRaspored(args[pozicija + 1]);
                            Console.WriteLine("--Učitano elemenata rasporeda: " + raspored.Count);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--Datoteka " + args[pozicija + 1] + " ne postoji!");
                            Environment.Exit(0);
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Nepoznata naredba " + args[pozicija]);
                    Console.WriteLine("Sustav brodske luke nije pokrenut!\n");
                    Environment.Exit(0);
                    break;
            }

            pozicija += 2;
        }
        virtualnoVrijeme = luke[0].Virtualno_vrijeme;
        timer = new Timer(1000);
        timer.Elapsed += PovecajVirtualnoVrijeme;
        timer.Enabled = true;
        Console.WriteLine("Sustav brodske luke uspješno pokrenut!\n");
        PrikaziIzbornik();
    }

    private static void PovecajVirtualnoVrijeme(Object source, ElapsedEventArgs e)
    {
        virtualnoVrijeme = virtualnoVrijeme.AddSeconds(1);
    }
    private static void PrikaziIzbornik()
    {
        Console.WriteLine("Moguće aktivnosti: ");
        Console.WriteLine(" - pregled statusa vezova:              I");
        Console.WriteLine(" - promijena virtualnog vremena:        VR dd.mm.yyyy hh:mm:ss");
        Console.WriteLine(" - ispis vezova:                        V [vrsta_veza] [status] [datum_vrijeme_od] [datum_vrijeme_do]");
        Console.WriteLine(" - ucitavanje zahtjeva rezervacija:     UR [naziv_datoteke]");
        Console.WriteLine(" - zahtjev za privez broda:             ZD [ID_brod]");
        Console.WriteLine(" - zahtjev za privez broda s trajanjem: ZP [ID_brod] [trajanje_u_h]");
        Console.WriteLine(" - izlaz:                               Q");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------");

        string naredba = Console.ReadLine();
        string oznaka;
        string parametri = "";

        if (naredba.Length != 1)
        {
            oznaka = naredba.Split(" ")[0];
        }
        else oznaka = naredba;

        switch (oznaka)
        {
            case "I":
                PregledajStatusVezova();
                PrikaziIzbornik();
                break;
            case "VR":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1), "^[0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d\\. [012]\\d\\:[0-5]\\d\\:[0-5]\\d$").Success)
                {
                    PromijeniVirtualnoVrijeme(naredba.Substring(naredba.IndexOf(" ") + 1));
                }else Console.WriteLine("Krivi format argumenata za naredbu VR");
                PrikaziIzbornik();
                break;
            case "V":
                if(Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1), 
                    "^[A-Z][A-Z] [A-Z] [0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d\\. [012]\\d\\:[0-5]\\d\\:[0-5]\\d [0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d\\. [012]\\d\\:[0-5]\\d\\:[0-5]\\d$").Success)
                {
                    IspisZauzetihSlobodnih(naredba.Substring(naredba.IndexOf(" ") + 1));
                }else Console.WriteLine("Krivi format argumenata za naredbu V");
                PrikaziIzbornik();
                break;
            case "UR":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1), "^\\w+.csv$").Success)
                {
                    if (File.Exists(naredba.Split(" ")[1]))
                    {
                        UcitavanjeDatotekeRezervacija(naredba.Split(" ")[1]);
                    }
                    else
                    {
                        Console.WriteLine("--Datoteka " + naredba.Split(" ")[1] + " ne postoji!");
                    }
                }
                else Console.WriteLine("Krivi format argumenata za naredbu UR");
                PrikaziIzbornik();
                break;
            case "ZD":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1), "^\\d+$").Success)
                {
                    ZahtjevZaPrivezBrodaRezervacija(naredba.Split(" ")[1]);
                }
                else Console.WriteLine("Krivi format argumenata za naredbu ZD");
                PrikaziIzbornik();
                break;
            case "ZP":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1), "^\\d+ \\d+$").Success)
                {
                    ZahtjevZaPrivezBrodaSlobodan(naredba.Split(" ")[1], naredba.Split(" ")[2]);
                }
                else Console.WriteLine("Krivi format argumenata za naredbu ZP");
                PrikaziIzbornik();
                break;
            case "Q":
                Console.WriteLine("Prekinut rad programa");
                timer.Stop();
                timer.Dispose();
                break;
            default:
                instancaKlase.br_greske++;
                Console.WriteLine("GRESKA " + instancaKlase.br_greske + ": Nepoznata naredba, pokušajte ponovno");
                PrikaziIzbornik();
                break;
        }
    }

    private static void PregledajStatusVezova()
    {
        Console.WriteLine(virtualnoVrijeme);
        foreach (Vez vez in vezovi)
        {
            if (raspored.Count == 0)
            {
                Console.WriteLine(vez + " -> slobodan");
            } else if (raspored.Count > 0)
            {
                Boolean zauzet = false;
                foreach (Raspored ras in raspored)
                {
                    if (ras.Id_vez == vez.Id && ras.Dani_u_tjednu.Contains("" + (int)virtualnoVrijeme.DayOfWeek) &&
                        TimeOnly.Parse(virtualnoVrijeme.ToString("HH:mm")).IsBetween(ras.Vrijeme_od, ras.Vrijeme_do))
                    {
                        zauzet = true;
                    }
                }
                if (zauzet) Console.WriteLine(vez + " -> zauzet");
                else Console.WriteLine(vez + " -> slobodan");
            }
        }
    }

    private static void PromijeniVirtualnoVrijeme(String novoVrijemeString)
    {
        Console.WriteLine(virtualnoVrijeme);
        DateTime novoVrijeme;
        if (DateTime.TryParse(novoVrijemeString, out novoVrijeme))
        {
            virtualnoVrijeme = novoVrijeme;
            Console.WriteLine("Uspješno promijenjeno virtualno vrijeme");
        }
        else
        {
            Console.WriteLine("Krivi format vremena");
        }
    }

    private static void IspisZauzetihSlobodnih(String argumenti)
    {
        Console.WriteLine(virtualnoVrijeme);

        List<String> popisVezova = new List<String>();

        string status = argumenti.Split(" ")[1];
        if (status == "Z")
        {
            popisVezova = ZauzetiVezovi(argumenti);
        }
        if(status == "S")
        {
            popisVezova = SlobodniVezovi(argumenti);
        }

        foreach(String s in popisVezova)
        {
            Console.WriteLine(s);
        }
    }

    private static List<string> ZauzetiVezovi(string argumenti)
    {
        string vrsta_veza = argumenti.Split(" ")[0];
        DateOnly datum_od = DateOnly.Parse(argumenti.Split(" ")[2]);
        DateOnly datum_do = DateOnly.Parse(argumenti.Split(" ")[4]);
        DateTime datum_vrijeme_od = DateTime.Parse(argumenti.Split(" ")[2] + " " + argumenti.Split(" ")[3]);
        DateTime datum_vrijeme_do = DateTime.Parse(argumenti.Split(" ")[4] + " " + argumenti.Split(" ")[5]);
        TimeOnly vrijeme_od = TimeOnly.Parse(datum_vrijeme_od.ToString("HH:mm"));
        TimeOnly vrijeme_do = TimeOnly.Parse(datum_vrijeme_do.ToString("HH:mm"));
        TimeOnly pocetakDana = TimeOnly.MinValue;
        TimeOnly krajDana = TimeOnly.MaxValue;

        List<string> ispis = new List<string>();

        if (datum_vrijeme_do < datum_vrijeme_od)
        {
            Console.WriteLine("GRESKA " + instancaKlase.br_greske + ": Unesen početni datum-vrijeme veći od završnog!");
        }
        else
        {
            foreach (Vez vez in vezovi)
            {
                if (raspored.Count == 0)
                {
                    Console.WriteLine("Nije učitan raspored");
                }
                else if (raspored.Count > 0)
                {
                    foreach (Raspored ras in raspored)
                    {
                        DateOnly datum = new DateOnly();
                        for (datum = datum_od; datum <= datum_do; datum = datum.AddDays(1))
                        {
                            if (vez.Id == ras.Id_vez && vez.Vrsta == vrsta_veza && ras.Dani_u_tjednu.Contains("" + (int)datum.DayOfWeek))
                            {
                                if (datum_od == datum_do)
                                {
                                    if ((ras.Vrijeme_do.IsBetween(vrijeme_od, vrijeme_do) || ras.Vrijeme_od.IsBetween(vrijeme_od, vrijeme_do)) ||
                                    (vrijeme_do.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do) && vrijeme_od.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do)))
                                        ispis.Add("Oznaka veza: " + vez.Oznaka_veza + " zauzet: " + ras.Vrijeme_od + " - " + ras.Vrijeme_do);
                                }
                                else if (datum.DayOfWeek == datum_od.DayOfWeek)
                                {
                                    if (ras.Vrijeme_do.IsBetween(vrijeme_od, krajDana) || ras.Vrijeme_od.IsBetween(vrijeme_od, krajDana) ||
                                    (vrijeme_od.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do) && krajDana.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do)))
                                        ispis.Add("Oznaka veza: " + vez.Oznaka_veza + " zauzet: " + ras.Vrijeme_od + " - " + ras.Vrijeme_do);
                                }
                                else if (datum.DayOfWeek == datum_do.DayOfWeek)
                                {
                                    if (ras.Vrijeme_do.IsBetween(pocetakDana, vrijeme_do) || ras.Vrijeme_od.IsBetween(pocetakDana, vrijeme_do) ||
                                    (pocetakDana.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do) && vrijeme_do.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do)))
                                        ispis.Add("Oznaka veza: " + vez.Oznaka_veza + " zauzet: " + ras.Vrijeme_od + " - " + ras.Vrijeme_do);
                                }
                                else ispis.Add("Oznaka veza: " + vez.Oznaka_veza + " zauzet: " + ras.Vrijeme_od + " - " + ras.Vrijeme_do);
                            }
                        }
                    }
                }
            }
        }
        return ispis;
    }

    private static List<string> SlobodniVezovi(string argumenti)
    {
        string vrsta_veza = argumenti.Split(" ")[0];
        DateTime datum_vrijeme_od = DateTime.Parse(argumenti.Split(" ")[2] + " " + argumenti.Split(" ")[3]);
        DateTime datum_vrijeme_do = DateTime.Parse(argumenti.Split(" ")[4] + " " + argumenti.Split(" ")[5]);
        List<string> ispis = new List<string>();
        List<string> popisZauzetihVezovaString = ZauzetiVezovi(argumenti);

        bool zauzet = false;
        foreach (Vez v in vezovi)
        {
            zauzet = false;
            foreach (string vez in popisZauzetihVezovaString)
            {
                if (vez.Split(" ")[2] == v.Oznaka_veza)
                {
                    zauzet = true;
                }
            }
            if(!zauzet && v.Vrsta == vrsta_veza) ispis.Add("Oznaka veza: " + v.Oznaka_veza + " slobodan: " + datum_vrijeme_od + " - " + datum_vrijeme_do);
        }
        return ispis;
    }

    private static void UcitavanjeDatotekeRezervacija(String datoteka)
    {
        Console.WriteLine(virtualnoVrijeme);
        List<Zahtjev_rezervacije> ucitaniZahtjevi = ZahtjevRezervacijeLoader.UcitajZahtjevRezervacije(datoteka);
        Console.WriteLine("--Učitano zahtjeva rezervacije: " + ucitaniZahtjevi.Count);
        Console.WriteLine("-----------------------------------------------------------------------------------------------------");

        foreach(Zahtjev_rezervacije zahtjev in ucitaniZahtjevi)
        {
            ZahtjevOdobren(zahtjev.Id_brod, zahtjev.Datum_vrijeme_od, zahtjev.Trajanje_priveza_u_h);
        }
    }

    private static void ZahtjevZaPrivezBrodaRezervacija(String ID_brod)
    {
        Console.WriteLine(virtualnoVrijeme);

        int brodID;
        if (Int32.TryParse(ID_brod, out brodID))
        {
            DateTime datum_vrijeme_od = virtualnoVrijeme;

                if (raspored.Count == 0)
                {
                    Console.WriteLine("Nije učitan raspored");
                }
                else if (raspored.Count > 0)
                {
                    Boolean rezervirano = false;
                Raspored odgovarajucaRezervacija = new Raspored();
                    foreach (Raspored ras in raspored)
                    {
                        if (ras.Id_brod == brodID && ras.Dani_u_tjednu.Contains("" + (int)datum_vrijeme_od.DayOfWeek) &&
                            TimeOnly.Parse(datum_vrijeme_od.ToString("HH:mm")).IsBetween(ras.Vrijeme_od, ras.Vrijeme_do))
                        {
                            rezervirano = true;
                        Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(brodID, datum_vrijeme_od, "odobren");
                        zahtjevi.Add(zahtjev_Rezervacije);
                        odgovarajucaRezervacija = ras;
                        }
                    }
                if (rezervirano) Console.WriteLine("Zahtjev za privez odobren, rezervacija: " + odgovarajucaRezervacija);
                else {
                    Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(brodID, datum_vrijeme_od, "odbijen");
                    zahtjevi.Add(zahtjev_Rezervacije);
                    Console.WriteLine("Zahtjev odbijen");
                }
                }
        }
        else
        {
            Console.WriteLine("Krivi format ID_broda");
        }
    }

    private static void ZahtjevZaPrivezBrodaSlobodan(String ID_brod, String trajanje_u_h)
    {
        Console.WriteLine(virtualnoVrijeme);

        DateTime datumVrijemeDo = virtualnoVrijeme.AddHours(Int32.Parse(trajanje_u_h));

        Console.WriteLine(ZahtjevOdobren(Int32.Parse(ID_brod), virtualnoVrijeme, Int32.Parse(trajanje_u_h)));
    }

    private static String ZahtjevOdobren(int ID_brod,DateTime datumVrijeme, int trajanje_u_h)
    {
            Brod brod = new Brod();
            foreach(Brod b in brodovi)
            {
                if (b.Id == ID_brod) brod = b;
            }

            DateTime datumVrijemeDo = datumVrijeme.AddHours(trajanje_u_h);

            Boolean slobodan = false;
            String odgovor = "Odbijen zahtjev";
            
            List<string> popisSlobodnihVezovaZaIspis = SlobodniVezovi(VrstaVezaZaBrod(brod.Vrsta) + " S " + datumVrijeme + " "+ datumVrijemeDo);

            foreach (string vez in popisSlobodnihVezovaZaIspis)
            {
                foreach(Vez v in vezovi)
                {
                    if (vez.Split(" ")[2] == v.Oznaka_veza && v.Maksimalna_duljina >= brod.Duljina && 
                        v.Miksimalana_sirina >= brod.Sirina && v.Maksimalna_dubina >= brod.Gaz) {
                        slobodan = true;
                        odgovor = "Odobren zahtjev za spajanje na vez " + v.Oznaka_veza + " u terminu: " + datumVrijeme + " - " + datumVrijemeDo;
                        break;
                    }
                }
                if (slobodan) break;
            }

        if (slobodan)
        {
            Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(ID_brod, datumVrijeme, "odobren", trajanje_u_h);
            zahtjevi.Add(zahtjev_Rezervacije);
        }
        else
        {
            Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(ID_brod, datumVrijeme, "odbijen", trajanje_u_h);
            zahtjevi.Add(zahtjev_Rezervacije);
        }
        return odgovor;
    }

    private static Zahtjev_rezervacije KreirajZahtjev(int id_brod, DateTime datum_vrijeme_od, string status, int trajanje_priveza_u_h = 0)
    {
        Zahtjev_rezervacije zahtjev_Rezervacije;
        if(trajanje_priveza_u_h != 0)  zahtjev_Rezervacije = new Zahtjev_rezervacije.ZahtjevBuilder(id_brod, datum_vrijeme_od)
                                                                                    .setTrajanjePriveza(trajanje_priveza_u_h)
                                                                                    .setStatus(status)
                                                                                    .build();
        else zahtjev_Rezervacije = new Zahtjev_rezervacije.ZahtjevBuilder(id_brod, datum_vrijeme_od)
                                                          .setStatus(status)
                                                          .build();
        return zahtjev_Rezervacije;
    }
    public static bool DatumVrijemeJeIzmeđu(DateTime datum, DateTime manji, DateTime veci)
    {
        return (datum > manji && datum < veci);
    }

    public static string VrstaVezaZaBrod(string vrsta_broda)
    {
        switch (vrsta_broda)
        {
            case "TR":
            case "KA":
            case "KL":
            case "KR":
                {
                    return "PU";
                    break;
                }
            case "RI":
            case "TE":
                {
                    return "PO";
                    break;
                }
            case "JA":
            case "BR":
            case "RO":
                {
                    return "OS";
                    break;
                }
            default:
                return "greska";
                break;
        }
    }
}
