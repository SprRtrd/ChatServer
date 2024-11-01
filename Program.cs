// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
            Console.WriteLine("{0}", context);
            Console.ReadLine();

        }
    }
}
