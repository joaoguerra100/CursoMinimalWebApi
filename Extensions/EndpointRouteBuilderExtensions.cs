using MicroWebApi.EndpointHandlers;

namespace MicroWebApi.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
            var rangosComIdEndpoints = rangosEndpoints.MapGroup("/{rangoID:int}");

            rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

            rangosComIdEndpoints.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

            rangosEndpoints.MapPost("", RangosHandlers.CreateRangoAsync);

            rangosComIdEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync)
                .AddEndpointFilter(async (context, next) =>
                {
                    var rangoId = context.GetArgument<int>(2);

                    var result = await next.Invoke(context);
                    return result;
                });

            rangosComIdEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync);
        }

        public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ingredientesEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes");
            ingredientesEndpoints.MapGet("", IngredientesHandlers.GetIngredientesAsync);
        }
    }
}