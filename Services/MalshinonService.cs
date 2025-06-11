using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Malshinon
{
    internal class MalshinonService
    {
        MalshinonDAL dal = new MalshinonDAL();

        // יצירת סיקרט קוד
        public string CreateSecretCode(string firstName, string lastName)
        {
            string secretCode = $"{firstName[0]}{lastName[0]}";
            return secretCode;
        }


        //בדיקה אם אדם קיים במערכת, אם לא - נוצר אדם חדש עם טייפ מלשין
        public void MngPersonFlow()
        {
            Console.WriteLine("Enter your first Name: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter your last Name: ");
            string lastName = Console.ReadLine();

            bool exists = dal.SearchPerson(firstName, lastName);
            if (exists)
            {
                Console.WriteLine("The person exists in the system.");
            }
            else
            {
                string secretCode = CreateSecretCode(firstName, lastName);
                dal.InsertNewPerson(firstName, lastName, secretCode, "reporter");
                Console.WriteLine("The person has been successfully added to the system!");
            }
        }

        //---------------------------------------------------------
         
        //זרימת הגשת דוח

        //חיפוש מטרה בדוח
        public List<string> SearchTarget()
        {
            Console.WriteLine("Enter free text of the report: ");
            string report = Console.ReadLine();
            string[] splitReport = report.Split();

            List<string> fullName = new List<string>();

            foreach (string word in splitReport)
            {
                if (char.IsUpper(word[0]))
                {
                    fullName.Add(word);
                }

                else
                {
                    Console.WriteLine("Please enter the name of the reporter...");
                }
            }
            return fullName;
        }

        //ניהול זרימת מידע
        public string MngReportFlow()
        {
            List<string> fullName = SearchTarget();
            string firstName = fullName[0];
            string lastName = fullName[1];

            bool exists = dal.SearchPerson(firstName, lastName);
            if (exists)
            {
                string id = $"SELECT id FROM People WHERE firstName = {first_name} AND lastName = {last_name}";
                return id;
            }
            else
            {
                string secretCode = CreateSecretCode(firstName, lastName);
                dal.InsertNewPerson(firstName, lastName, secretCode, "target");
                Console.WriteLine("The person has been successfully added to the system!");
            }
        }
    }
}
