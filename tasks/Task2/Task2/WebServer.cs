using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class WebServer : GenericServer
    {
        public WebServer(string hostnameOrIpAdress) 
            : base(hostnameOrIpAdress)
        {
        }
    }
}
