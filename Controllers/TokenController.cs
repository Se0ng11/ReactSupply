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
    [Route("api/Token")]
    public class TokenController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ResponseMessage _responseMessage = new ResponseMessage();
        private readonly SettingLogic _setting;
        public TokenController(SupplyChainContext context, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ILogger<HistoryController> logger) 
            :base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _setting = new SettingLogic(_context);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<string> RefreshToken([FromBody]JwtTokenResponse token)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(token.UserId);

            bool isSuper = token.UserId == _setting.GetSuperId();

            if (isSuper)
            {
                var newToken = Tools.GenerateJwtToken(_setting, token.UserId, token.Refresh, isSuper, out string outRefreshToken);

                _responseMessage.Status = Bundles.Status.MessageType.SUCCESS.ToString();
                _responseMessage.Result = newToken;
            }
            else
            {
                var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Messages.COMPANYNAME, Messages.REFRESHTOKEN);

                if (token.Refresh == currentRefresh)
                {
                    var newToken = Tools.GenerateJwtToken(_setting, user.UserName, currentRefresh, isSuper, out string outRefreshToken);

                    _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                    _responseMessage.Result = newToken;
                }
                else
                {

                    _responseMessage.Status = Status.MessageType.FAILED.ToString();
                    _responseMessage.Result = Messages.INVALIDTOKEN;
                }
            }
          

            return Tools.ConvertToJSON(_responseMessage);

        }
    }
}