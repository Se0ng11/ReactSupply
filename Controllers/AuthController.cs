using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Models.ViewModel.User;
using System;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ResponseMessage _responseMessage = new ResponseMessage();

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginAsync([FromBody]LoginViewModel model)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
                
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    //var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Static.Const.COMPANYNAME, Static.Const.REFRESHTOKEN);
                    var token = new JwtTokenLogic().GenerateJwtToken(model.UserName, "", out string outRefreshToken);
                 
                    await _userManager.SetAuthenticationTokenAsync(user, Static.Const.COMPANYNAME, Static.Const.REFRESHTOKEN, outRefreshToken);
                    await _signInManager.SignInAsync(user, true);
                    var s = await _signInManager.CreateUserPrincipalAsync(user);

                    _responseMessage.Status = Static.Response.MessageType.SUCCESS.ToString();
                    _responseMessage.Result = token;
                }
                else
                {
                    _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                    _responseMessage.Result = Static.Const.LOGININFO;
                }
            }
            catch(Exception ex)
            {
                _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }
          
            return FormatJSON(_responseMessage);
        }

        [HttpGet]
        public async Task<JsonResult> RefreshToken(JwtTokenResponse token)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);  
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

        [HttpGet]
        public async Task<JsonResult> RegisterAsync([FromBody]RegisterViewModel model)
        {
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _responseMessage.Status = Static.Response.MessageType.SUCCESS.ToString();
                }
                else
                {
                    string message = "";
                    foreach(var error in result.Errors)
                    {
                        message += error.Description;
                    }
                    _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                    _responseMessage.Result = message;

                }

            }
            catch(Exception ex)
            {
                _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }

            return FormatJSON(_responseMessage);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return View();
        }
    }
}