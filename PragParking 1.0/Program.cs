using System;


namespace PragParking_1._0
{
    class Program
    {
        static void Main()
        {
            string[] parkhus = new string[100];
            parkhus[0] = "*M*AAA111*M*AAA222";

            while (1 > 0)
            {

                int val = 0;
                string fordonsinfo = "";
                string regnummer = "";
                string mc = "*M*";
                string bil = "*B*";
                int plats = 0;
                int nyplats = 0;


                Console.WriteLine("\n\tVÄLKOMMEN TILL PRAG PARKING!\n" +
                    "\nVad vill du göra?");
                Console.WriteLine("\n 1. Parkera. 2. Hämta ut fordon. 3. Flytta till ny plats. 4. Sök efter fordon");
                val = int.Parse(Console.ReadLine());

                switch (val)
                {
                    case 1: // Parkera
                        {
                            Console.WriteLine("\nÄr det en:\n\n 1. Bil\n 2. Motorcykel");
                            val = int.Parse(Console.ReadLine());
                            switch (val)
                            {
                                case 1:
                                    {
                                        fordonsinfo += bil;
                                        Console.Write("\nRegnummmer: ");
                                        regnummer = Console.ReadLine();
                                        if (InputLength(regnummer))
                                        {
                                            fordonsinfo += regnummer;
                                            park(fordonsinfo, parkhus);
                                            receipt(regnummer, find(regnummer, parkhus));
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Felaktigt regnummer");
                                            break;
                                        }
                                    }
                                case 2:
                                    {
                                        fordonsinfo += mc;
                                        Console.Write("\nRegnummmer: ");
                                        regnummer = Console.ReadLine();
                                        if (InputLength(regnummer))
                                        {
                                            fordonsinfo += regnummer;
                                            parkmc(fordonsinfo, parkhus);
                                            receipt(regnummer, (contains(regnummer, parkhus)));
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Felaktigt regnummer");
                                            break;
                                        }
                                    }
                            }
                            break;
                        }
                    case 2: // Hämta ut
                        {
                            //find(regnummer, parkhus);
                            Console.Write("\nVilket Regnummer vill du hitta: ");
                            regnummer = Console.ReadLine();
                            Console.WriteLine("\nFordonet finns på plats: {0}.\n\n | Kör ut den till kunden |\n", (contains(regnummer, parkhus) + 1));
                            remove(contains(regnummer, parkhus), regnummer, parkhus);
                            break;
                        }
                    case 3: // flytta 
                        {
                            Console.WriteLine("\nVilket fordon vill du flytta?");
                            Console.Write("Regnummer: ");
                            regnummer = Console.ReadLine();
                            plats = find(regnummer, parkhus);
                            Console.WriteLine("\nFordonet står på plats: {0}\n", plats + 1);
                            Console.WriteLine("\nVilken plats vill du flytta den till?");
                            Console.Write("Parkeringplats: ");
                            nyplats = int.Parse(Console.ReadLine());
                            move(plats, nyplats, parkhus, regnummer);
                            receipt(regnummer, find(regnummer, parkhus));
                            break;
                        }
                    case 4: //sök
                        {
                            Console.Write("\nVilket regnummer vill du söka efter?\n" +
                                "Regnummer: ");
                            regnummer = Console.ReadLine();
                            if (contains(regnummer, parkhus) >= 0)
                            {
                                Console.WriteLine("\nFordonet står på plats: {0}\n", (contains(regnummer, parkhus)+1));
                                break;
                            }
                            else { Console.WriteLine("Hittar inte regnummret i systemet"); break; }
                        }
                }
            }
        }

