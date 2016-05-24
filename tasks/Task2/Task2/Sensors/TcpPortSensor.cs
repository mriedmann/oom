using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Sensors
{
    public class TcpPortSensor : ISensor
    {
        public int Port { get; private set; }

        public TcpPortSensor(int portNumber)
        {
            Port = portNumber;
        }

        public SensorState DoCheckState(Server target)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    
                    var result = client.BeginConnect(target.FullyQualifiedHostName, Port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));

                    if (!success)
                        return SensorState.Error;

                    client.EndConnect(result);
                }
                return SensorState.OK;
            }
            catch (SocketException)
            {
                //TODO: Check for Status
                return SensorState.Error;
            }
            catch (Exception)
            {
                return SensorState.Error;
            }
        }
    }
}
