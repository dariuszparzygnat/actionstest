using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SWDProject.Model;

namespace SWDProject.ViewModel
{
    public class DataItem
    {
        public int Lap { get; set; }
        public string TyreType { get; set; } 
        public TimeSpan LapTime { get; set; }

        public int TyreState { get; set; }

        public Color RowColor => TyreState == 0 ? Colors.Yellow : Colors.White;
    }
}
