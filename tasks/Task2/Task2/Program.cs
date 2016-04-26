using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<GenericServer> servers = new List<GenericServer>()
            {
                new DnsServer("8.8.8.8"),
                new DnsServer("8.8.4.4"),
                new WebServer("www.technikum-wien.at"),
                new WebServer("www.ic15.at")
            };

            foreach (var server in servers)
            {
                string ipAdresses = string.Join<IPAddress>(",", server.IPv4Addresses);
                Console.WriteLine("====\nFQHN: {0}\n IPs: {1}", server.FullyQualifiedHostName, ipAdresses);
            }

            Console.Write("\n\n\n");

            foreach (var server in servers)
            {
                Console.Write(server.HostnameOrIpAddress);
                if (server.CheckIfOnline())
                    Console.WriteLine(" is Online");
                else
                    Console.WriteLine(" is Offline");
            }
        }
    }
}
