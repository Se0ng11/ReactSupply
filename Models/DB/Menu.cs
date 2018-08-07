using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.DB
{
    public class Menu
    {
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public string MenuClass { get; set; }
        public decimal Position { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        
        public ICollection<SubMenu> SubMenus { get; set; }
    }
}
