using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Sensors
{
    public class IcmpSensor : ISensor
    {

        public SensorState DoCheckState(Server target)
        {
            try
            {
                Ping Sender = new Ping();
                PingReply Result = Sender.Send(target.FullyQualifiedHostName);
                if (Result.Status == IPStatus.Success)
                    return SensorState.OK;
                else
                    return SensorState.Error;
            }
            catch (Exception)
            {
                return SensorState.Error;
            }
        }
    }
}
