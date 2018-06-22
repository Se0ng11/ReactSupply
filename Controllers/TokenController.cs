using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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


        public TokenController(SupplyChainContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<HistoryController> logger) : base(context, userManager, signInManager, logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<JsonResult> RefreshToken([FromBody]JwtTokenResponse token)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(token.UserId);
            var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Static.Const.COMPANYNAME, Static.Const.REFRESHTOKEN);

            if (token.Refresh == currentRefresh)
            {
                var newToken = new JwtTokenLogic().GenerateJwtToken(user.UserName, currentRefresh, out string outRefreshToken);

                _responseMessage.Status = Static.Response.MessageType.SUCCESS.ToString();
                _responseMessage.Result = newToken;
            }
            else
            {

                _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                _responseMessage.Result = Static.Const.UNAUTHORIZED;
            }

            return FormatJSON(_responseMessage);

        }
    }
}