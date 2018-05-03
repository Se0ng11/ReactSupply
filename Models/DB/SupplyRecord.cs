using System;
using System.Collections.Generic;

namespace ReactSupply.Models.DB
{
    public partial class SupplyRecord
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ValueName { get; set; }
        public string AxNumber { get; set; }
        public string Data { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
