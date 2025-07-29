using Microsoft.EntityFrameworkCore;
using MicroWebApi.DbContexts;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
builder.Services.AddDbContext<RangoDbContext>(x => x.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
