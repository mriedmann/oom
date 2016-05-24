using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Sensors
{
    public class UdpPortSensor : ISensor
    {
        public int Port { get; private set; }

        public UdpPortSensor(int portNumber)
        {
            Port = portNumber;
        }

        public SensorState DoCheckState(IServer target)
        {
            try
            {
                var udp_ep = new IPEndPoint(IPAddress.Any, 2280);
                UdpClient client = new UdpClient(udp_ep);
                client.Client.ReceiveTimeout = 3000;

                client.Send(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF }, 4, target.FullyQualifiedHostName, Port);
                client.Receive(ref udp_ep);

                return SensorState.OK;
            }
            catch (SocketException)
            {
                return SensorState.Error;
            }
            catch (TimeoutException)
            {
                return SensorState.OK;
            }
            catch (Exception)
            {
                return SensorState.Error;
            }
        }
    }
}
