using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Tls.Crypto;
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
        try
        {
            _conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in ExecuteCommand: " + ex.Message);
        }
        finally
        {
            _conn.Close();
        }
    }

    // מתודה כללית שמפעילה מתודות שמחזירות משו מהדאטה בייס
    private MySqlDataReader ExecuteReader(string query)
    {
        try
        {
            _conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            return cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in ExecuteReader: " + ex.Message);
            _conn.Close();
            throw;
        }
    }

    // מתודה בוליאנית שבודקת אם האדם קיים בטבלה
     public bool SearchPerson(string firstName, string lastName)
    {
        string query = "SELECT 1 FROM People WHERE first_name = '" + firstName + "' AND last_name = '" + lastName + "' LIMIT 1";
        MySqlDataReader reader = null;
        bool exists = false;

        try
        {
            reader = ExecuteReader(query);
            exists = reader.Read();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in SearchPerson: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }

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
        MySqlDataReader reader = null;

        try
        {
            reader = ExecuteReader(query);
            while (reader.Read())
            {
                Console.WriteLine(PersonToString(reader));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetPersonByName: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }
    }

    // מדפיס את הבנאדם לפי סיקרט - קוד
    public void GetPersonBySecretCode(string secretCode)
    {
        string query = "SELECT * FROM People WHERE secret_code = '" + secretCode + "'";
        MySqlDataReader reader = null;

        try
        {
            reader = ExecuteReader(query);
            while (reader.Read())
            {
                Console.WriteLine(PersonToString(reader));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetPersonBySecretCode: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }
    }

    // מדפיס את כל המולשנים עם המידע שלהם
    public void GetTargetStats(int Id)
    {
        string query = $"SELECT num_mention FROM People WHERE id = {Id}";
        MySqlDataReader reader = null;

        try
        {
            reader = ExecuteReader(query);
            while (reader.Read())
            {
                Console.WriteLine(PersonToString(reader));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetTargetStats: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }
    }

    // מתודה שמחזירה איי-די לפי שם ומשפחה
    public int GetPeopleId(string firstName, string lastName)
    {
        string query = $"SELECT id FROM People WHERE first_name = '{firstName}' AND last_name = '{lastName}' LIMIT 1";
        MySqlDataReader reader = null;
        int id = -1;

        try
        {
            reader = ExecuteReader(query);
            if (reader.Read())
            {
                id = reader.GetInt32("id");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetPeopleId: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }

        return id;
    }

    // מתודה שמחזירה את ממוצע המילים של הדוחות פר איי-די
    public double GetAvgNumWords(int id)
    {
        string query = $"SELECT AVG(LENGTH(ir.text) - LENGTH(REPLACE(ir.text, ' ', '')) + 1) AS avg_word_count FROM intelReports ir WHERE ir.reporter_id = {id};";
        MySqlDataReader reader = null;
        double avg = -1;

        try
        {
            reader = ExecuteReader(query);
            if (reader.Read() && !reader.IsDBNull(0))
            {
                avg = reader.GetDouble(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetAvgNumWords: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }

        return avg;
    }

    // מתודה שמחזירה את מספר המשימות פר איי-די
    public int GetNumMention(int Id)
    {
        string query = $"SELECT num_mention FROM People WHERE id = {Id}";
        MySqlDataReader reader = null;
        int count = -1;

        try
        {
            reader = ExecuteReader(query);
            if (reader.Read())
            {
                count = int.Parse(reader["num_mention"].ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetNumMention: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }

        return count;
    }

    // מדפיס את כמות הדוחות של המלשינים
    public int GetNumReport(int Id)
    {
        string query = $"SELECT num_report FROM People WHERE id = {Id}";
        MySqlDataReader reader = null;
        int numReport = -1;

        try
        {
            reader = ExecuteReader(query);
            if (reader.Read())
            {
                numReport = int.Parse(reader["num_report"].ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetNumReport: " + ex.Message);
            throw;
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
            if (_conn.State == System.Data.ConnectionState.Open)
                _conn.Close();
        }

        return numReport;
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
    public void InsertIntelReport(int reporterId, int targetId, string text)
    {
        string query = $"INSERT INTO intelReports (reporter_id, target_id, text) VALUES ({reporterId}, {targetId}, '{text}')";
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

