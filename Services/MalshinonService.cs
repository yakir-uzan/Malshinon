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
        //ייבוא פונקציות העזר והדאל לקובץ הנוכחי
        MalshinonDAL dal = new MalshinonDAL();
        MalshinonHelper help = new MalshinonHelper();
            

        //בדיקה אם אדם קיים במערכת, אם לא אז נוצר אדם חדש עם טייפ מלשין
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
                string secretCode = help.CreateSecretCode(firstName, lastName);
                dal.InsertNewPerson(firstName, lastName, secretCode, "reporter");
                Console.WriteLine("The person has been successfully added to the system!");
            }
        }


        //---------------------------------------------------------

        //זרימת הגשת דוח

        //ניהול זרימת מידע
        public void MngReportFlow()
        {
            Console.WriteLine("Enter your reporter ID:");
            int reporterId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter report text starting with uppercase initials of first and last name:");
            string reportText = Console.ReadLine();

            string fullName = help.SearchTarget(reportText);

            if (string.IsNullOrEmpty(fullName))
            {
                Console.WriteLine("Target name not found.");
                return;
            }

            string[] names = fullName.Split(' ');

            if (names.Length < 2)
            {
                Console.WriteLine("Target name format invalid.");
                return;
            }

            string firstName = names[0];
            string lastName = names[1];

            int targetId;
            bool targetExists = dal.SearchPerson(firstName, lastName);

            if (targetExists)
            {
                targetId = dal.GetPeopleId(firstName, lastName);
            }
            else
            {
                string secretCode = help.CreateSecretCode(firstName, lastName);
                dal.InsertNewPerson(firstName, lastName, secretCode, "target");
                Console.WriteLine("The person has been successfully added to the system!");
                targetId = dal.GetPeopleId(firstName, lastName); 
            }

            //מוסיף את הדיווח לטבלת המודיעין עם האיי-דיז של המדווח והמטרה
            dal.InsertIntelReport(reporterId, targetId, reportText);
            Console.WriteLine("Report submitted successfully.");

            //עדכון העמודות של המשימות והמטרות לפי איי-דיז
            dal.UpdateMentionCount(targetId);
            dal.UpdateReportCount(reporterId);

            //בודק לפי איי-די אם מספר הדוחות >= ל10 וממוצע המילים של הדוחות גדול מ100
            // מעדכן את הטייפ ל"פוטנצייאלי" ומדפיס הודעה
            if (dal.GetNumReport(reporterId) >= 10 && dal.GetAvgNumWords(reporterId) >= 100)
            {
                dal.UpdatePersonType(reporterId, "potential_agent");
                Console.WriteLine("Reporter promoted to potential_agent.");
            }

            //בודק אם יש 20 דיווחים ויותר על המולשן
            //מדפיס אזהרה
            if (dal.GetNumMention(targetId) >= 20)
            {
                Console.WriteLine("ALERT: Target is a potential threat!");
            }
        }
        
    }
}
