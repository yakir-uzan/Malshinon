using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

internal class MalshinonDAL
{
    private string connStr = "server=localhost;user=root;password=;database=Malshinon;";
    private MySqlConnection _conn;

    public MalshinonDAL()
    {
        _conn = new MySqlConnection(connStr);
    }

    //============================================================================//
    
    //          == מתודות שונות ==//

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
     public bool SearchPerson(string firstName, string lastName)
    {
        string query = "SELECT 1 FROM People WHERE first_name = '" + firstName + "' AND last_name = '" + lastName + "' LIMIT 1";
        MySqlDataReader reader = ExecuteReader(query);
        bool exists = reader.Read();
        reader.Close();
        _conn.Close();
        return exists;
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

    //============================================================================//

    //          -== מתודות GET ==-

    // מתודה שמדפיסה אדם לפי שם ומשפחה
    public void GetPersonByName(string firstName, string lastName)
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
    public void GetPersonBySecretCode(string secretCode)
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

    // מדפיס את המלשינים עם כמות הדוחות שלהם
    public int GetNumReport(int Id)
    {
        string query = $"SELECT num_report FROM People WHERE id = 'Id'";
        MySqlDataReader reader = ExecuteReader(query);
        int numReport = -1;

        if (reader.Read())
        {
            numReport = int.Parse(reader["num_report"].ToString());
        }

        reader.Close();
        _conn.Close();
        return numReport;
    }

    // מדפיס את כל המולשנים עם המידע שלהם
    public void GetTargetStats(int Id)
    {
        string query = $"SELECT num_mention FROM People WHERE id = 'Id'";
        MySqlDataReader reader = ExecuteReader(query);

        while (reader.Read())
        {
            Console.WriteLine(PersonToString(reader));
        }
        reader.Close();
        _conn.Close();
    }

    //מתודה שמחזירה איי-די לפי שם ומשפחה
    public int GetPeopleId(string firstName, string lastName)
    {
        string query = $"SELECT id FROM People WHERE first_name = '{firstName}' AND last_name = '{lastName}' LIMIT 1";
        MySqlDataReader reader = ExecuteReader(query);

        int id = -1;

        if (reader.Read())
        {
            id = reader.GetInt32("id");
        }

        reader.Close();
        _conn.Close();

        return id;
    }

    //מתודה שמחזירה את ממוצע המילים של הדוחות פר בנאדם
    public int GetAvgNumWords(int id)
    {
        string query = $"SELECT p.id, AVG(CHAR_LENGTH(ir.text)) AS avg_length, p.type FROM people p JOIN intelReports ir ON ir.reporter_id = p.id"; GROUP BY p.id\r\n HAVING avg_length > 100   \r\n AND p.type != 'agent' \r\n ORDER BY avg_length; ";
        MySqlDataReader reader = ExecuteReader(query);
        int count = -1;

        if (reader.Read())
        {
            count = int.Parse(query);
        }
        reader.Close();
        _conn.Close();

        return count;
    }

    public int GetNumMention(int Id)
    {

    }

    //===================================================================================================//

    //          -== מתודות INSERT ==-

    // מתודה שמוסיפה אדם חדש לטבלה
    public void InsertNewPerson(string firstName, string lastName, string secretCode, string type)
    {
        string query = "INSERT INTO People (first_name, last_name, secret_code, type) " +
                       "VALUES ('" + firstName + "', '" + lastName + "', '" + secretCode + "', '" + type + "')";
        ExecuteCommand(query);
    }

    // מוסיף דוח לטבלה
    public void InsertIntelReport(int reporterId, int targetId, string text, DateTime timeStamp)
    {
        string timeString = timeStamp.ToString("yyyy-MM-dd HH:mm:ss");
        string query = "INSERT INTO Intelreports (reporter_id, target_id, text, timeStamp) " +
                       "VALUES ('" + reporterId + "', '" + targetId + "', '" + text + "', '" + timeString + "')";
        ExecuteCommand(query);
    }

    //===================================================================================================

    //          -== מתודות UPDATE ==-

    // מעדכן את כמות הדוחות לאדם
    public void UpdateReportCount(int id)
    {
        string query = "UPDATE People SET num_report = num_report + 1 WHERE id = " + id;
        ExecuteCommand(query);
    }

    // מעדכן את כמות המשימות
    public void UpdateMentionCount(int id)
    {
        string query = "UPDATE People SET num_mention = num_mention + 1 WHERE id = " + id;
        ExecuteCommand(query);
    }

    // מעדכן את הטייפ של הבנאדם
    public void UpdatePersonType(int Id, string newType)
    {
        string query = $"UPDATE People SET type = '{newType}' WHERE id = {Id}";
        ExecuteCommand(query);
    }

}

    // פונקצייה שמייצרת התראה
    // פונקצייה שמדפיסה את ההתראות

