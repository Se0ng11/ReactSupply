using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class SettingLogic : BaseLogic, IConfig
    {
        private List<Setting> _settings = new List<Setting>();

        public SettingLogic(SupplyChainContext context)
            :base(context)
        {
            _settings = SelectSetting();
        }

        public Task<string> PostSingleFieldAsync(string indentifier, string valueName, string data)
        {
            throw new NotImplementedException();
        }

        public Task<string> SelectAllDataAsync()
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
            throw new NotImplementedException();
        }

        public string SelectSchemaHeaderSync()
        {
            throw new NotImplementedException();
        }

        public string GetSuperId()
        {
            string value = "";
            try
            { 
                string query = _settings.Where(x => x.Code == "AllowSuper").Select(x => x.Value).SingleOrDefault();

                if (query.ToLower() == "true")
                {
                    value = _settings.Where(x => x.Code == "SuperId").Select(x => x.Value).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            return value.ToLower();
        }

        public string GetAccessExpireInMinute()
        {
            string value = "";
            try
            {
                value = _settings.Where(x => x.Code == "AccessExpire").Select(x => x.Value).SingleOrDefault();
            }
            catch(Exception ex)
            {
                _Logger.Error(ex);
            }
            return value;
        }

        public string GetRefreshExpireInMinute()
        {
            string value = "";
            try
            {
                value = _settings.Where(x => x.Code == "RefreshExpire").Select(x => x.Value).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }
            return value;
        }
            
        private List<Setting> SelectSetting()
        {
            List<Setting> lst = new List<Setting>();

            try
            {
                lst = _context.Setting
                        .AsNoTracking()
                        .ToList();
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            return lst;
        }

       
    }
}
