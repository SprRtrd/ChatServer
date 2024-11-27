namespace ChatServer;

public class ViestiController
{

    public static void ViestinKasittely(Dictionary<string, string> avattuViesti, DatabaseHandler dbHandler)
       {
        if (avattuViesti != null && avattuViesti.TryGetValue("Tyyppi", out var tyyppi))
            {
                switch (tyyppi)
                {
                    case "Viesti":
                    System.Console.WriteLine("Se oli viesti");
                    break;

                    case "Vertaus":
                    System.Console.WriteLine("Se oli vertaus");
                    Vertaa(avattuViesti, dbHandler);
                    break;
                    
                    default:
                    break;
                }
            }
            else
            {
                System.Console.WriteLine("Ei ollut mikään");
            }
       }



    public static void Vertaa(Dictionary<string, string> avattuViesti, DatabaseHandler dbHandler)
       {
            int serverId = dbHandler.ViimeisinId();
            avattuViesti.TryGetValue("Id", out var id);
            Int32.TryParse(id, out int viestiId);
            if (serverId == viestiId)
            {
                return;
            }

            List<Dictionary<string, string>> viestit = new();
            viestit = dbHandler.HaeViestit(viestiId);
       }
}