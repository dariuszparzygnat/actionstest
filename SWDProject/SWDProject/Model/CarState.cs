using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class CarState
    {
        private bool _usedSoftTyre;
        private bool _usedHardTyre;

        public int OnLap { get; set; }
        public TyreSet TyreSet { get; set; }

        public bool UsedRequiredTypesOfTyres { get; set; } = false;

        public List<CarState> Predecessors { get; } = new List<CarState>(); 
        public List<CarState> Successors { get; } = new List<CarState>(); 
        public Car Car { get; }

        
        public CarState(Car car, TyreSet tyreSet, int onLap)
        {
            Car = car;
            TyreSet = tyreSet;
            OnLap = onLap;
        }

        public CarState(Car car, TyreSet tyreSet, int onLap, bool usedRequiredTypesOfTyres)
        {
            Car = car;
            TyreSet = tyreSet;
            OnLap = onLap;
            UsedRequiredTypesOfTyres = usedRequiredTypesOfTyres;
        }

        public void AddSuccessor(CarState carState)
        {
            this.Successors.Add(carState);
            carState.Predecessors.Add(this);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash*7) + this.OnLap.GetHashCode();
            hash = (hash*7) + this.TyreSet.State.GetHashCode();
            hash = (hash*7) + this.TyreSet.State.GetType().GetHashCode();
            hash = (hash*7) + this.UsedRequiredTypesOfTyres.GetHashCode();
            return hash;
        }
        
    }
}
