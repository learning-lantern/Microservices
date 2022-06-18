using FluentValidation;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.Users.BuildingBlocks;

public interface IHavePersonName
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class PersonNameValidator : AbstractValidator<IHavePersonName>
{
    public PersonNameValidator()
    {
        RuleFor(x => x.FirstName).NotNull().Matches(Pattern.Name).WithMessage(
            "The field {PropertyName} must match the regular expression {expression}."
        );
        RuleFor(x => x.LastName).NotNull().Matches(Pattern.Name).WithMessage(
            "The field {PropertyName} must match the regular expression \'" + Pattern.Name + "\'"
        );
    }
}