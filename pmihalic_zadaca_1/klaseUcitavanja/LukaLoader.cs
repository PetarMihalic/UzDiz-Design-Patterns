using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_1.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_1.loaders
{
    class LukaLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Luka> UcitajLuke(string datoteka)
        {
            List<Luka> luke = new List<Luka>();
            using (TextFieldParser csvReader = new TextFieldParser(datoteka))
            {
                csvReader.CommentTokens = new string[] { "#" };
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    Boolean greskaUZapisu = false;

                    string[] fields = csvReader.ReadFields();
                    greskaUZapisu = ProvjeraZapisa(fields);

                    if (!greskaUZapisu)
                    {
                        Luka luka = (Luka)KreirajObjekt(fields);

                        bool ponavljajuci = false;
                        foreach (Luka l in luke)
                        {
                            if (l.Naziv == luka.Naziv)
                            {
                                ponavljajuci = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Ponavljajuća luka: " + luka.Naziv);
                                break;
                            }
                        }
                        if (!ponavljajuci) luke.Add(luka);
                    }
                }
            }
            return luke;
        }

        public static object KreirajObjekt(string[] fields)
        {
            Luka luka = new Luka();
            luka.Naziv = fields[0];
            luka.GPS_sirina = StringUDouble(fields[1]);
            luka.GPS_visina = StringUDouble(fields[2]);
            luka.Dubina_luke = StringUInt(fields[3]);
            luka.Ukupni_broj_putnickih_vezova = StringUInt(fields[4]);
            luka.Ukupni_broj_poslovnih_vezova = StringUInt(fields[5]);
            luka.Ukupni_broj_ostalih_vezova = StringUInt(fields[6]);
            luka.Virtualno_vrijeme = StringUDateTime(fields[7]);
            return luka;
        }

        public static Boolean ProvjeraZapisa(string[] fields)
        {
            if (fields == null)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("Greska " + brodskaLukaMain.br_greske + ": Prazan zapis");
                return true;
            }
            else if (RedImaPraznihPodataka(fields))
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Zapis sadrži prazni podatak");
                return true;
            }
            else if (fields.Length < 8 || fields.Length > 8)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Nispravna količina podataka u zapisu");
                return true;
            }
            else if (StringUDateTime(fields[7]) == DateTime.MinValue)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Greška u zapisu vremena");
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

        public static double StringUDouble(string input)
        {
            if (input.Contains(",")) input = input.Replace(',', '.');
            double parsirano;
            if (double.TryParse(input, out parsirano))
            {
                return parsirano;
            }
            else
            {
                return 0;
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