using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_2.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_2.loaders
{
    class VezLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static int broj_putnickih_vezova = 0;
        public static int broj_poslovnih_vezova = 0;
        public static int broj_ostalih_vezova = 0;
        public static List<Vez> UcitajVezove(string datoteka, List<Luka> luka)
        {
            List<Vez> vezovi = new List<Vez>();
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

                    if (!greskaUZapisu)
                    {
                        Boolean greska = false;

                        Vez vez = (Vez)KreirajObjekt(fields);

                        greska = ProvjeriPopunjenostLuke(vez, luka[0], fields);
                        
                        if (!greska) { 
                            if (luka[0].Dubina_luke < vez.Maksimalna_dubina) {
                                greska = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Vez je dublji od luke: " + vez.Id));
                            }
                            else
                            {
                                foreach (Vez v in vezovi)
                                {
                                    if (v.Id == vez.Id)
                                    {
                                        greska = true;
                                        brodskaLukaMain.br_greske++;
                                        Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Ponavljajući ID veza: " + vez.Id));
                                        break;
                                    }
                                }
                            }
                        }
                        if (!greska) vezovi.Add(vez);
                    }

                }
            }
            return vezovi;
        }
        public static object KreirajObjekt(string[] fields)
        {
            Vez vez = new Vez();
            vez.Id = StringUInt(fields[0]);
            vez.Oznaka_veza = fields[1];
            vez.Vrsta = fields[2];
            vez.Cijena_veza_po_satu = StringUInt(fields[3]);
            vez.Maksimalna_duljina = StringUDouble(fields[4]);
            vez.Miksimalana_sirina = StringUDouble(fields[5]);
            vez.Maksimalna_dubina = StringUDouble(fields[6]);
            return vez;
        }

        public static Boolean ProvjeriPopunjenostLuke(Vez vez, Luka luka, string[] fields)
        {
            switch (vez.Vrsta)
            {
                case "PU":
                    {
                        broj_putnickih_vezova++;
                        if (luka.Ukupni_broj_putnickih_vezova < broj_putnickih_vezova)
                        {
                            brodskaLukaMain.br_greske++;
                            Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Luka '" + luka.Naziv + "' nema više mjesta za putničke vezove"));
                            return true;
                        }
                        break;
                    }
                case "PO":
                    {
                        broj_poslovnih_vezova++;
                        if (luka.Ukupni_broj_poslovnih_vezova < broj_poslovnih_vezova)
                        {
                            brodskaLukaMain.br_greske++;
                            Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Luka '" + luka.Naziv + "' nema više mjesta za poslovne vezove"));
                            return true;
                        }
                        break;
                    }
                case "OS":
                    {
                        broj_ostalih_vezova++;
                        if (luka.Ukupni_broj_ostalih_vezova < broj_ostalih_vezova)
                        {
                            brodskaLukaMain.br_greske++;
                            Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Luka '" + luka.Naziv + "' nema više mjesta za ostale vezove"));
                            return true;
                        }
                        break;
                    }
                default:
                    {
                        brodskaLukaMain.br_greske++;
                        Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Nepoznata vrsta veza: " + vez.Vrsta));
                        return true;
                        break;
                    }
            }
            return false;
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
            else if (fields.Length < 7 || fields.Length > 7)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Neispravna količina podataka u zapisu"));
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