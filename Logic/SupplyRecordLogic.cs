using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Threading.Tasks;
using ReactSupply.Utils;
using System.Linq;
using System.Collections.Generic;

namespace ReactSupply.Logic
{
    public class SupplyRecordLogic: BaseLogic, IConfig, ISupplyRecord
    {
        public SupplyRecordLogic(SupplyChainContext context)
            :base(context)
        {
        }

        public async Task<string> SelectAllDataAsync()
        {
            ResultJson lst = new ResultJson();

            try
            {
                lst = await _context.JSONResult
                        .FromSql("EXECUTE GET_Supply_Record")
                        .AsNoTracking()
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
            if (lst.JsonResult == null)
            {
                return "";
            }
            return lst.JsonResult.Replace("\\\\", "");
        }

        public async Task<string> SelectMenuData(string menu, string updated)
        {
            ResultJson rj = new ResultJson();
            string query = "EXECUTE GET_Supply_Record @MENU={0}, @STARTDATE={1}, @ENDDATE={2}, @STATUS={3}";
            var vP = new List<ValuePair>();

            try
            {
                if (updated != null)
                {
                    var obj = JObject.Parse(updated);

                    vP = obj.Properties().Select(x => new ValuePair
                    {
                        Name = x.Name,
                        Value = x.Value.ToString()
                    }).ToList();


                    var startDate = Convert.ToDateTime(SingleValuePair(vP, "startDate"));
                    var endDate =Convert.ToDateTime(SingleValuePair(vP, "endDate")); 
                    var status = SingleValuePair(vP, "status"); 
                   
                    if (status == "")
                        status = null;


                    rj = await _context.JSONResult
                       .FromSql(query, Convert.ToInt32(menu), startDate, endDate, status)
                       .AsNoTracking()
                       .SingleOrDefaultAsync()
                       .ConfigureAwait(false);
                }
                else
                {
                    rj = await _context.JSONResult
                       .FromSql(query, Convert.ToInt32(menu), null, null, null)
                       .AsNoTracking()
                       .SingleOrDefaultAsync()
                       .ConfigureAwait(false);
                }

               
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
            if (rj.JsonResult == null)
            {
                return "";
            }
            return rj.JsonResult.Replace("\\\\", "");
        }

        public string SelectSchemaHeaderSync()
        {
            throw new NotImplementedException();
        }

        public async Task<Status.MessageType> PostDoubleKeyFieldAsync(string identifier, string identifier1, string updated, string user)
        {
            var obj = JObject.Parse(updated);
            var oName = "";
            var oValue = "";

            var temp = identifier.Split('|');

            foreach(var id in temp)
            {

                foreach (var property in obj.Properties())
                {
                    oName = property.Name;
                    oValue = property.Value.ToString();

                    try
                    {
                        var entity = await _context.SupplyRecord
                                        .FirstOrDefaultAsync(x => x.ValueName == oName && x.Identifier == id)
                                        .ConfigureAwait(false);

                        if (entity != null)
                        {
                            entity.Data = oValue;
                            entity.ModifiedDate = DateTime.Now;
                            entity.ModifiedBy = user;

                            _context.SupplyRecord.Update(entity);
                            await _context.SaveChangesAsync().ConfigureAwait(false);
                        }
                        else
                        {
                            entity = new SupplyRecord
                            {
                                Identifier = id,
                                ValueName = oName,
                                Data = oValue,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = user,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user
                            };

                            _context.SupplyRecord.Add(entity);
                            await _context.SaveChangesAsync().ConfigureAwait(false);

                        }
                        await new HistoryLogic(_context).LogHistory(id, oName, oValue, user, Status.Method.Update);
                    }
                    catch (Exception ex)
                    {
                        _Logger.Error(ex);
                        return Status.MessageType.FAILED;
                    }
                }
            }

            return Status.MessageType.SUCCESS;

        }
    }
}
