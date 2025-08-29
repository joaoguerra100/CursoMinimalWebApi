using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MicroWebApi.DbContexts;
using MicroWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
builder.Services.AddDbContext<RangoDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//metodo novo de usar a detalhaçao de erros no modo de produçao
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminFromBrazil", policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("country", "Brazil"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuthRango",
        new()
        {
            Name = "Authorization",
            Description = "Token baseado em Autheticação e Autorização",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        }
    );
    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TokenAuthRango"
                }
            },
            new List<string>()
        }
    }
    );
});


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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();

app.Run();
