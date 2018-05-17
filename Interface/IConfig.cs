using ReactSupply.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface IConfig
    {
        Task<string> SelectAllData();

        Task<string> SelectSchemaHeader();

        Task<string> PostSingleField(string indentifier, string valueName, string data);
    }
}
