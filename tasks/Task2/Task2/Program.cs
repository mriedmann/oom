using System;
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
        static void Main(string[] args)
        {
            SensorFactory.RegisterSensorBuilder<IcmpSensor>(ServiceProtocol.ICMP, (sfr) => new IcmpSensor());
            SensorFactory.RegisterSensorBuilder<TcpPortSensor>(ServiceProtocol.TCP, (sfr) => new TcpPortSensor(sfr.PortNumber));
            SensorFactory.RegisterSensorBuilder<UdpPortSensor>(ServiceProtocol.UDP, (sfr) => new UdpPortSensor(sfr.PortNumber));

            var serviceRecords = ObjectCache.Instance.GetOrSetObject("ServiceFileRecordsRegistryContent", () => LoadServiceRecordsFromFile());

            ServiceFileRecordRegistry.Load(serviceRecords);
            ServiceFileRecordRegistry.AddServiceFileRecord(new ServiceFileRecord() {
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
            Console.SetBufferSize(120, 40);
            Console.SetWindowSize(Console.BufferWidth, Console.BufferHeight);

            Console.Clear();
            ConsoleBuffer buffer = new ConsoleBuffer(Console.BufferWidth, Console.BufferHeight, Console.BufferWidth, Console.BufferHeight);

            object bufferLock = new object();
            while (true)
            {
                int rowIndex = 0;

                Parallel.ForEach(servers, server => {
                    Parallel.ForEach(server.Services, (s) => s.CheckStateAsync().Wait(60000));

                    lock (bufferLock)
                    {


                        buffer.clearRow(rowIndex);
                        buffer.Draw(string.Format("#### {0}", server.FullyQualifiedHostName), 0, rowIndex++, 15);
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

                            buffer.clearRow(rowIndex);
                            buffer.Draw(nameText, 0, rowIndex, 15);
                            buffer.Draw(statusText, 20, rowIndex, statusFlags);
                            buffer.Draw(timingText, 50, rowIndex, 15);

                            rowIndex++;
                        }
                        rowIndex += 2;
                        buffer.Print();
                    }
                });

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
