using pmihalic_zadaca_2.ChainOfResponsibility;
using pmihalic_zadaca_2.klase;
using pmihalic_zadaca_2.klaseObjekata;
using pmihalic_zadaca_2.klaseUcitavanja;
using pmihalic_zadaca_2.loaders;
using pmihalic_zadaca_2.Visitor;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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
    public static List<Mol> molovi = new List<Mol>();
    public static List<MolVez> moloviVezovi = new List<MolVez>();
    public static List<Kanal> kanali = new List<Kanal>();
    public static List<DnevnikRada> dnevnikRada = new List<DnevnikRada>();
    private static Timer timer;

    public static Boolean redni_br = false;
    public static Boolean zaglavlje = false;
    public static Boolean podnozje = false;

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
                            vezovi = VezLoader.UcitajVezove(args[pozicija + 1], luke);
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
                case "-m":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            molovi = MolLoader.UcitajMolove(args[pozicija + 1]);
                            Console.WriteLine("--Učitano molova: " + molovi.Count);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--Datoteka " + args[pozicija + 1] + " ne postoji!");
                            Environment.Exit(0);
                        }
                        break;
                    }
                case "-mv":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            moloviVezovi = MolVezLoader.UcitajMoloveVezove(args[pozicija + 1], vezovi);
                            Console.WriteLine("--Učitano molova-vezova: " + moloviVezovi.Count);
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--Datoteka " + args[pozicija + 1] + " ne postoji!");
                            Environment.Exit(0);
                        }
                        break;
                    }
                case "-k":
                    {
                        if (File.Exists(args[pozicija + 1]))
                        {
                            kanali = KanalLoader.UcitajKanale(args[pozicija + 1]);
                            Console.WriteLine("--Učitano kanala: " + kanali.Count);
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
        Console.WriteLine("     ---------------------------------------- Moguće aktivnosti -------------------------------------------");
        Console.WriteLine("     | pregled statusa vezova:              I                                                             |");
        Console.WriteLine("     | promijena virtualnog vremena:        VR dd.mm.yyyy hh:mm:ss                                        |");
        Console.WriteLine("     | ispis vezova:                        V [vrsta_veza] [status] [datum_vrijeme_od] [datum_vrijeme_do] |");
        Console.WriteLine("     | ucitavanje zahtjeva rezervacija:     UR [naziv_datoteke]                                           |");
        Console.WriteLine("     | zahtjev za privez broda:             ZD [ID_brod]                                                  |");
        Console.WriteLine("     | zahtjev za privez broda s trajanjem: ZP [ID_brod] [trajanje_u_h]                                   |");
        Console.WriteLine("     | spajanja broda na kanal:             F [ID_brod] [kanal] [Q]                                       |");
        Console.WriteLine("     | uređivanje tablice:                  T [Z | P | RB]                                                |");
        Console.WriteLine("     | zauzeti vezovi po vrsti u vremenu:   ZA [vrijeme]                                                  |");
        Console.WriteLine("     | raspodjela putnika po brodovima:     RP [broj_putnika]                                             |");
        Console.WriteLine("     | izlaz:                               Q                                                             |");
        Console.WriteLine("     ------------------------------------------------------------------------------------------------------");

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
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1).Trim(), "^[0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d\\. [012]\\d\\:[0-5]\\d\\:[0-5]\\d$").Success)
                {
                    PromijeniVirtualnoVrijeme(naredba.Substring(naredba.IndexOf(" ") + 1).Trim());
                }else Console.WriteLine("Krivi format argumenata za naredbu VR");
                PrikaziIzbornik();
                break;
            case "V":
                if(Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1).Trim(), 
                    "^[A-Z][A-Z] [A-Z] [0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d\\. [012]\\d\\:[0-5]\\d\\:[0-5]\\d [0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d\\. [012]\\d\\:[0-5]\\d\\:[0-5]\\d$").Success)
                {
                    IspisZauzetihSlobodnih(naredba.Substring(naredba.IndexOf(" ") + 1).Trim());
                }else Console.WriteLine("Krivi format argumenata za naredbu V");
                PrikaziIzbornik();
                break;
            case "UR":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1).Trim(), "^\\w+.csv$").Success)
                {
                    if (File.Exists(naredba.Split(" ")[1].Trim()))
                    {
                        UcitavanjeDatotekeRezervacija(naredba.Split(" ")[1].Trim());
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
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1).Trim(), "^\\d+$").Success)
                {
                    ZahtjevZaPrivezBrodaRezervacija(naredba.Split(" ")[1].Trim());
                }
                else Console.WriteLine("Krivi format argumenata za naredbu ZD");
                PrikaziIzbornik();
                break;
            case "ZP":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1).Trim(), "^\\d+ \\d+$").Success)
                {
                    ZahtjevZaPrivezBrodaSlobodan(naredba.Split(" ")[1], naredba.Split(" ")[2]);
                }
                else Console.WriteLine("Krivi format argumenata za naredbu ZP");
                PrikaziIzbornik();
                break;
            case "F":
                if (Regex.Match(naredba.Trim(), "^F \\d+ \\d+ ?Q?$").Success)
                {
                    if(naredba.Trim().Contains("Q")) 
                        SpajanjeBrodaNaKanal(int.Parse(naredba.Trim().Split(" ")[1]), int.Parse(naredba.Trim().Split(" ")[2]), naredba.Trim().Split(" ")[3]);
                    else
                        SpajanjeBrodaNaKanal(int.Parse(naredba.Trim().Split(" ")[1]), int.Parse(naredba.Trim().Split(" ")[2]));
                }
                else Console.WriteLine("Krivi format argumenata za naredbu F");
                PrikaziIzbornik();
                break;
            case "T":
                if (Regex.Match(naredba.Trim(), "^T(\\s\\w+)*").Success)
                {
                    Console.WriteLine(virtualnoVrijeme);
                    zaglavlje = false;
                    redni_br = false;
                    podnozje = false;
                    String opcije = naredba.Substring(naredba.IndexOf(" ") + 1).Trim();
                    if (opcije.Contains("Z")) zaglavlje = true;
                    if (opcije.Contains("RB")) redni_br = true;
                    if (opcije.Contains("P")) podnozje = true;
                }
                else Console.WriteLine("Krivi format argumenata za naredbu T");
                PrikaziIzbornik();
                break;
            case "ZA":
                if (Regex.Match(naredba.Substring(naredba.IndexOf(" ") + 1).Trim(), "^[0-3]\\d\\.[0-1]\\d\\.[12]\\d\\d\\d. [012]\\d\\:[0-5]\\d$").Success)
                {
                    IspisBrojaZauzetihVezova(naredba.Substring(naredba.IndexOf(" ") + 1).Trim());
                }
                else Console.WriteLine("Krivi format argumenata za naredbu ZP");
                PrikaziIzbornik();
                break;
            case "RP":
                if (Regex.Match(naredba.Trim(), "^RP \\d+$").Success)
                {
                    RasporediPutnike(naredba.Substring(naredba.IndexOf(" ") + 1).Trim());
                }
                else Console.WriteLine("Krivi format argumenata za naredbu RP");
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

        String formatZaglavlja = "{0,-10}|{1,-10}|{2,-10}|{3,-10}";
        String formatPodataka = "{0,10}|{1,-10}|{2,-10}|{3,-10}";
        String formatPodnozja = "{0,-10}|{1,10}|{2,10}|{3,10}";

        int redni_broj = 0;

        if (redni_br)
        {
            formatZaglavlja = "{4,-10}|" + formatZaglavlja;
            formatPodataka = "{4,10}|" + formatPodataka;
            formatPodnozja = "{4,10}|" + formatPodnozja;
        }
        if (zaglavlje)
        {
            String naslov = "Status vezova";
            String linije = String.Format(formatZaglavlja, "----------", "----------", "----------", "----------", "----------");
            Console.WriteLine(new String(' ', (linije.Length/2)-(naslov.Length/2))+naslov);
            Console.WriteLine(linije);
            Console.WriteLine(String.Format(formatZaglavlja, "ID", "Oznaka", "Vrsta", "Status", "RB"));
            Console.WriteLine(linije);
        }

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
                
                if (zauzet) Console.WriteLine(String.Format(formatPodataka, vez.Id, vez.Oznaka_veza, vez.Vrsta, "zauzet", ++redni_broj + "."));
                else Console.WriteLine(String.Format(formatPodataka, vez.Id, vez.Oznaka_veza, vez.Vrsta, "slobodan", ++redni_broj + "."));
            }
        }
        if(podnozje) Console.WriteLine(String.Format(formatPodnozja, "----------", "----------", "UKUPNO", redni_broj, "----------"));
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
        List<string> ispis = new List<string>();

        String formatZaglavlja = "{0,-10}|{1,-10}|{2,-10}|{3,-10}";
        String formatPodataka = "{0,-10}|{1,10}|{2,10}|{3,10}";
        String formatPodnozja = "{0,-10}|{1,10}|{2,10}|{3,10}";

        int redni_broj = 0;

        if (redni_br)
        {
            formatZaglavlja = "{4,-10}|" + formatZaglavlja;
            formatPodataka = "{4,10}|" + formatPodataka;
            formatPodnozja = "{4,10}|" + formatPodnozja;
        }
        if (zaglavlje)
        {
            String naslov = "Zauzeti vezovi";
            String linije = String.Format(formatZaglavlja, "----------", "----------", "----------", "----------", "----------");
            ispis.Add(new String(' ', (linije.Length / 2) - (naslov.Length / 2)) + naslov);
            ispis.Add(linije);
            ispis.Add(String.Format(formatZaglavlja, "Oznaka", "Datum", "Vrijeme od", "Vrijeme do", "RB"));
            ispis.Add(linije);
        }

        string vrsta_veza = argumenti.Split(" ")[0];
        DateOnly datum_od = DateOnly.Parse(argumenti.Split(" ")[2]);
        DateOnly datum_do = DateOnly.Parse(argumenti.Split(" ")[4]);
        DateTime datum_vrijeme_od = DateTime.Parse(argumenti.Split(" ")[2] + " " + argumenti.Split(" ")[3]);
        DateTime datum_vrijeme_do = DateTime.Parse(argumenti.Split(" ")[4] + " " + argumenti.Split(" ")[5]);
        TimeOnly vrijeme_od = TimeOnly.Parse(datum_vrijeme_od.ToString("HH:mm"));
        TimeOnly vrijeme_do = TimeOnly.Parse(datum_vrijeme_do.ToString("HH:mm"));
        TimeOnly pocetakDana = TimeOnly.MinValue;
        TimeOnly krajDana = TimeOnly.MaxValue;

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
                                        ispis.Add(String.Format(formatPodataka, vez.Oznaka_veza, datum, ras.Vrijeme_od, ras.Vrijeme_do, ++redni_broj + "."));
                                }
                                else if (datum.DayOfWeek == datum_od.DayOfWeek)
                                {
                                    if (ras.Vrijeme_do.IsBetween(vrijeme_od, krajDana) || ras.Vrijeme_od.IsBetween(vrijeme_od, krajDana) ||
                                    (vrijeme_od.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do) && krajDana.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do)))
                                        ispis.Add(String.Format(formatPodataka, vez.Oznaka_veza, datum, ras.Vrijeme_od, ras.Vrijeme_do, ++redni_broj + "."));
                                }
                                else if (datum.DayOfWeek == datum_do.DayOfWeek)
                                {
                                    if (ras.Vrijeme_do.IsBetween(pocetakDana, vrijeme_do) || ras.Vrijeme_od.IsBetween(pocetakDana, vrijeme_do) ||
                                    (pocetakDana.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do) && vrijeme_do.IsBetween(ras.Vrijeme_od, ras.Vrijeme_do)))
                                        ispis.Add(String.Format(formatPodataka, vez.Oznaka_veza, datum, ras.Vrijeme_od, ras.Vrijeme_do, ++redni_broj + "."));
                                }
                                else ispis.Add(String.Format(formatPodataka, vez.Oznaka_veza, datum, ras.Vrijeme_od, ras.Vrijeme_do, ++redni_broj + "."));
                            }
                        }
                    }
                }
            }
        }
        if (podnozje) ispis.Add(String.Format(formatPodnozja, "----------", "----------", "UKUPNO", redni_broj, "----------"));
        return ispis;
    }

    private static List<string> SlobodniVezovi(string argumenti)
    {
        List<string> ispis = new List<string>();

        String formatZaglavlja = "{0,-10}|{1,-20}|{2,-20}";
        String formatPodataka = "{0,-10}|{1,20}|{2,20}";
        String formatPodnozja = "{0,-10}|{1,20}|{2,20}";

        int redni_broj = 0;

        if (redni_br)
        {
            formatZaglavlja = "{3,-10}|" + formatZaglavlja;
            formatPodataka = "{3,10}|" + formatPodataka;
            formatPodnozja = "{3,10}|" + formatPodnozja;
        }
        if (zaglavlje)
        {
            String naslov = "Slobodni vezovi";
            String linije = String.Format(formatZaglavlja, "----------", "--------------------", "--------------------", "----------");
            ispis.Add(new String(' ', (linije.Length / 2) - (naslov.Length / 2)) + naslov);
            ispis.Add(linije);
            ispis.Add(String.Format(formatZaglavlja, "Oznaka", "Datum vrijeme od", "Datum vrijeme do", "RB"));
            ispis.Add(linije);

        }

        string vrsta_veza = argumenti.Split(" ")[0];
        DateTime datum_vrijeme_od = DateTime.Parse(argumenti.Split(" ")[2] + " " + argumenti.Split(" ")[3]);
        DateTime datum_vrijeme_do = DateTime.Parse(argumenti.Split(" ")[4] + " " + argumenti.Split(" ")[5]);
        List<string> popisZauzetihVezovaString = ZauzetiVezovi(argumenti);

        bool zauzet = false;
        foreach (Vez v in vezovi)
        {
            zauzet = false;
            foreach (string vez in popisZauzetihVezovaString)
            {
                string[] dijelovi = vez.Split("|");
                if(dijelovi.Length >= 2)
                {
                    if (vez.Split("|")[0].Trim() == v.Oznaka_veza || vez.Split("|")[1].Trim() == v.Oznaka_veza)
                    {
                        zauzet = true;
                    }
                }
            }
            if(!zauzet && v.Vrsta == vrsta_veza) ispis.Add(String.Format(formatPodataka, v.Oznaka_veza, datum_vrijeme_od, datum_vrijeme_do, ++redni_broj + "."));
        }
        if (podnozje) ispis.Add(String.Format(formatPodnozja, "----------", "UKUPNO", redni_broj, "----------"));
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

        Brod brod = new Brod();
        foreach (Brod b in brodovi)
        {
            if (b.Id == int.Parse(ID_brod)) brod = b;
        }

        Kanal kanal = new Kanal("");
        Boolean spojen = false;
        foreach (Kanal k in kanali)
        {
            if (k.SpojeniBrodovi.Contains(brod))
            {
                spojen = true;
                kanal = k;
                break;
            }
        }
        if (!spojen)
        {
            Console.WriteLine("Brod " + brod.Id + " nije spojen ni na jedan kanal!");
        }
        else
        {
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
                            odgovarajucaRezervacija = ras;
                            Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(brodID, datum_vrijeme_od, "odobren");
                            zahtjevi.Add(zahtjev_Rezervacije);
                            DateTime datum_vrijeme_do = datum_vrijeme_od.Add(ras.Vrijeme_do-ras.Vrijeme_od);
                            DnevnikRada noviZapis = new DnevnikRada(brodID, virtualnoVrijeme, true, datum_vrijeme_od, datum_vrijeme_do, zahtjev_Rezervacije);
                            dnevnikRada.Add(noviZapis);
                        }
                    }
                    if (rezervirano)
                    {
                        foreach (Brod b in kanal.SpojeniBrodovi)
                        {
                            Console.WriteLine("Poruka za brod " + b.Id + ": Zahtjev za privez odobren, rezervacija: " + odgovarajucaRezervacija);
                        }
                    }
                    else
                    {
                        Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(brodID, datum_vrijeme_od, "odbijen");
                        zahtjevi.Add(zahtjev_Rezervacije);
                        DnevnikRada noviZapis = new DnevnikRada(brodID, virtualnoVrijeme, false, datum_vrijeme_od, datum_vrijeme_od, zahtjev_Rezervacije);
                        dnevnikRada.Add(noviZapis);
                        foreach (Brod b in kanal.SpojeniBrodovi)
                        {
                            Console.WriteLine("Poruka za brod " + b.Id + ": Zahtjev za spajanje broda " + brodID + " odbijen");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Krivi format ID_broda");
            }
        }
    }

    private static void ZahtjevZaPrivezBrodaSlobodan(String ID_brod, String trajanje_u_h)
    {
        Console.WriteLine(virtualnoVrijeme);

        DateTime datumVrijemeDo = virtualnoVrijeme.AddHours(int.Parse(trajanje_u_h));

        ZahtjevOdobren(Int32.Parse(ID_brod), virtualnoVrijeme, Int32.Parse(trajanje_u_h));
    }

    private static void ZahtjevOdobren(int ID_brod,DateTime datumVrijeme, int trajanje_u_h)
    {
        DateTime datumVrijemeDo = datumVrijeme.AddHours(trajanje_u_h);

        Brod brod = new Brod();
        Boolean postojiBrod = false;
        foreach (Brod b in brodovi)
        {
            if (b.Id == ID_brod)
            {
                brod = b;
                postojiBrod = true;
            }
        }

        Kanal kanal = new Kanal("");
        Boolean spojen = false;
        foreach (Kanal k in kanali)
        {
            if (k.SpojeniBrodovi.Contains(brod))
            {
                spojen = true;
                kanal = k;
                break;
            }
        }
        if (!postojiBrod)
        {
            Console.WriteLine("Ne postoji brod s ID-om: " + ID_brod);
            return;
        }

        String odgovor = "Brod " + brod.Id + " nije spojen ni na jedan kanal!";
        if (!spojen)
        {
            Console.WriteLine(odgovor);
            return;
        }
        else
        {
            odgovor = String.Format("Zahtjev odbijen za brod {0,-16} | termin:{1,20} - {2,-20}", ID_brod, datumVrijeme, datumVrijemeDo);

            List<string> popisSlobodnihVezovaZaIspis = SlobodniVezovi(VrstaVezaZaBrod(brod.Vrsta) + " S " + datumVrijeme + " " + datumVrijemeDo);

            string najboljiVez = "";
            double minRazlika = 1000;
            double razlika;

            foreach (string vez in popisSlobodnihVezovaZaIspis)
            {
                foreach (Vez v in vezovi)
                {
                    if(vez.Split("|").Length >= 2)
                    {
                        if ((vez.Split("|")[0].Trim() == v.Oznaka_veza || vez.Split("|")[1].Trim() == v.Oznaka_veza) &&
                        v.Maksimalna_duljina >= brod.Duljina && v.Miksimalana_sirina >= brod.Sirina && v.Maksimalna_dubina >= brod.Gaz)
                        {
                            razlika = v.Maksimalna_duljina - brod.Duljina + v.Miksimalana_sirina - brod.Sirina + v.Maksimalna_dubina - brod.Gaz;
                            if (razlika < minRazlika)
                            {
                                minRazlika = razlika;
                                najboljiVez = v.Oznaka_veza;
                            }
                        }
                    }
                }
            }

            if (najboljiVez != "")
            {
                odgovor = String.Format("Zahtjev odobren za brod {0,-3} na vez {1,-5} | termin:{2,20} - {3,-20}", ID_brod, najboljiVez, datumVrijeme, datumVrijemeDo);
                Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(ID_brod, datumVrijeme, "odobren", trajanje_u_h);
                zahtjevi.Add(zahtjev_Rezervacije);
                DnevnikRada noviZapis = new DnevnikRada(ID_brod, virtualnoVrijeme, true, datumVrijeme, datumVrijemeDo, zahtjev_Rezervacije);
                dnevnikRada.Add(noviZapis);
            }
            else
            {
                Zahtjev_rezervacije zahtjev_Rezervacije = KreirajZahtjev(ID_brod, datumVrijeme, "odbijen", trajanje_u_h);
                zahtjevi.Add(zahtjev_Rezervacije);
                DnevnikRada noviZapis = new DnevnikRada(ID_brod, virtualnoVrijeme, false, datumVrijeme, datumVrijemeDo, zahtjev_Rezervacije);
                dnevnikRada.Add(noviZapis);
            }
            kanal.setPoruka(odgovor);
            return;
        }
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

    private static void IspisBrojaZauzetihVezova(String datumVrijemeString)
    {
        Console.WriteLine(virtualnoVrijeme);
        DateTime datumVrijeme = DateTime.Parse(datumVrijemeString);
        int dan = (int)datumVrijeme.DayOfWeek;
        TimeOnly vrijeme = TimeOnly.Parse(datumVrijemeString.Split(" ")[1]);
        var vezPO = new VezPO();
        var vezPU = new VezPU();
        var vezOS = new VezOS();
        var brojac = new BrojacVrstaVezaVisitor();
        List<int> zauzetiVezovi = new List<int>();
        foreach (Raspored ras in raspored){
            if (ras.Dani_u_tjednu.Contains(dan.ToString()) && vrijeme.IsBetween(ras.Vrijeme_od,ras.Vrijeme_do)) zauzetiVezovi.Add(ras.Id_vez);
        }
        foreach (int i in zauzetiVezovi)
        {
            foreach(Vez v in vezovi)
            {
                if (v.Id == i) {
                    if (v.Vrsta == "PO") vezPO.Accept(brojac);
                    if (v.Vrsta == "PU") vezPU.Accept(brojac);
                    if (v.Vrsta == "OS") vezOS.Accept(brojac);
                    break;
                }
            }
        }
        String formatZaglavlja = "{0,-10}|{1,-10}|{2,-13}";
        String formatPodataka = "{0,-10}|{1,-10}|{2,13}";
        String formatPodnozja = "{0,-10}|{1,10}|{2,13}";

        int redni_broj = 0;

        if (redni_br)
        {
            formatZaglavlja = "{3,-10}|" + formatZaglavlja;
            formatPodataka = "{3,10}|" + formatPodataka;
            formatPodnozja = "{3,10}|" + formatPodnozja;
        }
        if (zaglavlje)
        {
            String naslov = "Broj zauzetih vezova prema vrsti";
            String linije = String.Format(formatZaglavlja, "----------", "----------", "-------------", "----------");
            Console.WriteLine(new String(' ', (linije.Length / 2) - (naslov.Length / 2)) + naslov);
            Console.WriteLine(linije);
            Console.WriteLine(String.Format(formatZaglavlja, "Vrsta veza", "Oznaka", "Broj zauzetih", "RB"));
            Console.WriteLine(linije);
        }
        Console.WriteLine(String.Format(formatPodataka, "Putnički", "PU", vezPU.brojacVezova, ++redni_broj + "."));
        Console.WriteLine(String.Format(formatPodataka, "Poslovni", "PO", vezPO.brojacVezova, ++redni_broj + "."));
        Console.WriteLine(String.Format(formatPodataka, "Ostali", "OS", vezOS.brojacVezova, ++redni_broj + "."));

        if (podnozje) Console.WriteLine(String.Format(formatPodnozja, "----------", "UKUPNO", redni_broj, "----------"));
    }

    private static void SpajanjeBrodaNaKanal(int IDbrod, int frekvencija, string izlazak = "")
    {
        Console.WriteLine(virtualnoVrijeme);
        Brod brod = new Brod();
        Kanal kanal = new Kanal("");
        Boolean postojiBrod = false;
        Boolean postojiKanal = false;
        Boolean spojen = false;
        foreach (Brod b in brodovi)
        {
            if (b.Id == IDbrod)
            {
                postojiBrod = true;
                brod = b;
            }
        }
        foreach (Kanal k in kanali)
        {
            if (k.SpojeniBrodovi.Contains(brod))
            {
                spojen = true;
                if(izlazak != "Q") Console.WriteLine("Brod " + IDbrod + " je već spojen na kanal " + k.Frekvencija);
            }
            if (k.Frekvencija == frekvencija) 
            {
                postojiKanal = true;
                kanal = k;
            }
        }
        if(!postojiBrod) Console.WriteLine("Ne postoji brod s ID-om: " + IDbrod);
        else if(!postojiKanal) Console.WriteLine("Ne postoji kanal s frekvencijom: " + frekvencija);
        else
        {
            if (izlazak !="Q" && !spojen)
            {
                if (kanal.MaksimalanBroj > kanal.SpojeniBrodovi.Count)
                {
                    kanal.SpojeniBrodovi.Add(brod);
                    kanal.Attach(brod);
                    kanal.setPoruka("Brod s id " + IDbrod + " spojen na kanal " + frekvencija);
                }
                else Console.WriteLine("Nema više mjesta za spajanje na kanal " + frekvencija);
            }
            else if(izlazak == "Q" && spojen)
            {
                kanal.setPoruka("Brod s id " + IDbrod + " odjavljen s kanala " + frekvencija);
                kanal.SpojeniBrodovi.Remove(brod);
                kanal.Detach(brod);
            }
            else if(izlazak == "Q" && !spojen)
            {
                Console.WriteLine("Brod nije spojen na kanal " + frekvencija);
            }
        }
    }

    private static void RasporediPutnike(string brojPutnikaString)
    {
        Console.WriteLine(virtualnoVrijeme);
        Lanac lanacRasporedivanja = new Lanac();
        int brojPutnika = int.Parse(brojPutnikaString);
        if(brojPutnika != 0)
        {
            lanacRasporedivanja.lanac1.raspodjeli(new Putnici(brojPutnika), brodovi);
        }
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
