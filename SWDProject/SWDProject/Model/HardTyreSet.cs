using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class HardTyreSet : TyreSet
    {
        public HardTyreSet() : base()
        { }

        public HardTyreSet(int state)
            : base(state)
        { }

        public override double GetCurrentInfluenceForPerfomance()
        {
            return 2.60887607093972E-08 * Math.Pow(State, 5) - 1.63385090979146E-06 * Math.Pow(State, 4) +
                   0.0000302138469174194 * Math.Pow(State, 3) + 9.66058175429907E-06 * Math.Pow(State, 2) -
                   0.00369329902638001*State + 0.0244105464179507;
        }

        public override string ToString()
        {
            return "Hard";
        }
    }
}
