using System.ComponentModel.DataAnnotations;

namespace ReactSupply.Models.DB
{
    public class JSONResult
    {
        [Key]
        public string JsonResult { get; set; }
    }
}
