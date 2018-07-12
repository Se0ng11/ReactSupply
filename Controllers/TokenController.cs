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
    [Route("api/Token")]
    public class TokenController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SettingLogic _setting;
        private readonly ILogger<TokenController> _logger;

        public TokenController(SupplyChainContext context, 
            UserManager<ApplicationUser> userManager,
            ILogger<TokenController> logger) 
            :base(context)
        {
            _userManager = userManager;
            _logger = logger;
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
                var newToken = Tools.GenerateJwtToken(_setting, token.UserId, token.Role, token.Refresh, isSuper, out string outRefreshToken);

                _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                _responseMessage.Result = newToken;
            }
            else
            {
                var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Messages.COMPANYNAME, Messages.REFRESHTOKEN);

                if (token.Refresh == currentRefresh)
                {
                    var newToken = Tools.GenerateJwtToken(_setting, user.UserName, token.Role, currentRefresh, isSuper, out string outRefreshToken);

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