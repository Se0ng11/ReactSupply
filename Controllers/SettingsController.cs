using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Utils;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Settings")]
    public class SettingsController : BaseController
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly SettingLogic _setting;

        public SettingsController(SupplyChainContext context,
            ILogger<SettingsController> logger)
        :base(context)
        {
            _logger = logger;
            _setting = new SettingLogic(context);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public string GetAppInfo()
        {
            var appId = _setting.GetSettingValue("AppId");
            var adAuth = _setting.GetSettingValue("ADAuth");

            var combine = new
            {
                appId,
                adAuth
            };


            return Tools.ConvertToJSON(combine);
        }

        [HttpGet("[action]")]
        public string GetAdSearch()
        {
            var appId = _setting.GetSettingValue("AppId");
            var adAuth = _setting.GetSettingValue("ADSearch");

            var combine = new
            {
                appId,
                adAuth
            };


            return Tools.ConvertToJSON(combine);
        }
    }


}