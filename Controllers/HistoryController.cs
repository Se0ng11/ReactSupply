using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Bundles;
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
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(SupplyChainContext context,
            ILogger<HistoryController> logger)
            :base(context)
        {
            _logger = logger;
        }

        [HttpGet("[action]")]
        public string GetHistory(string identifier)
        {
            var obj = new HistoryLogic(_context);
            var header = obj.SelectSchemaHeaderSync();
            var data = obj.SelectSpecificdata(identifier);

            Task.WaitAll(data);

            var result = new TableFormatter { Header = header, Body = data.Result };

            return Tools.ConvertToJSON(result);
        }

    }
}