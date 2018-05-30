using ReactSupply.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface IConfig
    {
        Task<string> SelectAllDataAsync();

        string SelectSchemaHeaderSync();

        Task<string> PostSingleFieldAsync(string indentifier, string valueName, string data);
    }
}
