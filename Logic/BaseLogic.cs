using Newtonsoft.Json;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;

namespace ReactSupply.Logic
{
    public class BaseLogic
    {
        protected SupplyChainContext _context;
        protected ResponseMessage rm = new ResponseMessage();

        public string ConvertToJSON(object result)
        {
            return JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public ReactDataFormatter FixedIndexColumn()
        {
            return new ReactDataFormatter
            {
                key = "No",
                name = "No.",
                locked = true,
                editable = false,
                control = "identity",
                cellClass = "history",
                width = 100,
                filterable = true,
                sortable = true,
                resizable = true,
                filterRenderer = "NumericFilter"
            };
        }

    }

   
}
