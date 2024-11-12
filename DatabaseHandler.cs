using System;
using System.IO;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;

namespace ChatServer;

public class DatabaseHandler
{

    private string _connectionString = "Data Source=chat.db";

    public void CreateDatabase(){
        using (var connection = new SQLiteConnection(_connectionString)){
            
            connection.Open();
            string tableQuery1 = "CREATE TABLE IF NOT EXISTS viestit (viesti_id INTEGER PRIMARY KEY, lahettaja TEXT, viesti TEXT, timestamp DATETIME)";
            LuoPoyta(tableQuery1, connection);
        }
    }
    
    public static void LuoPoyta(string tableQuery, SQLiteConnection connection)
    {
        using (var command = new SQLiteCommand(tableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

    }
    
    public void LisaaViesti(string viesti){
        Viesti? avattuViesti = JsonSerializer.Deserialize<Viesti>(viesti);
        string lisaysQuery = $"INSERT INTO viestit (lahettaja, viesti, timestamp) VALUES ({avattuViesti.Nimi}, {avattuViesti.Teksti}, {avattuViesti.TimeStamp})";
        
        try
        {

        
        using (var connection = new SQLiteConnection(_connectionString)){
            connection.Open();
            using var command = new SQLiteCommand(lisaysQuery, connection);
            var rowInserted = command.ExecuteNonQuery();
        }
        }
        catch(SQLiteException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
