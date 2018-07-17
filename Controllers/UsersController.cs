using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Models.ViewModel.User;
using ReactSupply.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    //[Authorize(Roles ="Administrator")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _rolesManager;
        private readonly ILogger<UsersController> _logger;
        private TableFormatter _tableFormatter;
        private readonly SettingLogic _setting;
        private readonly HistoryLogic _history;

        public UsersController(SupplyChainContext context, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<ApplicationRole> roleManager,
             ILogger<UsersController> logger) 
            :base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _rolesManager = roleManager;
            _logger = logger;
            _setting = new SettingLogic(context);
            _history = new HistoryLogic(context);
        }

        [HttpPost("[action]")]
        public async Task<string> Register([FromBody]RegisterViewModel modal)
        {
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = modal.UserName,
                    Email = modal.Email,
                    EmailConfirmed = true
                };

                var failed = UserRoleCheck(user).Result;

                if (failed != "")
                {
                    return failed;
                }

                var result = await _userManager.CreateAsync(user, "DefaultPassword1234!");
         
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, modal.Role);
                    _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                    _responseMessage.Result = Messages.SAVESUCCESS;
                    await _history.LogHistory(user.UserName, "New ID", user.UserName, User.Identity.Name, Status.Method.Create);
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

        [HttpGet("[action]")]
        public string GetUsers()
        {
            _tableFormatter = new TableFormatter();

            var superId = _setting.GetSuperId();
            try
            {
                var users = _userManager.Users
                    .Where(x=> x.UserName != superId)
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .OrderBy(x => x.UserName)
                    .Select(x => new UserViewModel
                    {
                        UserName = x.UserName,
                        Email = x.Email,
                        Role = x.UserRoles.FirstOrDefault().Role.Name,
                        LockupEnd = x.LockoutEnd,
                        Locked = (x.LockoutEnd == null)? false: true 
                    })
                    .ToList();

                UserLogic obj = new UserLogic(_context);
       
                var header = obj.SelectSchemaHeaderSync();
                
                _tableFormatter.Header = header;
                _tableFormatter.Body = Tools.ConvertToJSON(users);

            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }


            return Tools.ConvertToJSON(_tableFormatter);
        }


        [HttpPost("[action]")]
        public async Task<string> PostRoles([FromBody]RequestData modal)
        {
            try
            {
                var obj = JObject.Parse(modal.updated);
                var oName = "";
                var oValue = "";

                foreach (var property in obj.Properties())
                {
                    oName = property.Name;
                    oValue = property.Value.ToString();
                }

               var user = await _userManager.FindByNameAsync(modal.identifier);

                var failed = UserRoleCheck(user).Result;

                if (failed != "")
                {
                    return failed;
                }

                if (oName == "Role")
                {
                    var roles = _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.Result.ToArray());
                    await _userManager.AddToRoleAsync(user, oValue);
                    await _history.LogHistory(user.UserName, oName, oValue, User.Identity.Name, Status.Method.Update);
                }
                else
                {
                    if (oValue == "false")
                    {
                        await _userManager.SetLockoutEndDateAsync(user, null);
                    }
                    else
                    {
                        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                    }

                    await _history.LogHistory(user.UserName, "Locked", oValue, User.Identity.Name, Status.Method.Update);
                }

                _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                _responseMessage.Result = Messages.SAVESUCCESS;
            }
            catch (Exception ex)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }


        private async Task<string> UserRoleCheck(ApplicationUser user)
        {
            var superUser = _setting.GetSuperId();
            var isAdmin = User.IsInRole("Administrator");

          
            if (!isAdmin && superUser != User.Identity.Name || user.UserName == superUser)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = Messages.INVALIDACCES;

                return Tools.ConvertToJSON(_responseMessage);
            }

            if (await _userManager.IsInRoleAsync(user, "Administrator") && isAdmin)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = Messages.SAMELEVELADD;
                return Tools.ConvertToJSON(_responseMessage);
            }

            return "";
        }

    }


}