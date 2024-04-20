using FluentValidation;
using solution.DTOs;

namespace solution.Validators;

public static class Validators
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        //services.AddValidatorsFromAssemblyContaining<AnimalCreateRequestValidator>();
        services.AddTransient<IValidator<CreateAnimalRequest>, AnimalCreateRequestValidator>();
    }
}