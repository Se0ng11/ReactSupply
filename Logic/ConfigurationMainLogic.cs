using Microsoft.EntityFrameworkCore;
using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class ConfigurationMainLogic : BaseLogic, IConfig 
    {
        private readonly SupplyChainContext _context;
        public ConfigurationMainLogic(SupplyChainContext context)
        {
            _context = context;
            
        }

        public async Task<string> SelectAll()
        {
            List<ConfigurationMain> lst = new List<ConfigurationMain>();

            try
            {
                using (var db = _context)
                {
                    lst = await db.ConfigurationMain
                        .OrderBy(x => x.TabGroup)
                        .ThenBy(x => x.Position)
                        .ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public async Task<string> SelectSimpleVisibleOnly()
        {
            List<ConfigurationMainDTO> lst = new List<ConfigurationMainDTO>();

            try
            {
                using (var db = _context)
                {
                    lst = await db.ConfigurationMain
                        .Where(x => x.IsDisplay == true)
                        .OrderBy(x => x.TabGroup)
                        .ThenBy(x => x.Position)
                        .Select(x => new ConfigurationMainDTO
                        {
                            DisplayName = x.DisplayName,
                            ControlType = x.ControlType,
                            Position = x.Position,

                            TabGroup = x.TabGroup,
                            Css = x.Css
                        })
                        .ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public async Task<string> SelectVisibleOnly()
        {
            List<ConfigurationMain> lst = new List<ConfigurationMain>();

            try
            {
                using (var db = _context)
                {
                    lst = await db.ConfigurationMain
                        .Where(x => x.IsDisplay == true)
                        .OrderBy(x => x.TabGroup)
                        .ThenBy(x => x.Position)
                        .ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ConvertToJSON(lst);
        }

        public async Task<List<ConfigurationMainDTO>> SelectObjectSimpleVisible()
        {
            List<ConfigurationMainDTO> lst = new List<ConfigurationMainDTO>();

            try
            {
                using (var db = _context)
                {
                    lst = await db.ConfigurationMain
                        .Where(x => x.IsDisplay == true)
                        .OrderBy(x => x.TabGroup)
                        .ThenBy(x => x.Position)
                        .Select(x => new ConfigurationMainDTO
                        {
                            DisplayName = x.DisplayName,
                            ControlType = x.ControlType,
                            Position = x.Position,

                            TabGroup = x.TabGroup,
                            Css = x.Css
                        })
                        .ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst;
        }
    }
}
