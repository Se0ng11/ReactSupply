using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.DB
{
    public class History
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Identifier { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
