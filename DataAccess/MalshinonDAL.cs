using MySql.Data.MySqlClient;
using System;

internal class MalshinonDAL
{
    private string connStr = "server=localhost;user=root;password=;database=Malshinon;";
    private MySqlConnection _conn;

    public MalshinonDAL()
    {
        _conn = new MySqlConnection(connStr);
    }

    // מתודה שמפעילה מתודות שלא מחזירות כלום מהדאטה - בייס  
    private void ExecuteCommand(string query)
    {
        _conn.Open();
        MySqlCommand cmd = new MySqlCommand(query, _conn);
        cmd.ExecuteNonQuery();
        _conn.Close();
    }

    // מתודה כללית שמפעילה מתודות שמחזירות משו מהדאטה בייס
    private MySqlDataReader ExecuteReader(string query)
    {
        _conn.Open();
        MySqlCommand cmd = new MySqlCommand(query, _conn);
        return cmd.ExecuteReader();
    }

    // מתודה בוליאנית שבודקת אם האדם קיים בטבלה
    public bool PersonExists(string firstName, string lastName)
    {
        string query = "SELECT 1 FROM People WHERE first_name = '" + firstName + "' AND last_name = '" + lastName + "' LIMIT 1";
        MySqlDataReader reader = ExecuteReader(query);
        bool exists = reader.Read();
        reader.Close();
        _conn.Close();
        return exists;
    }

    // מתודה שמוסיפה אדם חדש לטבלה
    public void InsertPerson(string firstName, string lastName, string secretCode, string type)
    {
        string query = "INSERT INTO People (first_name, last_name, secret_code, type) " +
                       "VALUES ('" + firstName + "', '" + lastName + "', '" + secretCode + "', '" + type + "')";
        ExecuteCommand(query);
    }

    // מתודה שמדפיסה אדם לפי שם ומשפחה
    public void PrintPersonByName(string firstName, string lastName)
    {
        string query = "SELECT * FROM People WHERE first_name = '" + firstName + "' AND last_name = '" + lastName + "'";
        MySqlDataReader reader = ExecuteReader(query);
        while (reader.Read())
        {
            Console.WriteLine(PersonToString(reader));
        }
        reader.Close();
        _conn.Close();
    }

    // מדפיס את הבנאדם לפי סיקרט - קוד
    public void PrintPersonBySecretCode(string secretCode)
    {
        string query = "SELECT * FROM People WHERE secret_code = '" + secretCode + "'";
        MySqlDataReader reader = ExecuteReader(query);
        while (reader.Read())
        {
            Console.WriteLine(PersonToString(reader));
        }
        reader.Close();
        _conn.Close();
    }

    // מוסיף דוח לטבלה
    public void InsertIntelReport(int reporterId, int targetId, string text, DateTime timeStamp)
    {
        string timeString = timeStamp.ToString("yyyy-MM-dd HH:mm:ss");
        string query = "INSERT INTO Intelreports (reporter_id, target_id, text, timeStamp) " +
                       "VALUES ('" + reporterId + "', '" + targetId + "', '" + text + "', '" + timeString + "')";
        ExecuteCommand(query);
    }

    // מעדכן את כמות הדוחות לאדם
    public void UpdateReportCount(int id, int count)
    {
        string query = "UPDATE People SET num_report = " + count + " WHERE id = " + id;
        ExecuteCommand(query);
    }

    // מעדכן את כמות המשימות
    public void UpdateMentionCount(int id, int count)
    {
        string query = "UPDATE People SET num_mention = " + count + " WHERE id = " + id;
        ExecuteCommand(query);
    }

    // מדפיס את המלשינים עם כמות הדוחות שלהם
    public void GetReporterStats()
    {
        string query = "SELECT first_name, last_name, num_report FROM People WHERE type = 'reporter' OR type = 'both' ORDER BY num_report DESC";
        MySqlDataReader reader = ExecuteReader(query);
        while (reader.Read())
        {
            string line = reader["first_name"].ToString() + " " + reader["last_name"].ToString() + " - Reports: " + reader["num_report"].ToString();
            Console.WriteLine(line);
        }
        reader.Close();
        _conn.Close();
    }

    // מדפיס את כל המולשנים עם המידע שלהם
    public void GetTargetStats()
    {
        string query = "SELECT id, first_name, last_name, type, num_report, num_mention FROM People WHERE type = 'target'";
        MySqlDataReader reader = ExecuteReader(query);
        while (reader.Read())
        {
            Console.WriteLine(PersonToString(reader));
        }
        reader.Close();
        _conn.Close();
    }

    // ממיר את המידע של האדם למחרוזת אחת מובנית
    private string PersonToString(MySqlDataReader reader)
    {
        string result = "ID: " + reader["id"].ToString() +
                        ", First Name: " + reader["first_name"].ToString() +
                        ", Last Name: " + reader["last_name"].ToString() +
                        ", Secret Code: " + reader["secret_code"].ToString() +
                        ", Type: " + reader["type"].ToString() +
                        ", Reports: " + reader["num_report"].ToString() +
                        ", Mentions: " + reader["num_mention"].ToString();
        return result;
    }

    // פונקצייה שמייצרת התראה
    // פונקצייה שמדפיסה את ההתראות
}
