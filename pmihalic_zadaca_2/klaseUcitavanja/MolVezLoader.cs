using Microsoft.VisualBasic.FileIO;
using pmihalic_zadaca_2.klase;
using pmihalic_zadaca_2.klaseObjekata;
using pmihalic_zadaca_2.loaders;

namespace pmihalic_zadaca_2.klaseUcitavanja
{
    class MolVezLoader : UcitavanjeSucelje
    {
        public static BrodskaLukaMain brodskaLukaMain = BrodskaLukaMain.getInstance();
        public static List<MolVez> UcitajMoloveVezove(string datoteka, List<Vez> vezovi)
        {
            List<MolVez> moloviVezovi = new List<MolVez>();
            using (TextFieldParser csvReader = new TextFieldParser(datoteka))
            {
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    Boolean greskaUZapisu = false;

                    string[] fields = csvReader.ReadFields();
                    greskaUZapisu = ProvjeraZapisa(fields);

                    string[] fieldsVezovi = fields[1].Split(",");

                    List<int> idvezova = new List<int>();

                    foreach (string field in fieldsVezovi)
                    {
                        int idVez = int.Parse(field);

                        Boolean vezPostoji = false;

                        foreach (Vez v in vezovi)
                        {
                            if(idVez == v.Id)
                            {
                                vezPostoji = true;
                                break;
                            }
                        }

                        if(vezPostoji)idvezova.Add(idVez);
                        else
                        {
                            brodskaLukaMain.br_greske++;
                            Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Vez s ID-om '" + idVez + "' ne postoji!"));
                        }
                    }

                    if (!greskaUZapisu)
                    {
                        MolVez molVez = (MolVez)KreirajObjekt(fields, idvezova);

                        Boolean ponavljajuciID = false;
                        foreach (MolVez mv in moloviVezovi)
                        {
                            if (mv.IdMol == molVez.IdMol)
                            {
                                ponavljajuciID = true;
                                brodskaLukaMain.br_greske++;
                                Console.WriteLine(String.Format("{0,-10}{1,-65}{2}", "GRESKA " + brodskaLukaMain.br_greske, " ZAPIS: " + IspisRedaGreske(fields), " RAZLOG: Ponavljajući ID mola: " + molVez.IdMol));
                                break;
                            }
                        }
                        if (!ponavljajuciID) moloviVezovi.Add(molVez);
                    }

                }

            }
            return moloviVezovi;
        }

        public static object KreirajObjekt(string[] fields, List<int> idVezova)
        {
            MolVez molVez = new MolVez();
            molVez.IdMol = StringUInt(fields[0]);
            molVez.IdVezovi = idVezova;
            return molVez;
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
