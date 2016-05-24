using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Sensors
{
    public class MockedSensor : ISensor
    {
        private Func<Server, SensorState> mockingFunc;

        public int Port { get; set; }

        public MockedSensor(int port, Func<Server, SensorState> mockingCheckFunc)
        {
            Port = port;
            this.mockingFunc = mockingCheckFunc;
        }

        public SensorState DoCheckState(Server target)
        {
            return mockingFunc(target);
        }
    }
}
