using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ReactSupply.Utils;

namespace ReactSupply.Logic
{
    public class MenuLogic : BaseLogic, IConfig
    {
        public MenuLogic(SupplyChainContext context)
            :base(context)
        {

        }

        public async Task<string> SelectAllDataAsync()
        {
            string role = "";
            await Task.Run(() => SelectAsList(role));

            return Tools.ConvertToJSON(SelectAsList(role).Result);
        }

        public async Task<string> SelectVisibleData()
        {
            IEnumerable<Menu> lst = null;
            try
            {
                lst = await _context.Menu
                        .AsNoTracking()
                        .Where(x => x.IsEnabled == true)
                        .OrderBy(x => x.Position)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                throw ex;
            }

            return Tools.ConvertToJSON(lst);
        }


        public string SelectVisibleData(string role)
        {
            IEnumerable<Menu> lst = null;
            try
            {
                lst = SelectAsList(role).Result
                        .Where(x => x.IsEnabled == true);
              
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                throw ex;
            }

            return Tools.ConvertToJSON(lst);
        }

        public string SelectSchemaHeaderSync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Menu>> SelectAsList(string role)
        {
            List<Menu> lst = new List<Menu>();

            try
            {
                lst = await _context.Menu
                        .Join(_context.RolesMenu, 
                            menu => menu.MenuCode,
                            rMenu => rMenu.MenuId,
                            (menu, rMenu) => new { menu, rMenu })
                        .AsNoTracking()
                        .Where(x=>
                            x.rMenu.RolesId == role
                        )
                        .OrderBy(x=> x.menu.Position)
                        .Select(x=> x.menu)
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                throw ex;
            }
            return lst;
        }

        public Task<Status.MessageType> PostDoubleKeyFieldAsync(string indentifier, string identifier1, string updated, string user)
        {
            throw new NotImplementedException();
        }
    }
}
