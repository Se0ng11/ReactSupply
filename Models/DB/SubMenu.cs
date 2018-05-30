using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.DB
{
    public class SubMenu
    {
        public string SubCode { get; set; }
        public string SubName { get; set; }
        public string SubClass { get; set; }
        public string SubParent { get; set; }
        public string MenuCode { get; set; }
        public decimal Position { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }

        public Menu Menu { get; set; }
    }
}
