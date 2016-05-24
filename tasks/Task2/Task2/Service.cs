using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.Sensors;

namespace Task2
{
    public class Service
    {
        public string DisplayName { get; private set; }
        public Server ParentServer { get; private set; }

        public DateTime LastCheck { get; protected set; }
        public TimeSpan LastCheckDuration { get; protected set; }
        public Queue<TimeSpan> LastCheckDurations { get; protected set; }
        public ServiceState LastState { get; protected set; }

        private ISensor sensor;

        public double LastCheckDurationMean {
            get
            {
                return LastCheckDurations.Select(x => x.TotalMilliseconds).Sum() / LastCheckDurations.Count;
            }
        }

        public double LastCheckDurationStdDev
        {
            get
            {
                return StdDev(LastCheckDurations.Select(x => x.TotalMilliseconds));
            }
        }

        public Service(string displayName, Server parentServer, ISensor sensor)
        {
            ParentServer = parentServer;
            DisplayName = displayName;

            this.sensor = sensor;

            LastCheckDurations = new Queue<TimeSpan>(10);
        }

        public Service(string serviceRecordName, Server parentServer)
        {
            var record = ServiceFileRecordRegistry.GetServiceFileRecord(serviceRecordName);

            ParentServer = parentServer;
            DisplayName = record.DisplayName;

            sensor = SensorFactory.BuildSensor(record);

            LastCheckDurations = new Queue<TimeSpan>(10);
        }

        private void DoCheckState()
        {
            DateTime startDate = DateTime.Now;
            SensorState state = sensor.DoCheckState(ParentServer);

            LastCheckDuration = DateTime.Now - startDate;
            LastState = ConvertSensorToServiceState(state);
            LastCheck = startDate;
        }

        public void CheckState()
        {
            DoCheckState();
            SaveLastCheckDuration();
        }

        protected ServiceState ConvertSensorToServiceState(SensorState sensorState)
        {
            switch (sensorState)
            {
                case SensorState.OK:
                    return ServiceState.Reachable;
                case SensorState.Error:
                    return ServiceState.Unreachable;
                case SensorState.Unknown:
                default:
                    return  ServiceState.Unknown;
            }
        }

        private void SaveLastCheckDuration()
        {
            while (LastCheckDurations.Count >= 10)
                LastCheckDurations.Dequeue();

            LastCheckDurations.Enqueue(LastCheckDuration);
        }

        private double StdDev(IEnumerable<double> x)
        {
            if (x.Count() < 2) return 0.0;

            double avg = x.Average();
            double sqrSum = x.Sum((v) => Math.Pow((v - avg), 2));

            return Math.Sqrt(sqrSum / (x.Count() - 1));
        }
    }
}
