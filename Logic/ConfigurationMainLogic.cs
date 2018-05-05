﻿using Microsoft.EntityFrameworkCore;
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
                        .OrderBy(x => x.Group)
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
                        .Where(x => x.IsVisible == true)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Position)
                        .Select(x => new ConfigurationMainDTO
                        {
                            DisplayName = x.DisplayName,
                            ControlType = x.ControlType,
                            Position = x.Position,

                            Group = x.Group,
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
                        .Where(x => x.IsVisible == true)
                        .OrderBy(x => x.Group)
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
                        .Where(x => x.IsVisible == true)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Position)
                        .Select(x => new ConfigurationMainDTO
                        {
                            DisplayName = x.DisplayName,
                            ControlType = x.ControlType,
                            Position = x.Position,

                            Group = x.Group,
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



        public async Task<List<ReactDataFormatter>> TableFormatter()
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {
                using (var db = _context)
                {
                    lst = await db.ConfigurationMain
                        .Where(x => x.IsVisible == true)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Position)
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.ValueName,
                            name = x.DisplayName,
                            width = Convert.ToInt32(x.Width),
                            locked = x.IsLocked,
                            sortable = x.IsSortable,
                            editable = x.IsEditable,
                            filterable = x.IsFilterable,
                            resizable = x.IsResizeable,
                            cellClass = x.Css,
                            formatter = x.Formatter,
                            headerRenderer = x.HeaderRenderer,
                            editor = x.Editor,
                            filterRenderer = x.FilterRenderer

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

        public async Task<string> TableFormatterJson()
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {
                using (var db = _context)
                {
                    lst = await db.ConfigurationMain
                        .Where(x => x.IsVisible == true)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Position)
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.ValueName,
                            name = x.DisplayName,
                            width = Convert.ToInt32(x.Width),
                            locked = x.IsLocked,
                            sortable = x.IsSortable,
                            editable = x.IsEditable,
                            filterable = x.IsFilterable,
                            resizable = x.IsResizeable,
                            cellClass = x.Css,
                            formatter = x.Formatter,
                            headerRenderer = x.HeaderRenderer,
                            editor = x.Editor,
                            filterRenderer = x.FilterRenderer

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
    }
}