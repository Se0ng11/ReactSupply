﻿namespace ReactSupply.Models.DTO
{
    public class ReactDataFormatter
    {
        public string key { get; set; }
        public string name { get; set; }
        public int width { get; set; }
        public bool locked { get; set; }
        public bool sortable { get; set; }
        public bool editable { get; set; }
        public bool filterable { get; set; }
        public bool resizable { get; set; }
        public string cellClass { get; set; }
        public string formatter { get; set; }
        public string headerRenderer { get; set; }
        public string editor { get; set; }
        public object filterRenderer { get; set; }

    }
}
