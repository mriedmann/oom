using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Sensors
{
    public class MockedSensor : ISensor
    {
        public int Port { get; set; }

        public MockedSensor()
        {
        }

        public SensorState DoCheckState(Server target)
        {
            Random rnd = new Random();

            var stateNr = rnd.Next(0, 3);

            SensorState state = (SensorState)stateNr;

            System.Threading.Thread.Sleep(rnd.Next(10, 100));
            return state;
        }
    }
}
