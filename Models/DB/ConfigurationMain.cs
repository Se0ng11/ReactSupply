using System;
using System.Collections.Generic;

namespace ReactSupply.Models.DB
{
    public partial class ConfigurationMain
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ValueName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string DefaultText { get; set; }
        public string ControlType { get; set; }
        public decimal? Position { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public int? Width { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsDisplay { get; set; }
        public bool? IsRequired { get; set; }
        public string TabGroup { get; set; }
        public string Css { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
