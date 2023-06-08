using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_2.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.loaders
{
    class RasporedLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Raspored> UcitajRaspored(string datoteka)
        {
            List<Raspored> raspored = new List<Raspored>();
            using (TextFieldParser csvReader = new TextFieldParser(datoteka))
            {
                csvReader.CommentTokens = new string[] { "#" };
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    Boolean greskaUZapisu = false;

                    string[] fields = csvReader.ReadFields();
                    greskaUZapisu = ProvjeraZapisa(fields);

                    if(!greskaUZapisu)
                    {
                        Raspored red_rasporeda = (Raspored)KreirajObjekt(fields);

                        bool ponavljajuci = false;
                        foreach (Raspored r in raspored)
                        {
                            if (red_rasporeda.Id_brod == r.Id_brod && red_rasporeda.Id_vez == r.Id_vez && red_rasporeda.Vrijeme_od == r.Vrijeme_od && red_rasporeda.Vrijeme_do == r.Vrijeme_do && red_rasporeda.Dani_u_tjednu == r.Dani_u_tjednu)
                            {
                                ponavljajuci = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Ponavljajući red rasporeda: " + red_rasporeda));
                                break;
                            }
                        }
                        if (!ponavljajuci) raspored.Add(red_rasporeda);
                    }

                }

            }
            return raspored;
        }

        public static object KreirajObjekt(string[] fields)
        {
            Raspored red_rasporeda = new Raspored();
            red_rasporeda.Id_vez = StringUInt(fields[0]);
            red_rasporeda.Id_brod = StringUInt(fields[1]);
            red_rasporeda.Dani_u_tjednu = fields[2];
            red_rasporeda.Vrijeme_od = StringUTimeOnly(fields[3]);
            red_rasporeda.Vrijeme_do = StringUTimeOnly(fields[4]);
            return red_rasporeda;
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
            else if (fields.Length < 5 || fields.Length > 5)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Nispravna količina podataka u zapisu\n"));
                return true;
            }
            else if (StringUTimeOnly(fields[3]) == TimeOnly.MinValue || StringUTimeOnly(fields[4]) == TimeOnly.MinValue)
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

        public static TimeOnly StringUTimeOnly(string input)
        {
            TimeOnly parsirano;
            if (TimeOnly.TryParse(input, out parsirano))
            {
                return parsirano;
            }
            else
            {
                return TimeOnly.MinValue;
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