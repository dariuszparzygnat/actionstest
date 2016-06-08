using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class PerformanceSet
    {
        private TimeSpan _theBestLapTime;

        public TimeSpan BestLapTime { get { return _theBestLapTime;} }

        public PerformanceSet(TimeSpan bestLapTime)
        {
            _theBestLapTime = bestLapTime;
        }

    }
}
