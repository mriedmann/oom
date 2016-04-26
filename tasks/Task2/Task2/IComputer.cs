using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public interface IServer
    {
        string HostnameOrIpAddress { get; set; }
        string FullyQualifiedHostName { get; set; }
        IPAddress[] IPv4Addresses { get; set; }

        bool CheckIfOnline();
    }
}
