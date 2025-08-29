
using MicroWebApi.Models;
using MiniValidation;

namespace MicroWebApi.EndpointFilters
{
    public class ValidateAnnotationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var rangoParaCriacaoDTIO = context.GetArgument<RangoParaCriacaoDTO>(2);

            if (!MiniValidator.TryValidate(rangoParaCriacaoDTIO, out var validationErrors))
            {
                return TypedResults.ValidationProblem(validationErrors);
            }

            return await next(context);
        }
    }
}