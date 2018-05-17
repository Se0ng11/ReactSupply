﻿using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using System;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class SupplyRecordLogic: JSONFormatter, IConfig
    {
        private readonly SupplyChainContext _context;
        public SupplyRecordLogic(SupplyChainContext context)
        {
            _context = context;

        }

        public async Task<string> SelectAllData()
        {
            JSONResult lst = new JSONResult();

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
                throw ex;
            }

            return lst.JsonResult.Replace("\\\\", "");
        }

        public async Task<string> PostSingleField(string indentifier, string valueName, string data)
        {
            string StatusMessage = "Failed";

            try
            {
                var entity = await _context.SupplyRecord
                                .FirstOrDefaultAsync(x => x.ValueName == valueName && x.AxNumber == indentifier)
                                .ConfigureAwait(false);

                if (entity != null)
                {
                    entity.Data = data;
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = "";

                    _context.SupplyRecord.Update(entity);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    StatusMessage = "Success";

                }
                else
                {
                    //do add here
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return StatusMessage;
        }

        public Task<string> SelectSchemaHeader()
        {
            throw new NotImplementedException();
        }
    }
}
