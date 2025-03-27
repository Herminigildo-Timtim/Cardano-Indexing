
using Argus.Example.Data;
using Argus.Example.Models.Entity;
using Argus.Example.Reducers;
using Argus.Sync.Data.Models;
using Argus.Sync.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCardanoIndexer<ArgusDbContext>(builder.Configuration);
builder.Services.AddReducers<ArgusDbContext, IReducerModel>([
    typeof(TxOutputBySlotReducer)
]);

var app = builder.Build();

app.Run();