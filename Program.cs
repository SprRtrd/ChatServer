// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Data.SQLite;
using System.Net.WebSockets;


namespace ChatServer
{
    class Program
    {
        static DatabaseHandler dbHandler = new();
        static void Main(string[] args)
        {
            
            dbHandler.CreateDatabase();
            
            string[] osoite = ["http://127.0.0.1:8080/"];
            HttpListener listener = new();

            foreach(string numero in osoite){
                listener.Prefixes.Add(numero);
            }

            listener.Start();
            Console.WriteLine("Listening...");
            
            YhteydenTestaus(listener);
        
            Console.ReadLine();           

        }

        static async void YhteydenTestaus(HttpListener listener){
            
            HttpListenerContext context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest){
                WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                WebSocket webSocket = webSocketContext.WebSocket;
                Console.WriteLine("Client connected");
                
                // Lähettää viestin takaisin clientille
                // await Echo(webSocket);

                // 
                // await EchoUusi(webSocket);
                await ViestinVastaanotto(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }

            
        }

        private static async Task Echo(WebSocket webSocket){

            byte[] buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while(!receiveResult.CloseStatus.HasValue){
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count), 
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None
                );
                

            }

            
            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None
            );
        }

        private static async Task EchoUusi(WebSocket webSocket){

            byte[] buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            string viesti = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            dbHandler.LisaaViesti(viesti);
            int id = dbHandler.ViimeisinId();

            buffer = Encoding.UTF8.GetBytes(id.ToString());
            
            while(!receiveResult.CloseStatus.HasValue){
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer), 
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None
                );
                

            }
            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None
            );
        }

        private static async Task ViestinVastaanotto(WebSocket webSocket){
        
            byte[] buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            string viesti = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);

    }

    }

    

}