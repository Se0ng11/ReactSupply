using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Route("api/Roles")]
    public class RoleController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _rolesManager;
        private readonly ILogger<RoleController> _logger;

        public RoleController(SupplyChainContext context, 
            RoleManager<ApplicationRole> roleManager,
            ILogger<RoleController> logger)
            : base(context)
        {
            _rolesManager = roleManager;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public string GetRoles()
        {
            try
            {
                var roles = _rolesManager.Roles
                    .OrderBy(x=> x.Name)
                    .Select(x=> x.Name)
                    .ToList();

                _responseMessage.Status = Status.MessageType.SUCCESS.ToString();
                _responseMessage.Data = Tools.ConvertToJSON(roles);
            }
            catch (Exception ex)
            {
                _responseMessage.Status = Status.MessageType.FAILED.ToString();
                _responseMessage.Result = ex.Message;
            }

            return Tools.ConvertToJSON(_responseMessage);
        }


        [HttpPost("[action]")]
        public async Task<string> Register([FromBody] RoleViewModel model)
        {
            try
            {
                ApplicationRole role = new ApplicationRole
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