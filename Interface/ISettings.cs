using ReactSupply.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface ISettings
    {
        string GetSuperId();
        string GetSettingValueInString(string code);

        Setting GetSettingValue(string code);
    }
}
