namespace ReactSupply.Models.DTO
{
    public class ConfigurationMainDTO
    {
        public string DisplayName { get; set; }
        public string ControlType { get; set; }
        public decimal? Position { get; set; }
        //public Nullable<int> MinLength { get; set; }
        //public Nullable<int> MaxLength { get; set; }
        //public Nullable<int> Width { get; set; }
        //public Nullable<bool> IsEnabled { get; set; }
        //public Nullable<bool> IsDisplay { get; set; }
        //public Nullable<bool> IsRequired { get; set; }
        public string TabGroup { get; set; }
        public string Css { get; set; }
    }
}
