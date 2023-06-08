using System.Formats.Asn1;
using System.Globalization;
using System;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Collections.Generic;
using System;
using pmihalic_zadaca_2.klase;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace pmihalic_zadaca_2.loaders
{
    class BrodLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Brod> UcitajBrodove(string datoteka)
        {
            List<Brod> brodovi = new List<Brod>();
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
                        Brod brod = (Brod)KreirajObjekt(fields);

                        Boolean ponavljajuciID = false;
                        foreach (Brod b in brodovi)
                        {
                            if (b.Id == brod.Id)
                            {
                                ponavljajuciID = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Ponavljajući ID broda: " + brod.Id));
                                break;
                            }
                        }
                        if (!ponavljajuciID) brodovi.Add(brod);
                    }

                }

            }
            return brodovi;
        }

        public static object KreirajObjekt(string[] fields)
        {
            Brod brod = new Brod();
            brod.Id = StringUInt(fields[0]);
            brod.Oznaka_broda = fields[1];
            brod.Naziv = fields[2];
            brod.Vrsta = fields[3];
            brod.Duljina = StringUDouble(fields[4]);
            brod.Sirina = StringUDouble(fields[5]);
            brod.Gaz = StringUDouble(fields[6]);
            brod.Maksimalna_brzina = StringUInt(fields[7]);
            brod.Kapacitet_putnika = StringUInt(fields[8]);
            brod.Kapacitet_osobnih_vozila = StringUInt(fields[9]);
            brod.Kapacitet_tereta = StringUInt(fields[10]);
            return brod;
        }

        public static bool ProvjeraZapisa(string[] fields)
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
            else if (fields.Length < 11 || fields.Length > 11)
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