// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] osoite = ["http://127.0.0.1:8080/"];
            HttpListener listener = new HttpListener();

            foreach(string numero in osoite){
                listener.Prefixes.Add(numero);
            }

            listener.Start();
            Console.WriteLine("Listening...");
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            HttpListenerResponse response = context.Response;
            
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer,0,buffer.Length);
            output.Close();
            Stream body = request.InputStream;
            Encoding encoding = request.ContentEncoding;
            StreamReader reader = new StreamReader(body, encoding);
            

            Console.WriteLine("{0}", body.Length);
            Console.ReadLine();           

        }
    }
}
