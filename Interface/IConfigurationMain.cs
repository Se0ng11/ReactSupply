using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    interface IConfigurationMain
    {
        Task<string> SelectHeader(int menu, bool isGuest);
    }
}
