﻿using System;
using System.Collections.Generic;

namespace ReactSupply.Models.DB
{
    public class MainProduct
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Size { get; set; }
        public decimal Quantity { get; set; }
        public bool IsRecount { get; set; }
        public DateTime? SODate { get; set; }
        public DateTime? OriginalDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public bool? IsOnTime { get;set;}
        public ICollection<LineProduct> LineProduct { get; set; }
    }
}
