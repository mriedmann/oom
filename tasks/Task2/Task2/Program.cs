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
            List<Server> servers = new List<Server>()
            {
                new Server("8.8.8.8"),
                new Server("8.8.4.4")
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
