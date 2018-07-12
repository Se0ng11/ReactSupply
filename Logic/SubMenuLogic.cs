using Microsoft.EntityFrameworkCore;
using ReactSupply.Utils;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class SubMenuLogic : BaseLogic, IConfig
    {
        public SubMenuLogic(SupplyChainContext context)
            :base(context)
        {

        }

        public async Task<string> SelectAllDataAsync()
        {
            await Task.Run(() => SelectAsList());

            return Tools.ConvertToJSON(SelectAsList().Result);
        }

        public string SelectVisibleData()
        {
            IEnumerable<SubMenu> lst = null;
            try
            {
                lst = SelectAsList().Result
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
            throw new NotImplementedException();
        }

        public async Task<List<SubMenu>> SelectAsList()
        {
            List<SubMenu> lst = new List<SubMenu>();

            try
            {
                lst = await _context.SubMenu
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
