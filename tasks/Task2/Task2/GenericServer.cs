using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class GenericServer : IServer
    {
        public string HostnameOrIpAddress { get; set; }
        public string FullyQualifiedHostName { get; set; }
        public IPAddress[] IPv4Addresses { get; set; }

        private DateTime lastSeen = DateTime.MinValue;

        public GenericServer(string hostnameOrIpAdress)
        {
            HostnameOrIpAddress = hostnameOrIpAdress;
            updateFQHN();
            updateIPv4Adresses();
        }

        private void updateIPv4Adresses()
        {
            try
            {
                IPv4Addresses = Dns.GetHostAddresses(HostnameOrIpAddress);

            }
            catch (Exception)
            {
                IPv4Addresses = new IPAddress[] { };
            }
        }

        public bool CheckIfOnline()
        {
            if (FullyQualifiedHostName == null)
                return false;

            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            var reply = p.Send(FullyQualifiedHostName);

            return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
        }

        private void updateFQHN()
        {
            try
            {
                var hostentry = Dns.GetHostEntry(HostnameOrIpAddress);
                FullyQualifiedHostName = hostentry.HostName;
            }
            catch (Exception)
            {
                FullyQualifiedHostName = null;
            }
        }
    }
}
