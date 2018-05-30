using Microsoft.EntityFrameworkCore;
using ReactSupply.Models.Entity;

namespace ReactSupply.Models.DB
{
    public partial class SupplyChainContext : DbContext
    {
        public virtual DbSet<JSONResult> JSONResult { get; set; }

        public SupplyChainContext(DbContextOptions<SupplyChainContext> options): base(options)
        { }
    }
}
