using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ReactSupply.Bundles;

namespace ReactSupply.Logic
{
    public class MenuLogic : BaseLogic, IConfig
    {
        public MenuLogic(SupplyChainContext context)
            :base(context)
        {

        }

        public Task<string> PostSingleFieldAsync(string indentifier, string valueName, string data)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> SelectAllDataAsync()
        {
            await Task.Run(()=> SelectAsList());

            return Tools.ConvertToJSON(SelectAsList().Result);
        }

        public string SelectVisibleData()
        {
            IEnumerable<Menu> lst = null;
            try
            {
                lst = SelectAsList().Result
                        .Where(x => x.IsEnabled == true)
                        .OrderBy(x=> x.Position);
              
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

        public async Task<List<Menu>> SelectAsList()
        {
            List<Menu> lst = new List<Menu>();

            try
            {
                lst = await _context.Menu
                        .OrderBy(x=> x.Position)
                        .AsNoTracking()
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
    }
}
