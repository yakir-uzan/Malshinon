using System;


namespace Malshinon
{
    class Menu
    {
        public static void Manu()
        {
            bool flag = true;

            while (flag)
            {
                Console.Clear();

                Console.WriteLine(" "); ;
                Console.WriteLine("=== MENU ===");
                Console.WriteLine("1. Submit New Intel Report");
                Console.WriteLine("2. Search Person by Name or Secret Code");
                Console.WriteLine("3. Show Secret Code of Person");
                Console.WriteLine("4. Import Intel Reports from CSV");
                Console.WriteLine("5. List Potential Agents");
                Console.WriteLine("6. List Dangerous Targets");
                Console.WriteLine("7. Show Active Alerts");
                Console.WriteLine("8. Exit");
                Console.WriteLine("Choose an option: ");
                Console.WriteLine(" ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":

                        break;
                    case "2":

                        break;
                    case "3":

                        break;
                    case "4":

                        break;
                    case "5":

                        break;
                    case "6":

                        break;
                    case "7":

                        break;
                    case "8":
                        flag = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}