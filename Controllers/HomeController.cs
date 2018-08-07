using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Utils;
using System;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SettingLogic _setting;
        private readonly ILogger<HomeController> _logger;

        public HomeController(SupplyChainContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
            :base(context)
        {
            _logger = logger;
            _userManager = userManager;
            _setting = new SettingLogic(_context);
        }

        [HttpGet("[action]")]
        public async Task<string> GetSupplyRecord(RequestData modal)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var isGuest = (User.Identity.Name == _setting.GetSuperId()) ? true : await _userManager.IsInRoleAsync(user, "Guest");

            var obj = new ConfigurationMainLogic(_context);
            var obj1 = new SupplyRecordLogic(_context);
            var data = obj.SelectHeader(Convert.ToInt32(modal.identifier), isGuest);
            var data1 = obj1.SelectMenuData(modal.identifier, modal.updated);

            await Task.WhenAll(data, data1);

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
                _responseMessage.Data = Messages.SAVEFAILED;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }




    }
}