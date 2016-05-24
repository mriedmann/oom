using System.Net;

namespace Task2
{
    public interface IServer
    {
        string FullyQualifiedHostName { get; }
        string HostnameOrIpAddress { get; }
        IPAddress[] IPv4Addresses { get; }
        Service[] Services { get; }

        void UpdateServices();
    }
}