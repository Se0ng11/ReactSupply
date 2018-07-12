using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class SettingLogic : BaseLogic, IConfig, ISettings
    {
        private List<Setting> _settings = new List<Setting>();

        public SettingLogic(SupplyChainContext context)
            :base(context)
        {
            _settings = SelectSetting();
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

        public string GetSettingValueInString(string code)
        {
            string value = "";
            try
            {
                value = _settings.Where(x => x.Code == code).Select(x => x.Value).SingleOrDefault();
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

        public Task<Status.MessageType> PostDoubleKeyFieldAsync(string indentifier, string identifier1, string updated, string user)
        {
            throw new NotImplementedException();
        }

        public Setting GetSettingValue(string code)
        {
            Setting obj = new Setting();
            try
            {
                obj = _settings.Where(x => x.Code == code)
                        .SingleOrDefault();
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
            }

            return obj;
        }
    }
}
