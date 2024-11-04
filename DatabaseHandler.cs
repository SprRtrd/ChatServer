using System;
using System.IO;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace ChatServer;

public class DatabaseHandler
{

    private string _connectionString = "Data Source=chat.db";

    public void CreateDatabase(){
        using (var connection = new SQLiteConnection(_connectionString)){
            
            connection.Open();
            string tableQuery1 = "CREATE TABLE IF NOT EXISTS viestit (lahettajaid INTEGER PRIMARY KEY, viestinumero INTEGER, viesti TEXT, timestamp DATETIME)";
            string tableQuery2 = "CREATE TABLE IF NOT EXISTS kayttajat (lahettajaid INTEGER PRIMARY KEY, kayttajatunnus TEXT, salasana TEXT)";
            LuoPoyta(tableQuery1, connection);
            LuoPoyta(tableQuery2, connection);

            

        }
    }
    
    public void LuoPoyta(string tableQuery, SQLiteConnection connection)
    {
        using (var command = new SQLiteCommand(tableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

    }
    

}
