using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.DB
{
    public class LineProduct
    {
        public int Id { get; set; }
        [Key]
        public string Identifier { get; set; }
        public string Size { get; set; }
        public decimal Quantity { get; set; }
        public bool IsRecount { get; set; }
        public string Parent { get; set; }
        public DateTime? SODate { get; set; }
        public DateTime? OriginalDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public bool? IsOnTime { get; set; }

        [ForeignKey("Parent")]
        public MainProduct MainProduct { get; set; }
    }
}
