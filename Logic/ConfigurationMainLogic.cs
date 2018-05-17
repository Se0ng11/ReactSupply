using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class ConfigurationMainLogic : JSONFormatter, IConfig 
    {
        private readonly SupplyChainContext _context;

        public ConfigurationMainLogic(SupplyChainContext context)
        {
            _context = context;
            
        }

        public Task<string> PostSingleField(string indentifier, string valueName, string data)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SelectAllData()
        {
            List<Models.DB.ConfigurationMain> lst = new List<Models.DB.ConfigurationMain>();

            try
            {
                lst = await _context.ConfigurationMain
                        .OrderBy(x => x.Position)
                        .AsNoTracking()
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public async Task<string> SelectHeader()
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {
                    lst = await _context.ConfigurationMain
                        .Where(x => x.IsVisible == true)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Position)
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.ValueName,
                            name = x.DisplayName,
                            title = x.Group,
                            width = Convert.ToInt32(x.Width),
                            locked = x.IsLocked,
                            sortable = x.IsSortable,
                            editable = x.IsEditable,
                            filterable = x.IsFilterable,
                            resizable = x.IsResizeable,
                            headerClass = x.HeaderCss,
                            cellClass = x.BodyCss,
                            control = x.ControlType,
                            formatter = x.Formatter,
                            headerRenderer = x.HeaderRenderer,
                            editor = x.Editor,
                            filterRenderer = x.FilterRenderer

                        })
                        .AsNoTracking()
                        .ToListAsync()
                        .ConfigureAwait(false);
                }

            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public Task<string> SelectSchemaHeader()
        {
            //string lst = "";

            //try
            //{
            //    lst = _context.Model
            //            .FindEntityType(typeof(ConfigurationMain))
            //            .Relational()
            //            .Schema;
            //}

            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return ConvertToJSON(lst);
            throw new NotImplementedException();
        }
    }
}