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

        public Task<Status.MessageType> PostDoubleKeyFieldAsync(string indentifier, string identifier1, string updated, string user)
        {
            throw new NotImplementedException();
        }
    }
}
