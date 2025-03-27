using Argus.Example.Data;
using Argus.Example.Models.Entity;
using Argus.Sync.Reducers;
using Chrysalis.Cardano.Core.Extensions;
using Chrysalis.Cardano.Core.Types.Block;
using Chrysalis.Cardano.Core.Types.Block.Transaction.Body;
using Microsoft.EntityFrameworkCore;

namespace Argus.Example.Reducers;

public class TxOutputBySlotReducer(
    IDbContextFactory<ArgusDbContext> dbContextFactory
) : IReducer<TxOutputBySlot>
{
    public async Task RollBackwardAsync(ulong slot)
    {   
        await using ArgusDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        IQueryable<TxOutputBySlot> toRollBack = dbContext.TxOutputBySlot
            .Where(e => e.Slot >= slot);

        dbContext.RemoveRange(toRollBack);
        await dbContext.SaveChangesAsync();
    }

    public async Task RollForwardAsync(Block block)
    {
        await using ArgusDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        IEnumerable<TransactionBody> txBodies = block.TransactionBodies();
        ulong slot = block.Slot() ?? 0;

        ulong index = 0;
        txBodies.ToList().ForEach(tx => 
        {
            string txHash = tx.Id();
            TxOutputBySlot newEntry = new(
                txHash,
                index,
                slot,
                tx.Raw ?? []
            );
            index++;
            dbContext.TxOutputBySlot.Add(newEntry);
        });
        await dbContext.SaveChangesAsync();
    }
}
