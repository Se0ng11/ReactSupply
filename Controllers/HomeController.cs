﻿using Microsoft.AspNetCore.Mvc;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ReactSupply.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        private readonly SupplyChainContext _configuration;

        public HomeController(SupplyChainContext configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public List<ReactDataFormatter> GetSimpleConfigurationMain()
        {
            var obj = new ConfigurationMainLogic(_configuration);
            var data = obj.TableFormatter();
            
            Task.WaitAny(data);

            return data.Result;
        }

        [HttpGet("[action]")]
        public string GetConfigurationMainJson()
        {
            var obj = new ConfigurationMainLogic(_configuration);
            var data = obj.TableFormatterJson();

            Task.WaitAny(data);

            return data.Result;
        }
    }
}