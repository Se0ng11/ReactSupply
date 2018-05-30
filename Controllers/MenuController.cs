using Microsoft.AspNetCore.Mvc;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : BaseController
    {
        public MenuController(SupplyChainContext configuration) => _context = configuration;

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
    }
}