using System;
using System.IO;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace ChatServer;

public class DatabaseHandler
{

    private string _connectionString = "Data Source=viestit.db";

    public void CreateDatabase(){
        using (var connection = new SQLiteConnection(_connectionString)){
            
            connection.Open();
            string tableQuery = "CREATE TABLE IF NOT EXISTS viestit (id INTEGER PRIMARY KEY, name TEXT, age INTEGER)";
            using (var command = new SQLiteCommand(tableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

        }
    }
    
      
    

}
