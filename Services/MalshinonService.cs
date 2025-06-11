using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


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
            string reportTxt = Console.ReadLine();
            string[] splitReport = reportTxt.Split();

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
        public void MngReportFlow()
        {
            Console.WriteLine("Enter your reporter ID:");
            int reporterId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter report text starting with uppercase initials of first and last name:");
            string reportText = Console.ReadLine();

            List<string> fullName = SearchTarget();
            string firstName = fullName[0];
            string lastName = fullName[1];

            int targetId;
            bool targetExists = dal.SearchPerson(firstName, lastName);

            if (targetExists)
            {
                targetId = dal.GetPeopleId(firstName, lastName);

            }
            else
            {
                string secretCode = CreateSecretCode(firstName, lastName);
                dal.InsertNewPerson(firstName, lastName, secretCode, "target");
                Console.WriteLine("The person has been successfully added to the system!");
                targetId = dal.GetPeopleId(firstName, lastName); 
            }

            //מוסיף את הדיווח לטבלת המודיעין עם האיי-דיז של המדווח והמטרה
            dal.InsertIntelReport(reporterId, targetId, reportText, DateTime.Now);
            Console.WriteLine("Report submitted successfully.");

            //עדכון המדדים של המשימות והמטרות לפי איי-דיז
            dal.UpdateMentionCount(targetId);
            dal.UpdateReportCount(reporterId);

            //בודק לפי איי-די אם מספר הדוחות >= ל10 וממוצע המיליםשל הדוחות גדול מ100
            if (dal.GetNumReport(reporterId) >= 10 && dal.GetAvgNumWords(reporterId) >= 100)
            {
                dal.UpdatePersonType(reporterId, "potential_agent");
                Console.WriteLine("Reporter promoted to potential_agent.");
            }

            if (dal.GetNumMention(targetId) >= 20)
            {
                Console.WriteLine("ALERT: Target is a potential threat!");
            }



        }
        
    }
}
