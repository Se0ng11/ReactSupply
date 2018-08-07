using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NLog;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System.Linq;
using System.Collections.Generic;


namespace ReactSupply.Logic
{
    public class BaseLogic
    {
        protected readonly SupplyChainContext _context;
        protected ResponseMessage rm = new ResponseMessage();
        protected static Logger _Logger { get; set; } = LogManager.GetCurrentClassLogger();

        public BaseLogic(SupplyChainContext context)
        {
            _context = context;
       
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

        public string SingleValuePair(List<ValuePair> vp, string name)
        {
            return vp.Where(x => x.Name == name).Select(x => x.Value).SingleOrDefault();
        }
    }

   
}
