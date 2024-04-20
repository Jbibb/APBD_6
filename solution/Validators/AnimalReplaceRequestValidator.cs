using FluentValidation;
using solution.DTOs;

namespace solution.Validators;

public class AnimalReplaceRequestValidator : AbstractValidator<ReplaceAnimalRequest>
{
    public AnimalReplaceRequestValidator()
    {
        RuleFor(s => s.Name).MaximumLength(200).NotNull();
        RuleFor(s => s.Area).MaximumLength(200).NotNull();
        RuleFor(s => s.Category).MaximumLength(200).NotNull();
        RuleFor(s => s.Description).MaximumLength(200);
    }
}