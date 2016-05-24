using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task2
{

    public class ServicesFileReader : StreamReader
    {
        private const string pattern = @"([\w-]+)[ \t]*([1-9][0-9]*)\/([\w]+)[ \t]*([\w ]*).*";

        public ServicesFileReader(string path) : base(path)
        {
        }

        public ServiceFileRecord? ReadNextRecord()
        {
            while (Peek() >= 0)
            {
                string line = ReadLine();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                var m = Regex.Match(line, pattern, RegexOptions.IgnoreCase);
                if (!m.Success || m.Groups.Count < 1)
                    continue;

                try
                {
                    var protocol = (ServiceProtocol)Enum.Parse(typeof(ServiceProtocol), m.Groups[3].Value, true);

                    return new ServiceFileRecord()
                    {
                        Name = m.Groups[1].Value + "-" + protocol.ToString().ToLower(),
                        PortNumber = int.Parse(m.Groups[2].Value),
                        Protocol = protocol,
                        DisplayName = (string.IsNullOrWhiteSpace(m.Groups[4].Value) ? m.Groups[1].Value : m.Groups[4].Value)
                    };
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return null;
        }
    }
}
