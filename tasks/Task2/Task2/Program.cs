using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

        static Dictionary<string, Server> servers = new Dictionary<string, Server>();
        static ConsoleBuffer buffer;
        static Dictionary<string, int> offsets = new Dictionary<string, int>();

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

            foreach (var serverRecord in serverRecords)
                servers.Add(serverRecord.HostnameOrIpAddress, new Server(serverRecord.HostnameOrIpAddress, serverRecord.ServiceNames));

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
            buffer = new ConsoleBuffer(Console.BufferWidth, Console.BufferHeight, Console.BufferWidth, Console.BufferHeight);

            object bufferLock = new object();

            buffer.Draw("= == === ==== ===== =========================================== ====== ==== === == =", 0, 0, 15);
            buffer.Draw("= == === ==== ===== =      Fancy Console Host Monitor         = ====== ==== === == =", 0, 1, 15);
            buffer.Draw("= == === ==== ===== =========================================== ====== ==== === == =", 0, 2, 15);

            int offsetIndex = 4;
            foreach(var s in servers.Values)
            {
                var pos = offsetIndex;
                offsetIndex += s.Services.Count() + 2;

                buffer.Draw(string.Format("#### {0}", s.FullyQualifiedHostName), 0, pos, 15);

                offsets.Add(s.HostnameOrIpAddress, pos);
            }
            buffer.Print();

            using (var serverObservable = new Subject<string>())
            {
                foreach (var server in servers.Values) {
                    Task.Factory.StartNew((obj) =>
                    {
                        while (true)
                        {
                            string s = obj as string;
                            lock (servers[s])
                            {
                                servers[s].UpdateServices();
                            }
                            serverObservable.OnNext(s);
                            System.Threading.Thread.Sleep(1000);
                        }
                    }, server.HostnameOrIpAddress);
                }

                serverObservable
                        .Subscribe(s => {
                            lock (servers[s])
                            {
                                RedrawServer(servers[s]);
                            }
                        });

                while (!Console.KeyAvailable)
                {
                    buffer.Print();
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        static void RedrawServer(Server server)
        {
            var j = 0;
            var offset = offsets[server.HostnameOrIpAddress];
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

                lock (buffer)
                {
                    buffer.clearRow(rowIndex(0));
                    buffer.Draw(nameText, 0, rowIndex(0), 15);
                    buffer.Draw(statusText, 20, rowIndex(0), statusFlags);
                    buffer.Draw(timingText, 50, rowIndex(0), 15);
                }
                rowIndex(1);
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
