using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactSupply.Models.Entity
{
    public class JwtTokenResponse
    {
        public string Token { get; set; }
        public string Refresh { get; set; }

        public string UserId { get; set; }

    }
}
