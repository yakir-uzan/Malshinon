using System;


namespace Malshinon
{
    class Menu
    {
        public static void Manu()
        {
            MalshinonDAL dal = new MalshinonDAL();
            bool flag = true;

            while (flag)
            {
                Console.Clear();

                Console.WriteLine(" "); ;
                Console.WriteLine("=== MENU ===");
                Console.WriteLine("1. Submit New Intel Report");
                Console.WriteLine("2. Search Person by full Name");
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
                        Console.WriteLine("Enter first name:");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("Enter last name:");
                        string lastName = Console.ReadLine();
                        Console.WriteLine("Enter secret code:");
                        string secretCode = Console.ReadLine();
                        Console.WriteLine("Enter type:");
                        string type = Console.ReadLine();

                        dal.InsertNewPerson(firstName, lastName, secretCode, type);
                        break;

                    case "2":
                        Console.WriteLine("Enter first name to search:");
                        string searchFirstName = Console.ReadLine();
                        Console.WriteLine("Enter last name to search:");
                        string searchLastName = Console.ReadLine();

                        bool exists = dal.SearchPerson(searchFirstName, searchLastName);
                        if (exists)
                        {
                            dal.GetPersonByName(searchFirstName, searchLastName);
                        }
                        else
                        {
                            Console.WriteLine("Person not found.");
                        }
                        break;

                    case "3":
                        Console.WriteLine("Enter secret code to search:");
                        string searchSecretCode = Console.ReadLine();
                        dal.GetPersonBySecretCode(searchSecretCode);
                        break;

                    case "4":
                        Console.WriteLine("Sorry, I didn't have time to write this...");
                        break;

                    case "5":
                        Console.WriteLine("Sorry, I didn't have time to write this...");
                        break;

                    case "6":
                        Console.WriteLine("Sorry, I didn't have time to write this...");
                        break;

                    case "7":
                        Console.WriteLine("Sorry, I didn't have time to write this...");
                        break;

                    case "8":
                        flag = false;
                        Console.WriteLine("GoodByeeeeeeeeeeeee........");
                        break;

                }

            }
        }
    }
}