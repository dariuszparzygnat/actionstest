using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class Race
    {
        private int _currentLap = 0;
        public TimeSpan PitStopTime { get; }
        public int SuggestedLapsForSoftTyres { get; set; }
        public int SuggestedLapsForHardTyres { get; set; }

        public int CurrentLap => _currentLap;

        public int NumberOfLaps { get; }

        public Race(int numberOfLaps, TimeSpan pitStopTime, int suggestedLapsForSoftTyres, int suggestedLapsForHardTyres)
        {
            PitStopTime = pitStopTime;
            NumberOfLaps = numberOfLaps;
            SuggestedLapsForSoftTyres = suggestedLapsForSoftTyres;
            SuggestedLapsForHardTyres = suggestedLapsForHardTyres;
        }
    }
}
