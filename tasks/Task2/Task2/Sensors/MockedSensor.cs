using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Sensors
{
    public class MockedSensor : ISensor
    {
        private Func<IServer, SensorState> mockingFunc;

        public int Port { get; set; }

        public MockedSensor(int port, Func<IServer, SensorState> mockingCheckFunc)
        {
            Port = port;
            this.mockingFunc = mockingCheckFunc;
        }

        public SensorState DoCheckState(IServer target)
        {
            return mockingFunc(target);
        }
    }
}
