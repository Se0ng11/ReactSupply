using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Utils;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SettingLogic _setting;
        private readonly ILogger<MenuController> _logger;

        public MenuController(SupplyChainContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<MenuController> logger)
            :base(context)
        {
            _userManager = userManager;
            _setting = new SettingLogic(context);
            _logger = logger;
        }

        //[HttpGet("[action]")]
        //public string GetMenuDisplay()
        //{
        //    var obj = new MenuLogic(_context);
        //    var data = obj.SelectVisibleData();

        //    return data;
        //}

        [HttpGet("[action]")]
        public string GetSubMenuDisplay()
        {
            var obj = new SubMenuLogic(_context);
            var data = obj.SelectVisibleData();

            return data;
        }


        [HttpGet("[action]")]
        public async Task<string> GetMenu()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            var superId = _setting.GetSuperId();
            var obj = new MenuLogic(_context);
            var obj1 = new SubMenuLogic(_context);

            string data = "";

            if (User.Identity.Name == superId)
            {
                data = await obj.SelectVisibleData();
            }
            else
            {
                var role = await _userManager.GetRolesAsync(user);
                data = obj.SelectVisibleData(role[0]);
            }

            var data1 = obj1.SelectVisibleData();
          
            var format = new { top = data, left= data1 };

            return Tools.ConvertToJSON(format);
        }

    }
}