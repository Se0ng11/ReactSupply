using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactSupply.Interface;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Utils;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{

    [Authorize]
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(SupplyChainContext context,
            ILogger<HomeController> logger)
            :base(context)
        {
            _logger = logger;
        }

        [HttpGet("[action]")]
        public string GetSupplyRecord()
        {
            var obj = new ConfigurationMainLogic(_context);
            var obj1 = new SupplyRecordLogic(_context);
            var data = obj.SelectHeader();
            var data1 = obj1.SelectAllDataAsync();

            Task.WhenAll(data, data1);

            var result = new TableFormatter { Header = data.Result, Body = data1.Result };
            return Tools.ConvertToJSON(result);
        }

        [HttpGet("[action]")]
        public string GetConfiguration()
        {
            var obj = new ConfigurationMainLogic(_context);
            var data = obj.SelectSchemaHeaderSync();
            var data1 = obj.SelectAllDataAsync();

            Task.WaitAll(data1);

            var result = new TableFormatter { Header = data, Body = data1.Result };

            return Tools.ConvertToJSON(result);
        }


        [HttpPost("[action]")]
        public string PostSupplyRecords([FromBody]RequestData modal)
        {
            var obj = new SupplyRecordLogic(_context);

            var result = PostData(modal, obj);
            
            if (result == Status.MessageType.SUCCESS)
            {
                _responseMessage.Status = result.ToString();
                _responseMessage.Data = Messages.SAVESUCCESS;
            }
            else
            {
                _responseMessage.Status = result.ToString();
                _responseMessage.Data = Messages.SAVESUCCESS;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }


        [HttpPost("[action]")]
        public string PostSingleConfigurationField([FromBody]RequestData modal)
        {
            var obj = new ConfigurationMainLogic(_context);
            var result = PostData(modal, obj);

            if (result == Status.MessageType.SUCCESS)
            {
                _responseMessage.Status = result.ToString();
                _responseMessage.Data = Messages.SAVESUCCESS;
            }
            else
            {
                _responseMessage.Status = result.ToString();
                _responseMessage.Data = Messages.SAVESUCCESS;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }




    }
}