using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_3.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.loaders
{
    class ZahtjevRezervacijeLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Zahtjev_rezervacije> UcitajZahtjevRezervacije(string datoteka)
        {
            List<Zahtjev_rezervacije> zahtjevi = new List<Zahtjev_rezervacije>();
            using (TextFieldParser csvReader = new TextFieldParser(datoteka))
            {
                csvReader.CommentTokens = new string[] { "#" };
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;

                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    Boolean greskaUZapisu = false;

                    string[] fields = csvReader.ReadFields();
                    greskaUZapisu = ProvjeraZapisa(fields);

                    if (!greskaUZapisu)
                    {
                        Zahtjev_rezervacije zahtjev = (Zahtjev_rezervacije)KreirajObjekt(fields);
                        zahtjevi.Add(zahtjev);
                    }
                }
            }
            return zahtjevi;
        }

        public static object KreirajObjekt(string[] fields)
        {
            Zahtjev_rezervacije zahtjev = new Zahtjev_rezervacije.ZahtjevBuilder(StringUInt(fields[0]), StringUDateTime(fields[1])).setTrajanjePriveza(StringUInt(fields[2])).build();
            return zahtjev;
        }

        public static Boolean ProvjeraZapisa(string[] fields)
        {
            if (fields == null)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " Prazan zapis", ""));
                return true;
            }
            else if (RedImaPraznihPodataka(fields))
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Zapis sadrži prazni podatak"));
                return true;
            }
            else if (fields.Length < 3 || fields.Length > 3)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Nispravna količina podataka u zapisu"));
                return true;
            }
            else if (StringUDateTime(fields[1]) == DateTime.MinValue)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Greška u zapisu vremena"));
                return true;
            }

            return false;
        }

        public static int StringUInt(string input)
        {
            int parsirano;
            if (int.TryParse(input, out parsirano))
            {
                return parsirano;
            }
            else
            {
                return 0;
            }
        }

        public static DateTime StringUDateTime(string input)
        {
            DateTime parsirano;
            if (DateTime.TryParse(input, out parsirano))
            {
                return parsirano;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static string IspisRedaGreske(string[] zapis)
        {
            string stringBuilder = "";
            foreach (string podatak in zapis)
            {
                stringBuilder += podatak + ";";
            }
            return stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        public static bool RedImaPraznihPodataka(string[] zapis)
        {
            foreach (string podatak in zapis)
            {
                if (podatak == null || podatak == " " || podatak == "") return true;
            }
            return false;
        }
    }
}