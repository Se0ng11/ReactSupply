using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class HistoryLogic : BaseLogic, IConfig
    {
        public HistoryLogic(SupplyChainContext context) => _context = context;

        public async Task<string> PostSingleFieldAsync(string indentifier, string valueName, string data)
        {
            ResponseMessage rm = new ResponseMessage();

            try
            {
                //do add here
                History obj = new History
                {
                    Identifier = indentifier,
                    Message = "Update " + valueName + " to" + data,
                    CreatedDate = DateTime.Now,
                    CreatedBy = ""
                };
                _context.Add(obj);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                rm.Status = "Success";
            }
            catch (Exception ex)
            {
                rm.Status = "Failed";
                rm.Result = ex.Message;
            }

            return ConvertToJSON(rm);
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
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public string SelectSchemaHeaderSync()
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {

                var entityType = typeof(History).GetProperties();

                lst = entityType
                        .Where(x => x.Name != "Id" && x.Name != "ModuleId")
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.Name,
                            name = x.Name,
                            width = 200,
                            locked = false,
                            sortable = true,
                            editable = false,
                            filterable = true,
                            resizable = true,
                            control = x.PropertyType.Name
                        }).ToList();

                lst.Insert(0, FixedIndexColumn());
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public async Task<String> SelectSpecificdata(string identifier)
        {
            List<History> lst = new List<History>();

            try
            {
                lst = await _context.History
                        .Where(x => x.Identifier == identifier)
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
    }
}
