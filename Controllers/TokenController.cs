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
            var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Static.Messages.COMPANYNAME, Static.Messages.REFRESHTOKEN);

            if (token.Refresh == currentRefresh)
            {
                var newToken = new JwtTokenLogic().GenerateJwtToken(user.UserName, currentRefresh, out string outRefreshToken);

                _responseMessage.Status = Static.Response.MessageType.SUCCESS.ToString();
                _responseMessage.Result = newToken;
            }
            else
            {

                _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                _responseMessage.Result = Static.Messages.INVALIDTOKEN;
            }

            return FormatJSON(_responseMessage);

        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<JsonResult> RemoveToken([FromBody] JwtTokenResponse token)
        {

            ApplicationUser user = await _userManager.FindByNameAsync(token.UserId);
            var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Static.Messages.COMPANYNAME, Static.Messages.REFRESHTOKEN);

            if (token.Refresh == currentRefresh)
            {
                var deletedToken = await _userManager.RemoveAuthenticationTokenAsync(user, Static.Messages.COMPANYNAME, Static.Messages.REFRESHTOKEN);

                if (deletedToken.Succeeded) { 
                    _responseMessage.Status = Static.Response.MessageType.SUCCESS.ToString();
                }
                else
                {
                    _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                    _responseMessage.Result = Static.Messages.INVALIDTOKEN;
                }
            }
            else
            {
                _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                _responseMessage.Result = Static.Messages.INVALIDTOKEN;
            }
            return FormatJSON(_responseMessage);
        }
    }
}