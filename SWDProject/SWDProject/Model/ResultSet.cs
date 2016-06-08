using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWDProject.Model
{
    public class ResultSet
    {
        public ResultSet()
        {
            BestRacePlanOverall = new RacePlan();
            BestRacePlanForEachLeaf = new List<RacePlan>();
        }
        public RacePlan BestRacePlanOverall { get; set; }
        public List<RacePlan> BestRacePlanForEachLeaf { get; set; }
        
    }
}
