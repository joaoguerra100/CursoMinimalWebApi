using MicroWebApi.EndpointFilters;
using MicroWebApi.EndpointHandlers;

namespace MicroWebApi.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/pratos/{pratoid:int}", (int pratoid) => $"O prato {pratoid} e delicioso")
                .WithOpenApi(operation =>
                {
                    operation.Deprecated = true;
                    return operation;
                })
                .WithSummary("Este endpoint esta deprecated e sera descontinuado na versao 2 desta API")
                .WithDescription("Por favor utilize a outra roda desta API sendo ela /rangos/{rangoID} para evitar maiores transtornos futuros");

            var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos")
                .RequireAuthorization();
            var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoID:int}");

            var rangosComIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoID:int}")
                .RequireAuthorization("RequireAdminFromBrazil")
                .RequireAuthorization()
                .AddEndpointFilter(new RangoIsLockedFilter(8))
                .AddEndpointFilter(new RangoIsLockedFilter(12));

            rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

            rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos")
                .AllowAnonymous();

            rangosEndpoints.MapPost("", RangosHandlers.CreateRangoAsync)
            .AddEndpointFilter<ValidateAnnotationFilter>();

            rangosComIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);

            rangosComIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();
        }

        public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ingredientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes")
                .RequireAuthorization();
            ingredientesEndpoints.MapGet("", IngredientesHandlers.GetIngredientesAsync);
        }
    }
}