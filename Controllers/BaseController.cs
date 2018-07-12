using Microsoft.AspNetCore.Mvc;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Utils;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly SupplyChainContext _context;
        protected ResponseMessage _responseMessage = new ResponseMessage();
        public BaseController(SupplyChainContext context)
        {
            _context = context;
        }

        public Status.MessageType PostData([FromBody]RequestData modal, IConfig config)
        {
            var result = config.PostDoubleKeyFieldAsync(modal.identifier, modal.identifier1, modal.updated, User.Identity.Name);

            Task.WaitAll(result);

            return result.Result;
        }
    }
}