        static void park(string fordonsinfo, string[] parkhus) // Method för att fylla en parkplats
        {
            string[] checkempty = new string[2];

            for (int i = 0; i < parkhus.Length; i++)
            {
                if (parkhus[i] == checkempty[0] || parkhus[i] == "")
                {
                    parkhus[i] = fordonsinfo;
                    fordonsinfo=fordonsinfo.Remove(0, 3);
                    Console.WriteLine("Plats {0} är ledig. Kör {1} dit", (i + 1), fordonsinfo);
                    break;

                }
                else if (i == 99) // det sista som kan hända
                {
                    Console.WriteLine("Tyvärr är parkeringarna fulla. Vill du testa å frigöra plats?"); //Lägg in optimeringsruting här.
                    // goto optimeringsmethod?
                }
            }
        }
        static void parkmc(string fordonsinfo, string[] parkhus)
        {
            string mc = "";
            string[] checkempty = new string[2];
            for (int i = 0; i < parkhus.Length; i++)
            {

                if (parkhus[i] == checkempty[0])
                {
                    parkhus[i] = fordonsinfo;
                    break;
                }

                else if (parkhus[i].Substring(0, 3) == "*M*" && parkhus[i].LastIndexOf("*") < 4)
                {
                    parkhus[i] = parkhus[i] + mc + fordonsinfo;
                    break;
                }
            }
        }
        static int find(string regnummer, string[] parkhus)
        {
            string regnummerbil = "*B*" + regnummer;
            string regnummermc = "*M*" + regnummer;

            int platsinfo = Array.IndexOf(parkhus, regnummerbil);                      //Hittar platsen

            if (platsinfo == -1)                                                        //söker först efter en bil
            {
                platsinfo = Array.IndexOf(parkhus, regnummermc);                    //Sen mc  (Hittar inte motorcykel just nu.
                if (platsinfo == -1)
                {
                    platsinfo = contains(regnummer, parkhus);
                    if (platsinfo == -1)
                    {
                        Console.WriteLine("Hittar tyvärr inte regnummret...");
                        return 0;
                    }
                }
            }

            return platsinfo; // Får tillbaka vilken plats den befinner sig på
        }
        static void remove(int plats, string regnummer, string[] parkhus)
        {
            string mirror = "";
            if (parkhus[plats].Length < 13) // om den innehåller bara en bil. 
            {
                parkhus[plats] = null;
                Console.WriteLine("Pakeringsplats: {0}. Är nu tom.", plats + 1);
            }
            else if (parkhus[plats].Length >= 13) // om det står två motorcyklar parkerade.
            {
                mirror += "*M*" + regnummer;
                if (parkhus[plats].StartsWith(mirror))
                {
                    parkhus[plats] = parkhus[plats].Remove(0, mirror.Length);
                    Console.WriteLine("{0} har flyttats ur systemet.", regnummer);
                }
                else if (parkhus[plats].EndsWith(mirror))
                {
                    int pos = parkhus[plats].Length;
                    int pos2 = mirror.Length;
                    parkhus[plats] = parkhus[plats].Remove((pos - pos2), 9); // -1 för att hitta rätt position tror jag.
                    Console.WriteLine("{0} hämtas ut från plats {1}. Kvar är {2}", regnummer, plats, parkhus[plats]);
                }
            }
        } 
        static void receipt(string regnummer, int plats) // Kvitto att ge till kund
        {
            Console.WriteLine("\n**Kvitto**" +
                "\n|| Fordon * {0} *  parkeras på plats: * {1} * ||\n" +
                "**Kvitto**", regnummer, plats + 1);
        }
        static void move(int plats, int nyplats, string[] parkhus, string regnummer)
        {
            nyplats -= 1; // Användarsiffra fås in. Ta bort ett för att matcha i systemet.

            string mirrorregnummer = "";
            string newmirror = parkhus[nyplats];
            bool ismc = false;
            ismc = CheckIfMc(regnummer, parkhus);


            if (parkhus[nyplats] == null)
            {
                if (ismc == true)
                {
                    mirrorregnummer = "*M*" + regnummer;
                }
                else
                {
                    mirrorregnummer = "*B*" + regnummer;
                }
                parkhus[nyplats] += regnummer;
                remove(plats, regnummer, parkhus);
                Console.WriteLine("Fordon ska flyttas från plats * {0} *. Till plats: * {1} *", plats + 1, nyplats + 1);
            }


            else if (ismc == true && newmirror.StartsWith("*M*") && newmirror.Length < 14) // då är det bara en motorcykel.
            {
                parkhus[nyplats] += "*M*" + regnummer;
                remove(plats, regnummer, parkhus);
            }
            else
            {
                Console.WriteLine("Den platsen är tyvärr upptagen..");
            }



        }
        static int contains(string regnummer, string[] parkhus)
        {

            bool plats = false;
            string[] checkempy = new string[2];

            for (int i = 0; i < parkhus.Length; i++)
            {
                if (parkhus[i] != checkempy[0])
                {
                    plats = parkhus[i].Contains(regnummer);
                    if (plats == true)
                    {
                        return i;
                    }
                }

                else continue;
            }


            return -1;


        }
        static bool CheckIfMc(string regnummer, string[] parkhus)
        {
            string[] checkempty = new string[2];
            int plats = contains(regnummer, parkhus);
            if (parkhus[plats].StartsWith("*B*") || parkhus[plats] == checkempty[0])
            {
                return false;
            }
            else return true;
        }
        static bool InputLength(string regnummer)
        {
            if (regnummer.Length < 11)
            {
                return true;
            }
            else return false;
        }
    }
}