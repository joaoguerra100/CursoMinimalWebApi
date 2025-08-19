using Microsoft.EntityFrameworkCore;
using MicroWebApi.DbContexts;
using MicroWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
builder.Services.AddDbContext<RangoDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();

app.Run();
