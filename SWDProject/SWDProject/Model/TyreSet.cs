using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public abstract class TyreSet
    {
        


        /// <summary>
        ///  0 - brand new
        /// </summary>
        public int State { get; set; }

        public bool IsBrandNew { get { return State == 0; } }

        public TyreSet()
        {}

        public TyreSet(int state)
        {
            State = state;
        }

        public bool IsTheSameTyreType(TyreSet tyreSet)
        {
            return GetType() == tyreSet.GetType();
        }

        /// <summary>
        /// Count how big influence has tyre for performance
        /// </summary>
        /// <returns>Multiplier for lap time </returns>
        public abstract double GetCurrentInfluenceForPerfomance();
    }
}
