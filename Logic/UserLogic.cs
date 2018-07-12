using ReactSupply.Interface;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using ReactSupply.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Logic
{
    public class UserLogic : BaseLogic, IConfig
    {
        public UserLogic(SupplyChainContext context) 
            : base(context)
        {
        }

        public Task<Status.MessageType> PostDoubleKeyFieldAsync(string indentifier, string identifier1, string updated, string user)
        {
            throw new NotImplementedException();
        }

        public Task<string> PostSingleKeyFieldAsync(string indentifier, string valueName, string data)
        {
            throw new NotImplementedException();
        }

        public Task<string> SelectAllDataAsync()
        {
            throw new NotImplementedException();
        }

        public string SelectSchemaHeaderSync()
        {
            List<ReactDataFormatter> lst = new List<ReactDataFormatter>();

            try
            {
                var entityType = typeof(ApplicationUser).GetProperties();

                var showCol = new string[] { "UserName", "Email" };
                lst = entityType
                        .Where(x=> showCol.Contains(x.Name))
                        .Select(x => new ReactDataFormatter
                        {
                            key = x.Name,
                            name = x.Name,
                            width = 400,
                            locked = (showCol.Contains(x.Name)) ? true : false,
                            sortable = true,
                            editable = (showCol.Contains(x.Name)) ? false : true,
                            filterable = true,
                            resizable = true,
                            control = x.PropertyType.Name
                        }).ToList();

                lst.Insert(0, FixedIndexColumn());

                var roleCol = new ReactDataFormatter
                {
                    key = "Role",
                    name = "Role",
                    width = 300,
                    locked = false,
                    sortable = true,
                    editable = true,
                    filterable = true,
                    resizable = true,
                    control = "role"
                };

                lst.Add(roleCol);

                var isLockedCol = new ReactDataFormatter
                {
                    key = "Locked",
                    name = "Locked",
                    width = 150,
                    locked = false,
                    sortable = true,
                    editable = true,
                    filterable = true,
                    resizable = true,
                    control = "boolean"
                };
                lst.Add(isLockedCol);
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
