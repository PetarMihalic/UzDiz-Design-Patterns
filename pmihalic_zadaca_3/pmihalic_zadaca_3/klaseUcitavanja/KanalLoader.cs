using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_3.klase;
using pmihalic_zadaca_3.klaseObjekata;
using pmihalic_zadaca_3.loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmihalic_zadaca_3.klaseUcitavanja
{
    class KanalLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Kanal> UcitajKanale(string datoteka)
        {
            List<Kanal> kanali = new List<Kanal>();
            using (TextFieldParser csvReader = new TextFieldParser(datoteka))
            {
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    Boolean greskaUZapisu = false;

                    string[] fields = csvReader.ReadFields();
                    greskaUZapisu = ProvjeraZapisa(fields);

                    if (!greskaUZapisu)
                    {
                        Kanal kanal = (Kanal)KreirajObjekt(fields);

                        Boolean ponavljajuciID = false;
                        foreach (Kanal k in kanali)
                        {
                            if (k.IdKanal == kanal.IdKanal)
                            {
                                ponavljajuciID = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Ponavljajući ID kanala: " + kanal.IdKanal);
                                break;
                            }
                            if (k.Frekvencija == kanal.Frekvencija)
                            {
                                ponavljajuciID = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine("GRESKA " + brodskaLukaMain.br_greske + ", ZAPIS: " + IspisRedaGreske(fields) + ", RAZLOG: Ponavljajuća frekvencija kanala: " + kanal.Frekvencija);
                                break;
                            }
                        }
                        if (!ponavljajuciID) kanali.Add(kanal);
                    }

                }

            }
            return kanali;
        }

        public static object KreirajObjekt(string[] fields)
        {
            Kanal kanal = new Kanal("");
            kanal.IdKanal = StringUInt(fields[0]);
            kanal.Frekvencija = StringUInt(fields[1]);
            kanal.MaksimalanBroj = StringUInt(fields[2]);
            kanal.SpojeniBrodovi = new List<Brod>();
            return kanal;
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
            else if (fields.Length < 3 || fields.Length > 3)
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
