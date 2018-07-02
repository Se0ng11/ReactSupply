using Microsoft.EntityFrameworkCore;
using ReactSupply.Bundles;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class ConfigurationMainLogic : BaseLogic, IConfig 
    {
        public ConfigurationMainLogic(SupplyChainContext context)
            :base(context)
        {

        }

        public async Task<string> PostSingleFieldAsync(string indentifier, string valueName, string data)
        {
            ResponseMessage rm = new ResponseMessage();

            try
            {
                var entity = await _context.ConfigurationMain
                                .FirstOrDefaultAsync(x => x.ValueName == indentifier)
                                .ConfigureAwait(false);

                if (entity != null)
                {
                    var shadow = _context.Entry(entity).Property(valueName);
                    if (data == "")
                        data = null;

                    if (shadow.CurrentValue != null)
                    {
                        if (shadow.CurrentValue.GetType().Name == "Boolean")
                        {
                            shadow.CurrentValue = Convert.ToBoolean(data);
                        }
                        else if (shadow.CurrentValue.GetType().Name == "Decimal")
                        {
                            shadow.CurrentValue = Convert.ToDecimal(data);
                        }
                        else if (shadow.CurrentValue.GetType().Name == "Integer")
                        {
                            shadow.CurrentValue = Convert.ToInt32(data);
                        }
                        else
                        {
                            shadow.CurrentValue = data;
                        }
                    }
                    else
                    {
                        shadow.CurrentValue = data;
                    }

                    _context.ConfigurationMain.Update(entity);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    rm.Status = "Success";

                    
                }
                else
                {
                    //do add here
                }
            }
            catch (Exception ex)
            {
                rm.Status = "Failed";
                rm.Result = ex.Message;
                _Logger.Error(ex);
            }

            return Tools.ConvertToJSON(rm);
        }

        public async Task<string> SelectAllDataAsync()
        {
            List<ConfigurationMain> lst = new List<ConfigurationMain>();

            try
            {
                lst = await _context.ConfigurationMain
                        .OrderBy(x => x.Id)
                        .AsNoTracking()
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

                lst.Insert(0, FixedIndexColumn());
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
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {

                var entityType = typeof(ConfigurationMain).GetProperties();
            
                lst = entityType
                        .Where(x=> x.Name != "Id" && x.Name !="ModuleId")
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.Name,
                            name = x.Name,
                            width = 200,
                            locked = (x.Name == "ValueName" ? true: false),
                            sortable = true,
                            editable = (x.Name == "ValueName" ? false : true),
                            filterable = true,
                            resizable = true,
                            control = (x.Name =="ControlType" ? "DropDown" : x.PropertyType.Name)
                        }).ToList();

                lst.Insert(0, FixedIndexColumn());
            }

            catch (Exception ex)
            {
                _Logger.Error(ex);
                throw ex;
            }

            return Tools.ConvertToJSON(lst);
            //throw new NotImplementedException();
        }
    }

}