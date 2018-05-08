using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.DTO
{
    public class ReactTableFormatter
    {
        public string Header { get; set; }
        public columns[] columns { get; set; }
        
    }

    public class columns
    {
        public string Header { get; set; }
        public string accessor { get; set; }
        public string id { get; set;  }
        public string className { get; set; }
        public bool show { get; set; }
        public string Cell { get; set; }

    }
}
