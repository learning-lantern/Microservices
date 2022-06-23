using FluentValidation;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class UpdatePasswordDTO
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class UpdatePasswordCommand : AuthorizedRequest<Response>
{
    public UpdatePasswordCommand(UpdatePasswordDTO updatePasswordDTO)
    {
        OldPassword.Password = updatePasswordDTO.OldPassword;
        NewPassword.Password = updatePasswordDTO.NewPassword;
    }

    public PasswordObj OldPassword { get; set; } = null!;
    public PasswordObj NewPassword { get; set; } = null!;
}

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator());
        RuleFor(x => x.OldPassword).SetValidator(new PasswordValidator());
    }
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Response>
{
    private readonly UserManager<UserModel> _userManager;

    public UpdatePasswordCommandHandler(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var updateAsyncResult = await _userManager.ChangePasswordAsync(request.User,
            request.OldPassword.Password, request.NewPassword.Password);

        return updateAsyncResult.ToApplicationResponse();
    }
}