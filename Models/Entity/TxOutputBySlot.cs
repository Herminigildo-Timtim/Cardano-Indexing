
using Argus.Sync.Data.Models;

namespace Argus.Example.Models.Entity;

public record TxOutputBySlot(
    string TxHash,
    ulong Index,
    ulong Slot,
    byte[] RawData
) : IReducerModel;
