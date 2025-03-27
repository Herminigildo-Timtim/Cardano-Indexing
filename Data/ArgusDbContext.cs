
using Argus.Example.Models.Entity;
using Argus.Sync.Data;
using Microsoft.EntityFrameworkCore;

namespace Argus.Example.Data;

public class ArgusDbContext(
    DbContextOptions<ArgusDbContext> options,
    IConfiguration configuration
) : CardanoDbContext(options, configuration)
{
    public DbSet<TxOutputBySlot> TxOutputBySlot => Set<TxOutputBySlot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TxOutputBySlot>(entity =>
        {
            entity.HasKey(e => new { e.TxHash, e.Index, e.Slot });
        });
    }
}