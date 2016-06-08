using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class CarStateTree
    {
        public List<CarState> GrabbingBag { get; set; } = new List<CarState>();
        public HashSet<CarState> CarStates { get; set; }
        public CarState BeginningCarState { get; set; }
        public List<CarState> Leaves { get; set; } = new List<CarState>();
        public int NumberOfNodes { get; set; }
        public List<CarState> AllCarStates { get; set; } = new List<CarState>();


        public CarStateTree(CarState beginningCarState)
        {
            BeginningCarState = beginningCarState;
            CreateGrabbingBag();
            CarStates = new HashSet<CarState>();
            CarStates.Add(BeginningCarState);
            CreateCarStateTree();
        }

        private void CreateGrabbingBag()
        {
            GrabbingBag.Clear();
            for (int i = 0; i <= BeginningCarState.Car.Race.SuggestedLapsForSoftTyres; i++)
            {
                GrabbingBag.Add(new CarState(BeginningCarState.Car, new SoftTyreSet(i), 0));
                GrabbingBag.Add(new CarState(BeginningCarState.Car, new SoftTyreSet(i), 0, true));
            }

            for (int i = 0; i <= BeginningCarState.Car.Race.SuggestedLapsForHardTyres; i++)
            {
                GrabbingBag.Add(new CarState(BeginningCarState.Car, new HardTyreSet(i), 0));
                GrabbingBag.Add(new CarState(BeginningCarState.Car, new HardTyreSet(i), 0, true));
            }
        }

        public void GrabSuccessors(CarState grabber, int onLap)
        {
            foreach (var cs in GrabbingBag)
            {
                if (grabber.UsedRequiredTypesOfTyres)
                {
                    if (!cs.UsedRequiredTypesOfTyres)
                        continue;
                    if (cs.TyreSet.IsBrandNew)
                    {
                        cs.OnLap = onLap;
                        grabber.AddSuccessor(cs);
                        CarStates.Add(cs);
                    }
                    if (cs.TyreSet.State == grabber.TyreSet.State + 1 &&
                        cs.TyreSet.IsTheSameTyreType(grabber.TyreSet))
                    {
                        cs.OnLap = onLap;
                        grabber.AddSuccessor(cs);
                        CarStates.Add(cs);
                    }
                }
                else
                {
                    if (cs.UsedRequiredTypesOfTyres)
                    {
                        if (cs.TyreSet.IsBrandNew &&
                            !cs.TyreSet.IsTheSameTyreType(grabber.TyreSet))
                        {
                            cs.OnLap = onLap;
                            grabber.AddSuccessor(cs);
                            CarStates.Add(cs);
                        }
                    }
                    else
                    {
                        if (!cs.TyreSet.IsTheSameTyreType(grabber.TyreSet))
                            continue;
                        if (cs.TyreSet.IsBrandNew || cs.TyreSet.State == grabber.TyreSet.State + 1)
                        {
                            cs.OnLap = onLap;
                            grabber.AddSuccessor(cs);
                            CarStates.Add(cs);
                        }
                    }
                }
                
            }
              
        }


        public void CreateCarStateTree()
        {
            for (int i = 1; i <= BeginningCarState.Car.Race.NumberOfLaps; i++)
            {
                List<CarState> removableCarStates = new List<CarState>();
                removableCarStates.AddRange(CarStates); 
                
                foreach (var cs in removableCarStates)
                {
                    GrabSuccessors(cs,i);
                    CarStates.Remove(cs);

                    AllCarStates.Add(cs);
                    NumberOfNodes++;
                }
                CreateGrabbingBag();
            }
            Leaves.AddRange(CarStates);
            NumberOfNodes += CarStates.Count;
            AllCarStates.AddRange(Leaves);
        }
    }
}