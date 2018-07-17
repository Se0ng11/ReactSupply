using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.DB
{
    public class Tracker
    {
        public string TargetField { get; set; }
        public string AffectField { get; set; }
        public int TotalDay { get; set; }
    }
}
