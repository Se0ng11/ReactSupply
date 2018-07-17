using ReactSupply.Utils;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface IHistory
    {
        Task<string> LogHistory(string identifier, string field, string data, string user, Status.Method method);
        Task<string> SelectSpecificdata(string identifier);
    }
}
