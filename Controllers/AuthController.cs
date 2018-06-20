using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ResponseMessage _responseMessage = new ResponseMessage();
        private readonly string _loginInfo = "Incorrect id or password";

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<JsonResult> LoginAsync([FromBody]LoginViewModel loginVm)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(loginVm.Id);

                if (user != null)
                {
                    var isSuccess = await _userManager.CheckPasswordAsync(user, loginVm.Password);

                    if (isSuccess)
                    {
                        await _signInManager.SignInAsync(user, true);
                 
                        _responseMessage.Status = Enum.Response.MessageType.SUCCESS.ToString();
                    }
                    else
                    {
                        _responseMessage.Status = Enum.Response.MessageType.FAILED.ToString();
                        _responseMessage.Result = _loginInfo;
                    }
                }
                else
                {
                    _responseMessage.Status = Enum.Response.MessageType.FAILED.ToString();
                    _responseMessage.Result = _loginInfo;
                }
            }
            catch(Exception ex)
            {
                _responseMessage.Status = Enum.Response.MessageType.FAILED.ToString();
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