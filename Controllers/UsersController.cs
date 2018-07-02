using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Models.ViewModel.User;
using System;
using System.Threading.Tasks;
using ReactSupply.Bundles;

namespace ReactSupply.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ResponseMessage _responseMessage = new ResponseMessage();
        private readonly RoleManager<IdentityRole> _rolesManager;

        public UsersController(SupplyChainContext context, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            ILogger<HistoryController> logger) 
            :base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rolesManager = roleManager;
        }

        [HttpPost("[action]")]
        public async Task<string> Register([FromBody]RegisterViewModel model)
        {
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserId,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword1234!");
       
                if (result.Succeeded)
                {
                    _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                    _responseMessage.Result = Messages.SAVESUCCESS;
                }
                else
                {
                    string message = "";
                    foreach (var error in result.Errors)
                    {
                        message += error.Description;
                    }
                    _responseMessage.Status = Status.MessageType.FAILED.ToString();
                    _responseMessage.Result = message;

                }

            }
            catch (Exception ex)
            {
                _responseMessage.Status = Bundles.Status.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }

        [HttpGet("[action]")]
        public string GetUsers()
        {
            var users = _userManager.Users;
            _responseMessage.Status = Status.MessageType.SUCCESS.ToString();

            //var result = new TableFormatter { Header = data, Body = test };

            return Tools.ConvertToJSON(_responseMessage);
        }

        //[HttpGet("[action]")]
        //public async Task<JsonResult> GetRoles([FromBody] RoleViewModel model)
        //{
        //    try
        //    {
        //        await _rolesManager.Roles.ToList();

        //        _responseMessage.Status = Static.Response.MessageType.SUCCESS.ToString();
        //    }
        //    catch(Exception ex)
        //    {
        //        _responseMessage.Status = Static.Response.MessageType.FAILED.ToString();
        //        _responseMessage.Result = ex.Message;
        //    }

        //    return FormatJSON(_responseMessage);
        //}


        [HttpPost("[action]")]
        public async Task<string> PostRole([FromBody] RoleViewModel model)
        {
            try
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.Name
                };

                var result = await _rolesManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                    _responseMessage.Result = Messages.SAVESUCCESS;
                }
                else
                {
                    string message = "";
                    foreach (var error in result.Errors)
                    {
                        message += error.Description;
                    }
                    _responseMessage.Status = Status.MessageType.FAILED.ToString();
                    _responseMessage.Result = message;
                }
            }
            catch (Exception ex)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }

    }


}