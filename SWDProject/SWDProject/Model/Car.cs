using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class Car
    {
        public Race Race { get; }

        public PerformanceSet PerformanceSet { get; }


        public Car(PerformanceSet performanceSet, Race race)
        {
            Race = race;
            PerformanceSet = performanceSet;
        }
    }
}
