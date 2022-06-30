using FluentValidation;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.Users.BuildingBlocks;

public interface IHavePassword
{
    public string Password { get; set; }
}

public class PasswordObj : IHavePassword
{
    public string Password { get; set; }
}
public class PasswordValidator : AbstractValidator<IHavePassword>
{
    public PasswordValidator()
    {
        RuleFor(x => x.Password).NotNull().Matches(Pattern.Password)
            .WithMessage("The field {PropertyName} must match the regular expression \'" + Pattern.Password + "\'.");
    }
}