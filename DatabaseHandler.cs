using System;
using System.IO;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using System.Data;
using System.Net.Sockets;

namespace ChatServer;

public class DatabaseHandler
{

    private string _connectionString = "Data Source=chat.db";

    public void CreateDatabase(){
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        string tableQuery1 = "CREATE TABLE IF NOT EXISTS viestit (viesti_id INTEGER PRIMARY KEY, lahettaja TEXT, viesti TEXT, timestamp DATETIME)";
        LuoPoyta(tableQuery1, connection);
    }
    
    public static void LuoPoyta(string tableQuery, SQLiteConnection connection)
    {
        using var command = new SQLiteCommand(tableQuery, connection);
        command.ExecuteNonQuery();

    }
    
    public void LisaaViesti(string viesti){

        Viesti? avattuViesti = JsonSerializer.Deserialize<Viesti>(viesti);
        string sqlFormattedDate = avattuViesti.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
        string lisaysQuery = $"INSERT INTO viestit (lahettaja, viesti, timestamp) VALUES (\'{avattuViesti.Nimi}\', \'{avattuViesti.Teksti}\', \'{sqlFormattedDate}\')";

        try
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = new SQLiteCommand(lisaysQuery, connection);
            var rowInserted = command.ExecuteNonQuery();
        }
        catch(SQLiteException ex)
        {
            Console.WriteLine(ex.Message);
        }


    }

    public int ViimeisinId(){
    int id = 0;
    string hakuQuery = "SELECT viesti_id FROM viestit ORDER BY viesti_id DESC LIMIT 1";

    try{
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(hakuQuery, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
        }
    catch(SQLiteException ex)
    {
        System.Console.WriteLine(ex.Message);
    }


        return id;
    }

    public List<Dictionary<string, string>> HaeViestit(int id)
    {

        string hakuQuery = $"SELECT * FROM viestit WHERE viesti_id > {id}";
        List<Dictionary<string, string>> viestit = new();

        try{
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(hakuQuery, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string server_id = reader.GetInt32(0).ToString();
                    string lahettaja = reader.GetString(1);
                    string teksti = reader.GetString(2);
                    string timeStamp = reader.GetString(3);
                    Dictionary<string, string> viesti = new(){{"Tyyppi", "Viesti"}, 
                    {"server_viesti_id", server_id}, 
                    {"lahettaja", lahettaja}, 
                    {"viesti", teksti}, 
                    {"timestamp", timeStamp}};
                    viestit.Add(viesti);
                }
            }
        }
    catch(SQLiteException ex)
    {
        System.Console.WriteLine(ex.Message);
    }
        return viestit;
    }

}
