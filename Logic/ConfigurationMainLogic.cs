using Microsoft.EntityFrameworkCore;
using ReactSupply.Utils;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
//using System.Transactions;

namespace ReactSupply.Logic
{
    public class ConfigurationMainLogic : BaseLogic, IConfig, IConfigurationMain
    {
        public ConfigurationMainLogic(SupplyChainContext context)
            :base(context)
        {

        }

        public async Task<Status.MessageType> PostDoubleKeyFieldAsync(string identifier, string identifier1, string updated, string user)
        {
            try
            {
                var obj = JObject.Parse(updated);
                var oName = "";
                var oValue = "";

                foreach (var property in obj.Properties())
                {
                    oName = property.Name;
                    oValue = property.Value.ToString();
                }
                var moduleId = Convert.ToInt32(identifier);

                var entity = await _context.ConfigurationMain
                                .FirstOrDefaultAsync(x => x.ModuleId == moduleId && x.ValueName == identifier1)
                                .ConfigureAwait(false);

                if (entity != null)
                {
                    var shadow = _context.Entry(entity).Property(oName);
                    if (oValue == "")
                        oValue = null;

                    if (shadow.CurrentValue != null)
                    {
                        if (shadow.CurrentValue.GetType().Name == "Boolean")
                        {
                            shadow.CurrentValue = Convert.ToBoolean(oValue);
                        }
                        else if (shadow.CurrentValue.GetType().Name == "Decimal")
                        {
                            shadow.CurrentValue = Convert.ToDecimal(oValue);
                        }
                        else if (shadow.CurrentValue.GetType().Name == "Integer")
                        {
                            shadow.CurrentValue = Convert.ToInt32(oValue);
                        }
                        else
                        {
                            shadow.CurrentValue = oValue;
                        }
                    }
                    else
                    {
                        shadow.CurrentValue = oValue;
                    }

                    _context.ConfigurationMain.Update(entity);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    await new HistoryLogic(_context).LogHistory(identifier, oName, oValue, user, Status.Method.Update);
                    return Status.MessageType.SUCCESS;


                }
                else
                {
                    //do add here
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            return Status.MessageType.FAILED;
        }

        public async Task<string> SelectAllDataAsync()
        {
            List<ConfigurationMain> lst = new List<ConfigurationMain>();

            try
            {
                //using (var scope = new TransactionScope(
                //     TransactionScopeOption.Required,
                //     new TransactionOptions
                //     {
                //         IsolationLevel = IsolationLevel.ReadUncommitted
                //     }))
                //{
                    lst = await _context.ConfigurationMain
                       .OrderBy(x => x.Id)
                       .AsNoTracking()
                       .ToListAsync()
                       .ConfigureAwait(false);

                //    scope.Complete();
                //}
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);

            }
            return Tools.ConvertToJSON(lst);
        }

        public async Task<string> SelectHeader(int menu, bool isGuest)
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();
            try
            {
                lst = await _context.ConfigurationMain
                    .Where(x => x.IsVisible == true && x.ModuleId == menu)
                    .OrderBy(x => x.Position)
                    .Select(x => new ReactDataFormatter
                    {
                        key = x.ValueName,
                        name = x.DisplayName,
                        Group = x.Group,
                        width = Convert.ToInt32(x.Width),
                        locked = x.IsLocked,
                        sortable = x.IsSortable,
                        editable = (isGuest)? false: x.IsEditable,
                        filterable = x.IsFilterable,
                        resizable = x.IsResizeable,
                        inlineField = x.IsInlineField,
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

                var smallWidth = new string[] { "ModuleId", "Position", "MinLength",
                    "MaxLength", "Width", "IsLocked", "IsFilterable", "IsEditable", "IsVisible",
                    "IsRequired", "IsResizeable", "IsSortable", "IsInlineField", "ControlType" };
                lst = entityType
                        .Where(x=> x.Name != "Id")
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.Name,
                            name = x.Name,
                            width = (smallWidth.Contains(x.Name) ? 100 : 200),
                            locked = (x.Name == "ValueName" || x.Name == "ModuleId" ? true: false),
                            sortable = true,
                            editable = (x.Name == "ValueName" || x.Name == "ModuleId" ? false : true),
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