using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : BaseController
    {
        private readonly SupplyChainContext _context;
        private readonly ILogger<HistoryController> _logger;

        public MenuController(SupplyChainContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<HistoryController> logger) :base(context, userManager, signInManager,logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public string GetMenuDisplay()
        {
            var obj = new MenuLogic(_context);
            var data = obj.SelectVisibleData();

            return data;
        }

        [HttpGet("[action]")]
        public string GetSubMenuDisplay()
        {
            var obj = new SubMenuLogic(_context);
            var data = obj.SelectVisibleData();

            return data;
        }


        [HttpGet("[action]")]
        public JsonResult GetMenu()
        {
            var obj = new MenuLogic(_context);
            var obj1 = new SubMenuLogic(_context);

            var data = obj.SelectVisibleData();
            var data1 = obj1.SelectVisibleData();
          
            var format = new { top = data, left= data1 };

            return FormatJSON(format);
        }

    }
}