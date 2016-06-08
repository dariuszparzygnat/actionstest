using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class SoftTyreSet : TyreSet
    {

        public SoftTyreSet() : base()
        { }

        public SoftTyreSet( int state)
            : base(state)
        {
        }

        public override double GetCurrentInfluenceForPerfomance()
        {
            return Math.Pow(State, 6) * 1.06385857602206E-09 - 9.02513695569588E-08 * Math.Pow(State, 5) +
                   3.16473034333076E-06 * Math.Pow(State, 4) - 0.0000625457981661086 * Math.Pow(State, 3) +
                   0.000785092529869687 * Math.Pow(State, 2) - 0.0054005486535776 * State + 0.0141112902998791;
        }

        public override string ToString()
        {
            return "Soft";
        }

    }
}
