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
        static void Main(string[] args)
        {
            //TODO: Viestien tallennus tietokantaan. Käyttäjätunnusten luominen. Viestien esittäminen clientille
            DatabaseHandler dbHandler = new DatabaseHandler();
            dbHandler.CreateDatabase();
            


            string[] osoite = ["http://127.0.0.1:8080/"];
            HttpListener listener = new();

            foreach(string numero in osoite){
                listener.Prefixes.Add(numero);
            }

            listener.Start();
            Console.WriteLine("Listening...");
            
            YhteydenTestaus(listener);
          
            
            
            
            /*HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            Stream body = request.InputStream;
            Encoding encoding = request.ContentEncoding;
            StreamReader reader = new StreamReader(body, encoding);
            string s = reader.ReadToEnd();

            HttpListenerResponse response = context.Response;
            
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer,0,buffer.Length);
            output.Close();*/
           
            
            
            
            
            

            Console.ReadLine();           

        }

        static async void YhteydenTestaus(HttpListener listener){
            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest){
                WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                WebSocket webSocket = webSocketContext.WebSocket;

                Console.WriteLine("Client connected");

                byte[] receiveBuffer = new byte[1024];
                if (webSocket.State == WebSocketState.Open){
                    WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Text){
                        string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
                        Console.WriteLine($"Received ,essage: {receivedMessage}");

                        byte[] buffer = Encoding.UTF8.GetBytes("Hello from server");
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Close){
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                        Console.WriteLine("WebSocket closed");
                    }
                }
              }
              else{
                context.Response.StatusCode = 400;
                context.Response.Close();
              
        
                }
            }
        }
    }
}