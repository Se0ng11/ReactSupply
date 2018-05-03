using Microsoft.EntityFrameworkCore;

namespace ReactSupply.Models.DB
{
    public partial class SupplyChainContext : DbContext
    {
        public SupplyChainContext(DbContextOptions<SupplyChainContext> options): base(options)
        { }
    }
}
