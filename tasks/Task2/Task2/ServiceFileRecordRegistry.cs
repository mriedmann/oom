using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class ServiceFileRecordRegistry
    {
        private Dictionary<string, ServiceFileRecord> serviceFileRecords = new Dictionary<string, ServiceFileRecord>();

        private static ServiceFileRecordRegistry instance;

        public static void Load(IEnumerable<ServiceFileRecord> records)
        {
            if (instance == null)
                instance = new ServiceFileRecordRegistry();

            foreach (var record in records)
                AddServiceFileRecord(record);
        }

        public static ServiceFileRecord GetServiceFileRecord(string name)
        {
            if (instance == null)
                instance = new ServiceFileRecordRegistry();

            if (!instance.serviceFileRecords.ContainsKey(name))
                throw new KeyNotFoundException();

            return instance.serviceFileRecords[name];
        }

        public static void AddServiceFileRecord(ServiceFileRecord serviceFileRecord)
        {
            if (instance == null)
                instance = new ServiceFileRecordRegistry();

            if (!instance.serviceFileRecords.ContainsKey(serviceFileRecord.Name))
                instance.serviceFileRecords.Add(serviceFileRecord.Name, serviceFileRecord);
            else
                instance.serviceFileRecords[serviceFileRecord.Name] = serviceFileRecord;
        }

        public static IEnumerable<ServiceFileRecord> GetAllServiceFileRecords()
        {
            return instance.serviceFileRecords.Values;
        }
    }
}
