using System.Net;
using Microsoft.EntityFrameworkCore;
using MicroWebApi.DbContexts;
using MicroWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
builder.Services.AddDbContext<RangoDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//metodo novo de usar a detalhaçao de erros no modo de produçao
builder.Services.AddProblemDetails();

var app = builder.Build();

// para quando estiver em modo de produçao mostrar os erros menos detalhados
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    // metodo antigo de usar
    /* app.UseExceptionHandler(configureApplicationBuilder =>
    {
        configureApplicationBuilder.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("An unexpected problem happened.");
            });
    }); */
}

app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();

app.Run();
