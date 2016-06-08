using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SWDProject.Model
{
    public class DriveSimulation
    {
        public Race Race { get; set; }
        public Car Car { get; set; }
        public CarStateTree CarStateTree { get; set; }

        public Task<ResultSet> Start()
        {
            var methodResult = Task.Run(() =>
            {
                var result = new ResultSet();
                
                foreach (var lastCarState in GetLastCarStatesWhereAllTyreTypesInUse())
                {
                    RacePlan racePlan = new RacePlan(lastCarState);
                    List<RacePlan> racePlans = new List<RacePlan> { racePlan };
                    for (int i = Race.NumberOfLaps; i > 0; i--)
                    {
                        List<RacePlan> updatedRacePlans = UpdateRacePlansWithAnotherLap(racePlans);
                        racePlans.Clear();
                        racePlans.AddRange(updatedRacePlans);
                        
                        if (CheckForMergeInCarStateTree(racePlans))
                        {
                            RemoveSlowerRacePlans(racePlans);
                        }
                    }
                    result.BestRacePlanForEachLeaf.Add(racePlans[0]);
                }
                result.BestRacePlanOverall = GetFastestRacePlan(result.BestRacePlanForEachLeaf);
               
                return result;
            });

            return methodResult;

        }

        private void RemoveSlowerRacePlans(List<RacePlan> racePlans)
        {
            var duplicateGroups = (from p in racePlans
                                   group p by p.CarStates[p.CarStates.Count - 1]
                                       into g
                                       where g.Count() > 1
                                       select g).SelectMany(g => g).ToList();

            foreach (var dupGroup in duplicateGroups.GroupBy(x => x.CarStates[x.CarStates.Count - 1]))
            {
                TimeSpan minTime = new TimeSpan(999, 23, 59, 59);
                RacePlan minRacePlan = new RacePlan();
                foreach (var dup in dupGroup.ToList())
                {
                    racePlans.Remove(dup);
                    if (dup.TotalTime < minTime)
                    {
                        minRacePlan = dup;
                        minTime = dup.TotalTime;
                    }
                }
                racePlans.Add(minRacePlan);
            }
        }

        private List<RacePlan> UpdateRacePlansWithAnotherLap(List<RacePlan> racePlans)
        {
            List<RacePlan> newRacePlans = new List<RacePlan>();
            foreach (var pt in racePlans)
            {
                if (pt.CarStates[pt.CarStates.Count - 1].Predecessors.Count != 0)
                {
                    if (pt.CarStates[pt.CarStates.Count - 1].Predecessors.Count == 1)
                    {
                        RacePlan newRacePlan = new RacePlan(pt.CarStates);
                        newRacePlan.AddCarState(pt.CarStates[pt.CarStates.Count - 1].Predecessors[0]);
                        newRacePlans.Add(newRacePlan);
                    }
                    else
                    {
                        foreach (var pred in pt.CarStates[pt.CarStates.Count - 1].Predecessors)
                        {
                            RacePlan newRacePlan = new RacePlan(pt.CarStates);
                            newRacePlan.AddCarState(pred);
                            newRacePlans.Add(newRacePlan);
                        }
                    }
                }
            }
            return newRacePlans;
        }

        private bool CheckForMergeInCarStateTree(List<RacePlan> racePlans)
        {
            return racePlans.GroupBy(x => x.CarStates[x.CarStates.Count - 1]).Any(g => g.Count() != 1);
        }

        private IEnumerable<CarState> GetLastCarStatesWhereAllTyreTypesInUse()
        {
            return CarStateTree.Leaves.Where(e => e.UsedRequiredTypesOfTyres);
        }

        private RacePlan GetFastestRacePlan(List<RacePlan> racePlans)
        {
            TimeSpan minTime = new TimeSpan(999, 23, 59, 59);
            RacePlan minRacePlan = null;
            foreach (var racePlan in racePlans)
            {
                if (racePlan.TotalTime < minTime)
                {
                    minRacePlan = racePlan;
                    minTime = racePlan.TotalTime;
                }
            }
            return minRacePlan;
        }
    }
}