using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_1.klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_1.loaders
{
    class VezLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Vez> UcitajVezove(string datoteka)
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
                        Vez vez = (Vez)KreirajObjekt(fields);

                        Boolean ponavljajuciID = false;
                        foreach (Vez v in vezovi)
                        {
                            if (v.Id == vez.Id)
                            {
                                ponavljajuciID = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Ponavljajući ID veza: " + vez.Id);
                                break;
                            }
                        }
                        if (!ponavljajuciID) vezovi.Add(vez);
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
        public static Boolean ProvjeraZapisa(string[] fields)
        {
            if (fields == null)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ": Prazan zapis");
                return true;
            }
            else if (RedImaPraznihPodataka(fields))
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Zapis sadrži prazni podatak");
                return true;
            }
            else if (fields.Length < 7 || fields.Length > 7)
            {
                brodskaLukaMain.br_greske++;
                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Neispravna količina podataka u zapisu");
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