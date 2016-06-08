using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class Lap
    {
        public TimeSpan LapTime { get; set; }
        private const double y = 0.07621;
        
        public static TimeSpan GetLapTimeUsingCarStates(CarState fromCarState, CarState toCarState)
        {
            if(toCarState.TyreSet.State==(toCarState.TyreSet is SoftTyreSet ? toCarState.Car.Race.SuggestedLapsForSoftTyres : toCarState.Car.Race.SuggestedLapsForHardTyres))
            {
                return new TimeSpan(1,1,1,1);
            }

            double fuelInfluence = 5e-5 * (fromCarState.Car.Race.NumberOfLaps - toCarState.OnLap);
            double BestLapTimeTicks = toCarState.Car.PerformanceSet.BestLapTime.Ticks;

            double lapTimeTicks = y*BestLapTimeTicks + fuelInfluence * BestLapTimeTicks + fromCarState.TyreSet.GetCurrentInfluenceForPerfomance() * BestLapTimeTicks + BestLapTimeTicks;

            long longAverageTicks = Convert.ToInt64(lapTimeTicks);
            TimeSpan lapTime = new TimeSpan(longAverageTicks);
            if (toCarState.TyreSet.State == 0)
            {
                lapTime += fromCarState.Car.Race.PitStopTime;
            }
            
            return lapTime;
        }
    }
}
