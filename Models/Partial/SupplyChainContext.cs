using Microsoft.EntityFrameworkCore;
using ReactSupply.Models.Entity;

namespace ReactSupply.Models.DB
{
    public partial class SupplyChainContext : DbContext
    {
        public virtual DbSet<ResultJson> JSONResult { get; set; }

        public SupplyChainContext(DbContextOptions<SupplyChainContext> options): base(options)
        { }
    }
}
