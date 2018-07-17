using System;

namespace ReactSupply.Models.DB
{
    public class MainProduct
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public bool IsRecount { get; set; }
        public string Parent { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
