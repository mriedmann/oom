using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public static class SensorFactory
    {
        private static Dictionary<ServiceProtocol, Func<ServiceFileRecord,ISensor>> store = new Dictionary<ServiceProtocol, Func<ServiceFileRecord, ISensor>>();

        public static void RegisterSensorBuilder<T>(ServiceProtocol protocol, Func<ServiceFileRecord, ISensor> builder) where T : ISensor
        {
            store.Add(protocol, builder);
        }

        public static ISensor BuildSensor(ServiceFileRecord serviceFileRecord)
        {
            if (!store.ContainsKey(serviceFileRecord.Protocol))
                throw new Exception("Can't find builder for " + serviceFileRecord.Protocol);

            ISensor sensor = store[serviceFileRecord.Protocol](serviceFileRecord);
            return sensor;
        }
    }
}
