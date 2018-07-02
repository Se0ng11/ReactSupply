using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Bundles;
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
        private readonly SettingLogic _setting;

        public AuthController(SupplyChainContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<HistoryController> logger)
            :base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _setting = new SettingLogic(_context);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> LoginAsync([FromBody]LoginViewModel model)
        {
            try
            {
                bool isSuper = model.UserId == _setting.GetSuperId();
                ApplicationUser user = await _userManager.FindByNameAsync(model.UserId);

                if (user != null || isSuper)
                {
                    var token = Tools.GenerateJwtToken(_setting, model.UserId, "", isSuper, out string outRefreshToken);

                    if (user != null && !await _userManager.IsLockedOutAsync(user))
                    { 
                        await _userManager.SetAuthenticationTokenAsync(user, Messages.COMPANYNAME, Messages.REFRESHTOKEN, outRefreshToken);
                   
                    }

                    _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                    _responseMessage.Result = token;
                }
                else
                {
                    _responseMessage.Status = Status.MessageType.FAILED.ToString();
                    _responseMessage.Result = Messages.LOGININFO;
                }

            }
            catch (Exception ex)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }
          
            return Tools.ConvertToJSON(_responseMessage);
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