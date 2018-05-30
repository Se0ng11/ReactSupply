using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ReactSupply.Logic
{
    public class MenuLogic : BaseLogic, IConfig
    {
        public MenuLogic(SupplyChainContext context) => _context = context;

        public Task<string> PostSingleFieldAsync(string indentifier, string valueName, string data)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> SelectAllDataAsync()
        {
            await Task.Run(()=> SelectAsList());

            return ConvertToJSON(SelectAsList().Result);
        }

        public string SelectVisibleData()
        {
            IEnumerable<Menu> lst = null;
            try
            {
                lst = SelectAsList().Result
                        .Where(x => x.IsEnabled = true);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
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
                        .AsNoTracking()
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
    }
}
