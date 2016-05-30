using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Task2.Helpers;
using Task2.Sensors;

namespace Task2
{
    class Program
    {
        static int PreferedConsoleBufferWidth => 85;
        static int PreferedConsoleBufferHeight => Console.BufferHeight;
        static int PreferedConsoleWindowWidth => PreferedConsoleBufferWidth;
        static int PreferedConsoleWindowHeight => 40;

        static void Main(string[] args)
        {
#if DEBUG
            //ObjectCache.Instance.Clear();

            SensorFactory.RegisterSensorBuilder<IcmpSensor>(ServiceProtocol.ICMP, (sfr) => new MockedSensor());
            SensorFactory.RegisterSensorBuilder<TcpPortSensor>(ServiceProtocol.TCP, (sfr) => new MockedSensor());
            SensorFactory.RegisterSensorBuilder<UdpPortSensor>(ServiceProtocol.UDP, (sfr) => new MockedSensor());
#else
            SensorFactory.RegisterSensorBuilder<IcmpSensor>(ServiceProtocol.ICMP, (sfr) => new IcmpSensor());
            SensorFactory.RegisterSensorBuilder<TcpPortSensor>(ServiceProtocol.TCP, (sfr) => new TcpPortSensor(sfr.PortNumber));
            SensorFactory.RegisterSensorBuilder<UdpPortSensor>(ServiceProtocol.UDP, (sfr) => new UdpPortSensor(sfr.PortNumber));
#endif
            var serviceRecords = ObjectCache.Instance.GetOrSetObject("ServiceFileRecordsRegistryContent", () => LoadServiceRecordsFromFile());

            ServiceFileRecordRegistry.Load(serviceRecords);
            ServiceFileRecordRegistry.AddServiceFileRecord(new ServiceFileRecord()
            {
                Name = "echo-icmp",
                DisplayName = "ping",
                PortNumber = 0,
                Protocol = ServiceProtocol.ICMP
            });

            var serverRecords = ObjectCache.Instance.GetOrSetObject("ServerList", () => new[] {
                new ServerRecord() {
                    HostnameOrIpAddress = "127.0.0.1",
                    ServiceNames = new[] { "echo-icmp" , "microsoft-ds-tcp"} },
                new ServerRecord() {
                    HostnameOrIpAddress = "ic15.at",
                    ServiceNames = new[]  { "echo-icmp", "http-tcp", "https-tcp" } },
                new ServerRecord() {
                    HostnameOrIpAddress = "8.8.8.8",
                    ServiceNames = new[]  { "echo-icmp", "domain-tcp" } },
                new ServerRecord() {
                    HostnameOrIpAddress = "8.8.4.4",
                    ServiceNames = new[]  { "echo-icmp", "domain-tcp" } }
            });

            var servers = new List<Server>();
            foreach (var serverRecord in serverRecords)
                servers.Add(new Server(serverRecord.HostnameOrIpAddress, serverRecord.ServiceNames));

            Console.CursorVisible = false;

            int bufferWidth = PreferedConsoleBufferWidth > Console.BufferWidth ? PreferedConsoleBufferWidth : Console.BufferWidth;
            int bufferHeight = PreferedConsoleBufferHeight > Console.BufferHeight ? PreferedConsoleBufferHeight : Console.BufferHeight;

            Console.SetBufferSize(bufferWidth, bufferHeight);
            try
            {
                Console.SetWindowSize(PreferedConsoleWindowWidth, PreferedConsoleWindowHeight );
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                Console.WriteLine("== Continue With Enter ==");
                Console.ReadLine();
            }

            Console.Clear();
            ConsoleBuffer buffer = new ConsoleBuffer(Console.BufferWidth, Console.BufferHeight, Console.BufferWidth, Console.BufferHeight);

            object bufferLock = new object();

            buffer.Draw("= == === ==== ===== =========================================== ====== ==== === == =", 0, 0, 15);
            buffer.Draw("= == === ==== ===== =      Fancy Console Host Monitor         = ====== ==== === == =", 0, 1, 15);
            buffer.Draw("= == === ==== ===== =========================================== ====== ==== === == =", 0, 2, 15);

            int offsetIndex = 4;
            int[] offsets = servers.Select(s =>
            {
                var pos = offsetIndex;
                offsetIndex += s.Services.Count() + 2;

                buffer.Draw(string.Format("#### {0}", s.FullyQualifiedHostName), 0, pos, 15);

                return pos;
            }).ToArray();

            foreach (var server in servers)
            {
                Task.Factory.StartNew((object s) =>
                {
                    while (true)
                    {
                        lock (s)
                        {
                            ((Server)s).UpdateServices();
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                }, server);
            }

            while (true)
            {
                Parallel.For(0, servers.Count, i =>
                {
                    lock (servers[i])
                        lock (bufferLock)
                        {
                            var j = 0;
                            var server = servers[i];
                            var offset = offsets[i];
                            Func<int, int> rowIndex = ((p) => { var r = offset + j; j += p; return r; });            

                            rowIndex(1);

                            foreach (var service in server.Services)
                            {
                                var nameText = string.Format("{0}:", service.DisplayName);
                                var statusText = string.Format("{0,20}", service.LastState);
                                var timingText = string.Format("{0,8:###.00}ms  {1,8:###.00}ms +- {2,6:###.00}ms",
                                    service.LastCheckDuration.TotalMilliseconds,
                                    service.LastCheckDurationMean,
                                    service.LastCheckDurationStdDev);

                                short statusFlags = 0;
                                switch (service.LastState)
                                {
                                    case ServiceState.Unknown:
                                        statusFlags = 13;
                                        break;
                                    case ServiceState.Reachable:
                                        statusFlags = 10;
                                        break;
                                    case ServiceState.Unreachable:
                                        statusFlags = 12;
                                        break;
                                    default:
                                        statusFlags = 15;
                                        break;
                                }

                                buffer.clearRow(rowIndex(0));
                                buffer.Draw(nameText, 0, rowIndex(0), 15);
                                buffer.Draw(statusText, 20, rowIndex(0), statusFlags);
                                buffer.Draw(timingText, 50, rowIndex(0), 15);

                                rowIndex(1);
                            }
                        }
                });

                buffer.Print();
                System.Threading.Thread.Sleep(1000);
            }
        }

        static List<ServiceFileRecord> LoadServiceRecordsFromFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "services.txt");
            List<ServiceFileRecord> records = new List<ServiceFileRecord>();
            using (var reader = new ServicesFileReader(path))
            {
                ServiceFileRecord? record;
                while ((record = reader.ReadNextRecord()) != null)
                {
                    records.Add(record.Value);
                }
            }
            return records;
        }
    }
}
