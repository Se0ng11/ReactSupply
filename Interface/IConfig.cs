using ReactSupply.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface IConfig
    {
        Task<string> SelectAll();
        Task<string> SelectVisibleOnly();
        Task<string> SelectSimpleVisibleOnly();
        Task<List<ReactDataFormatter>> TableFormatter();
    }
}
