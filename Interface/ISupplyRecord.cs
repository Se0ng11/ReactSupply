﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Interface
{
    public interface ISupplyRecord
    {
        Task<string> SelectMenuData(string menu, string updated);
    }
}
