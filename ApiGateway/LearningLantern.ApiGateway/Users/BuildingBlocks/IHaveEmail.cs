using FluentValidation;

namespace LearningLantern.ApiGateway.Users.BuildingBlocks;

public interface IHaveEmail
{
    public string Email { get; set; }
}

public class EmailValidator : AbstractValidator<IHaveEmail>
{
    public EmailValidator()
    {
        RuleFor(x => x.Email).NotNull().EmailAddress();
    }
}