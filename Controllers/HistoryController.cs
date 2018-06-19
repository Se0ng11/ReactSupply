using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Produces("application/json")]
    [Route("api/History")]
    public class HistoryController : BaseController
    {
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(SupplyChainContext configuration, ILogger<HistoryController> logger)
        {
            _context = configuration;
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