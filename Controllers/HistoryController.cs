using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/History")]
    public class HistoryController : BaseController
    {
        private readonly SupplyChainContext _context;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(SupplyChainContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<HistoryController> logger)
            :base(context, userManager, signInManager, logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public JsonResult GetHistory(string identifier)
        {
            var obj = new HistoryLogic(_context);
            var header = obj.SelectSchemaHeaderSync();
            var data = obj.SelectSpecificdata(identifier);

            Task.WaitAll(data);

            var result = new TableFormatter { Header = header, Body = data.Result };

            return FormatJSON(result);
        }

    }
}