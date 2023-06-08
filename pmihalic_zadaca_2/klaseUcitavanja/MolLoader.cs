using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_2.klaseObjekata;
using pmihalic_zadaca_2.loaders;

namespace pmihalic_zadaca_2.klaseUcitavanja
{
     class MolLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<Mol> UcitajMolove(string datoteka)
        {
            List<Mol> molovi = new List<Mol>();
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
                        Mol mol = (Mol)KreirajObjekt(fields);

                        Boolean ponavljajuciID = false;
                        foreach (Mol m in molovi)
                        {
                            if (m.IdMol == mol.IdMol)
                            {
                                ponavljajuciID = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Ponavljajući ID mola: " + mol.IdMol));
                                break;
                            }
                        }
                        if (!ponavljajuciID) molovi.Add(mol);
                    }

                }

            }
            return molovi;
        }

        public static object KreirajObjekt(string[] fields)
        {
            Mol mol = new Mol();
            mol.IdMol = StringUInt(fields[0]);
            mol.Naziv = fields[1];
            return mol;
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
            else if (fields.Length < 2 || fields.Length > 2)
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
