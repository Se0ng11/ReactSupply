using ReactSupply.Models.Entity;
using ReactSupply.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface IConfig
    {
        Task<string> SelectAllDataAsync();
        string SelectSchemaHeaderSync();
        Task<Status.MessageType> PostDoubleKeyFieldAsync(string identifier, string identifier1, string updated, string user);
    }
}
