using FluentValidation;
using LearningLantern.ApiGateway.Utility;

namespace LearningLantern.ApiGateway.Users.BuildingBlocks;

public interface IHavePassword
{
    string Password { get; set; }
}

public class PasswordObj : IHavePassword
{
    public string Password { get; set; } = null!;
}
public class PasswordValidator : AbstractValidator<IHavePassword>
{
    public PasswordValidator()
    {
        RuleFor(x => x.Password).NotNull().Matches(Pattern.Password);
    }
}