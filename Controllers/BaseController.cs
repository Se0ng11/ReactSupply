using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;

namespace ReactSupply.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly SupplyChainContext _context;
        public BaseController(SupplyChainContext context)
        {
            _context = context;
        }
    }
}
