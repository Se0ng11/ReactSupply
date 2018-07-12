using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System;
using System.Threading.Tasks;
using ReactSupply.Utils;

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

        public string SelectSchemaHeaderSync()
        {
            throw new NotImplementedException();
        }


        public async Task<Status.MessageType> PostDoubleKeyFieldAsync(string identifier, string identifier1, string updated, string user)
        {
            var obj = JObject.Parse(updated);
            var oName = "";
            var oValue = "";

            foreach (var property in obj.Properties())
            {
                oName = property.Name;
                oValue = property.Value.ToString();

                try
                {
                    var entity = await _context.SupplyRecord
                                    .FirstOrDefaultAsync(x => x.ValueName == oName && x.AxNumber == identifier)
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
                            AxNumber = identifier,
                            ValueName = oName,
                            Data = oValue,
                            Status = "Open",
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = user,
                            CreatedDate = DateTime.Now,
                            CreatedBy = user
                        };

                        _context.SupplyRecord.Add(entity);
                        await _context.SaveChangesAsync().ConfigureAwait(false);

                    }
                    await new HistoryLogic(_context).LogHistory(identifier, oName, oValue, user);
                }
                catch (Exception ex)
                {
                    _Logger.Error(ex);
                    return Status.MessageType.FAILED;
                }
            }

            return Status.MessageType.SUCCESS;

        }
    }
}
