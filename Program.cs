// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // IP-osoite ja portti
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            // Alustetaan TcpListener kuuntelemaan tiettyä osoitetta ja porttia
            TcpListener server = new TcpListener(localAddress, port);

            Console.WriteLine("Käynnistetään palvelin localhostilla osoitteessa 127.0.0.1:8080...");

            // Käynnistä kuuntelu
            server.Start();
            Console.WriteLine("Palvelin käynnistetty ja kuuntelee yhteyksiä.");
        }
    }
}
