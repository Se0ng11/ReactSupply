using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Models.ViewModel.User;
using ReactSupply.Utils;
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
        private readonly ILogger<AuthController> _logger;
        private readonly SettingLogic _setting;

        public AuthController(SupplyChainContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthController> logger)
            :base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            _setting = new SettingLogic(_context);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<string> LoginAsync([FromBody]LoginViewModel model)
        {
            try
            {
                bool isSuper = model.UserName == _setting.GetSuperId();
                ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
                string token = "";

                if (user == null)
                {
                    token = Tools.GenerateJwtToken(_setting, model.UserName, "", "", isSuper, out string outRefreshToken);
                    _responseMessage = await LoginResult(user, isSuper, token, outRefreshToken);
                }
                else
                {
                    var role = await _userManager.GetRolesAsync(user);
                    token = Tools.GenerateJwtToken(_setting, model.UserName, role[0], "", isSuper, out string outRefreshToken);
                    _responseMessage = await LoginResult(user, isSuper, token, outRefreshToken);
                }
            }
            catch (Exception ex)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }
          
            return Tools.ConvertToJSON(_responseMessage);
        }

        private async Task<ResponseMessage> LoginResult(ApplicationUser user, bool isSuper, string token, string refreshToken)
        {
            ResponseMessage obj = new ResponseMessage();
            if (user != null && !await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetAuthenticationTokenAsync(user, Messages.COMPANYNAME, Messages.REFRESHTOKEN, refreshToken);
                obj.Status = Status.MessageType.SUCCESS.ToString();
                obj.Result = token;
            }
            else if (isSuper)
            {
                obj.Status = Status.MessageType.SUCCESS.ToString();
                obj.Result = token;
            }
            else
            {
                obj.Status = Status.MessageType.FAILED.ToString();
                obj.Result = Messages.LOGININFO;
            }

            return obj;
        }

        [HttpPost("[action]")]
        public async Task<string> Logout([FromBody] JwtTokenResponse token)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null && token.UserId == _setting.GetSuperId())
            {
                _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
            }
            else
            {
                var currentRefresh = await _userManager.GetAuthenticationTokenAsync(user, Messages.COMPANYNAME, Messages.REFRESHTOKEN);

                if (token.Refresh == currentRefresh)
                {
                    var deletedToken = await _userManager.RemoveAuthenticationTokenAsync(user, Messages.COMPANYNAME, Messages.REFRESHTOKEN);

                    if (deletedToken.Succeeded)
                    {
                        _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                    }
                }
                else
                {
                    _responseMessage.Status = Status.MessageType.FAILED.ToString();
                    _responseMessage.Result = Messages.UNAUTHORIZED;
                }
            }

         
            return Tools.ConvertToJSON(_responseMessage);
        }

        //private void CallADAPI(string id, string password)
        //{

        //    System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://ngc-devvm1:8086/api/login/login");
        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    var model = new
        //    {
        //        AppId = "Rr0eExUJ0zlvXr02",
        //        UserId = id,
        //        Password = password
        //    };

        //    using (var sw = new StreamWriter(request.GetRequestStream()))
        //    {
        //        string json = JsonConvert.SerializeObject(model);
        //        sw.Write(json);
        //        sw.Flush();
        //    }
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //}
    }
   
}