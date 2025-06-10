using MySql.Data.MySqlClient;
using System;
using System.Diagnostics.Metrics;

namespace Malshinon
{

    //משימות:
    //ליצור פונקצייה אחת של רידר


    internal class MalshinonDAL
    {
        private string connStr = "server=localhost;user=root;password=;database=Malshinon;";
        private MySqlConnection _conn;

        public MalshinonDAL()
        {
            _conn = new MySqlConnection(connStr);
        }


        //חיפוש בנאדם במאגר
        public bool SearchPerson(string firstName, string lastName)
        {
            _conn.Open();
            string query = $"SELECT * FROM People WHERE first_name = '{firstName}' AND last_name = '{lastName}'";

            MySqlCommand cmd = new MySqlCommand(query, _conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            bool exists = reader.Read();

            reader.Close();
            _conn.Close();

            return exists;
        }


       
        //עובד
        //ליצור בנאדם חדש
        public void InsertNewPerson(string FirstName, string LastName, string secretCode, string type)
        {
            string query = $"INSERT INTO People (first_name, last_name, secret_code, type) " +
                           $"VALUES ('{FirstName}', '{LastName}', '{secretCode}', '{type}')";
            UpdateData(query);
        }

        //עובד
        //לקבל בנאדם לפי שם
        public void GetPersonByName(string firstName, string lastName)
        {
            _conn.Open();
            string query = $"SELECT * FROM People WHERE first_name = '{firstName}' AND last_name = '{lastName}'";

            MySqlCommand cmd = new MySqlCommand(query, _conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["id"]}, First Name: {reader["first_name"]}, Last Name: {reader["last_name"]}, secret Code: {reader["secret_code"]}, Type: {reader["type"]}, Num Report: {reader["num_report"]}, Num Mention: {reader["num_mention"]}");
            }

            reader.Close();
            _conn.Close();
        }


        //עובד
        //לקבל בנאדם לפי שם סודי
        public void GetPersonBySecretCode(string secretCode)
        {
            _conn.Open();
            string query = $"SELECT * FROM People WHERE secret_code = '{secretCode}'";

            MySqlCommand cmd = new MySqlCommand(query, _conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            string name = reader.GetString("first_name");

            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["id"]}, First Name: {reader["first_name"]}, Last Name: {reader["last_name"]}, secret Code: {reader["secret_Code"]}, Type: {reader["type"]}, Num Report: {reader["num_report"]}, Num Mention: {reader["num_mention"]}");
            }

            reader.Close();
            _conn.Close();
        }

        //עובד
        //להכניס דוח לטבלה
        public void InsertIntelReport(int reporterId,int targetId, string text, DateTime timeStamp)
        {
           string query = $"INSERT INTO Intelreports (reporter_id, target_id, text, timeStamp) " +
                          $"VALUES ('{reporterId}', '{targetId}', '{text}', '{timeStamp}')";
            UpdateData(query);
        }

        //עובד
        //מעדכן מספר דוחות
        public void UpdateReportCount(int Id, int numReport)
        {
            string query = $"UPDATE People SET num_report = {numReport}  WHERE id = {Id}";
            UpdateData(query);
        }

        //עובד
        //מעדכן כמות משימות שהושלמו
        public void UpdateMentionCount(int Id, int numMention)
        {
          string query = $"UPDATE People SET num_mention = {numMention}  WHERE id = {Id}";
            UpdateData(query);

        }

        //מייצא סטטיסטיקות של דוחות
        public void GetReporterStats()
        {
            string query = $"SELECT first_name, last_name, num_report FROM People WHERE type = 'reporter' OR type = 'both' ORDER BY num_report DESC;";
            UpdateData(query);
        }

        //מייצא סטטיסטיקות של מטרות
        public void GetTargetStats(int id)
        {
            string query = "SELECT id, first_name, last_name, type, num_report, num_mention FROM People WHERE type = 'target'";
            UpdateData(query);
        }


        //public void CreateAlert()


        //public void GetAlerts()




        //פונקציה כללית שמעדכנת דאטה
        public void UpdateData(string query)
        {
            try
            {
                _conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, _conn);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _conn.Close();
            }
        }

        //פונקציה כללית שמחזירה דאטה
        public string GetData(string query)
        {
            _conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            MySqlDataReader reader = cmd.ExecuteReader();


            reader.Close();
            _conn.Close();
        }
    }
}
