using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class Server
    {
        public string HostnameOrIpAddress { get; private set; }
        public string FullyQualifiedHostName => fqhn ?? (fqhn = fetchFQHN());
        public IPAddress[] IPv4Addresses => ipv4Addresses ?? (ipv4Addresses = fetchIPv4Adresses());

        public Service[] Services { get { return services.ToArray(); } }

        private List<Service> services = new List<Service>();

        private string fqhn = null;
        private IPAddress[] ipv4Addresses = null;

        public Server(string hostnameOrIpAdress, string[] serviceNames)
        {
            HostnameOrIpAddress = hostnameOrIpAdress;

            foreach (var serviceName in serviceNames)
                services.Add(new Service(serviceName, this));
        }

        public Server(string hostnameOrIpAdress, IEnumerable<Service> services)
        {
            HostnameOrIpAddress = hostnameOrIpAdress;
            services = new List<Service>(services);
        }

        public void UpdateServices()
        {
            foreach(var service in services)
                service.CheckState();
        }

        private IPAddress[] fetchIPv4Adresses()
        {
            try
            {
                var result = Dns.BeginGetHostAddresses(HostnameOrIpAddress, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                if (!success)
                    throw new Exception("Can't get DNS Entry");
                return Dns.EndGetHostEntry(result).AddressList; ;
            }
            catch (Exception)
            {
                return new IPAddress[] { };
            }
        }

        private string fetchFQHN()
        {
            try
            {
                var result = Dns.BeginGetHostEntry(HostnameOrIpAddress, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                if (!success)
                    throw new Exception("Can't get DNS Entry");
                return Dns.EndGetHostEntry(result).HostName; ;
            }
            catch (Exception)
            {
                return HostnameOrIpAddress;
            }
        }
    }
}
