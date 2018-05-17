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
        public bool IsLocked { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsEditable { get; set; }
        public bool IsVisible { get; set; }
        public bool IsRequired { get; set; }
        public bool IsResizeable { get; set; }
        public bool IsSortable { get; set; }
        public string Formatter { get; set; }
        public string HeaderRenderer { get; set; }
        public string Editor { get; set; }
        public string FilterRenderer { get; set; }
        public string Group { get; set; }
        public string HeaderCss { get; set; }
        public string BodyCss { get; set; }
    }
}
