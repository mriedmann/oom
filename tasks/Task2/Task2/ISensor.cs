using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public interface ISensor
    {
        /// <summary>
        /// Checks state of given target.
        /// Should be multithreading-aware, stateless and should not throw exceptions.
        /// </summary>
        /// <param name="target">Target Server</param>
        /// <returns>Returns OK if successfull, ERROR if not</returns>
        SensorState DoCheckState(Server target);
    }
}
