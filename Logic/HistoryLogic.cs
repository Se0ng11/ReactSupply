using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactSupply.Utils;
using Newtonsoft.Json.Linq;

namespace ReactSupply.Logic
{
    public class HistoryLogic : BaseLogic, IConfig, IHistory
    {
        public HistoryLogic(SupplyChainContext context)
            :base(context)
        {

        }

        public async Task<string> LogHistory(string identifier, string field, string data, string user)
        {
            string message = "Update field {0} to {1}";

            try
            {
                History history = new History
                {
                    Identifier = identifier,
                    Message = string.Format(message, field, data),
                    CreatedDate = DateTime.Now,
                    CreatedBy = user
                };

                _context.Add(history);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                _Logger.Error(ex);
            }

            return "";
        }

        public async Task<Status.MessageType> PostDoubleKeyFieldAsync(string indentifier, string identifier1, string updated, string user)
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

                await LogHistory(identifier1, oName, oValue, user);
                return Status.MessageType.SUCCESS;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            return Status.MessageType.FAILED;
        }
        public async Task<string> SelectAllDataAsync()
        {
            List<History> lst = new List<History>();
            try
            {
                lst = await _context.History
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

        public string SelectSchemaHeaderSync()
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {

                var entityType = typeof(History).GetProperties();

                lst = entityType
                        .Where(x => x.Name != "Id" && x.Name != "ModuleId" && x.Name !="Identifier")
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.Name,
                            name = x.Name,
                            width = (x.Name == "Message")? 450: 150,
                            locked = false,
                            sortable = true,
                            editable = false,
                            filterable = true,
                            resizable = true,
                            headerClass = (x.Name == "Message") ? "left-align" : "",
                            cellClass = (x.Name == "Message") ? "left-align":"",
                            control = x.PropertyType.Name
                        }).ToList();

                lst.Insert(0, FixedIndexColumn());
            }

            catch (Exception ex)
            {
                _Logger.Error(ex);
                throw ex;
            }

            return Tools.ConvertToJSON(lst);
        }

        public async Task<String> SelectSpecificdata(string identifier)
        {
            List<History> lst = new List<History>();

            try
            {
                lst = await _context.History
                        .Where(x => x.Identifier == identifier)
                        .OrderByDescending(x=> x.CreatedDate)
                        .Select(x=> new History
                        {
                            Identifier=x.Identifier,
                            Message=x.Message,
                            CreatedBy=x.CreatedBy,
                            CreatedDate = x.CreatedDate
                        })
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
    }
}
