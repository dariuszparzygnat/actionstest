using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class RacePlan
    {
        private List<TimeSpan> _lapTimes;
        public List<CarState> CarStates { get; set; } = new List<CarState>(); 
        public TimeSpan TotalTime { get; set; }
        public List<TimeSpan> LapTimes {
            get
            {
                if (_lapTimes == null)
                {
                    _lapTimes = new List<TimeSpan>();
                    GenerateLapTimes();
                }
                return _lapTimes;
            } }

        public RacePlan()
        {
        }

        public RacePlan(CarState carState)
        {
            CarStates = new List<CarState> { carState };
            TotalTime = new TimeSpan();
        }

        public RacePlan(List<CarState> carStates)
        {
            CarStates.AddRange(carStates);
            TotalTime = new TimeSpan();
            CalculateTime();

        }

        public void AddCarState(CarState carState)
        {
            CarStates.Add(carState);
            CalculateTime();

        }

        private void CalculateTime()
        {
            TotalTime = new TimeSpan();
            var reversedCarStates = new List<CarState>(CarStates);
            reversedCarStates.Reverse();
            for (int i = 0; i < reversedCarStates.Count - 1; i++)
            {
                var lapTime = Lap.GetLapTimeUsingCarStates(reversedCarStates[i], reversedCarStates[i + 1]); 
                TotalTime += lapTime;
            }
        }

        private void GenerateLapTimes()
        {
            for (int i = 0; i < CarStates.Count - 1; i++)
            {
                var lapTime = Lap.GetLapTimeUsingCarStates(CarStates[i], CarStates[i + 1]);
                LapTimes.Add(lapTime);
            }
        }
    }
}
