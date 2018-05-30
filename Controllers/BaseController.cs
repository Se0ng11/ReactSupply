using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReactSupply.Models.DB;

namespace ReactSupply.Controllers
{
    public class BaseController: Controller
    {
        protected SupplyChainContext _context;
        protected JsonResult FormatJSON(object result)
        {
            return Json(JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
