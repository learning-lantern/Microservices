using System.Web;
using FluentValidation;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class UpdateEmailDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UpdateEmailCommand : AuthorizedWithPasswordRequest<Response>, IHaveEmail
{
    public UpdateEmailCommand(UpdateEmailDTO updateEmailDTO)
    {
        Email = updateEmailDTO.Email;
        Password = updateEmailDTO.Password;
    }

    public string Email { get; set; }
}

public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        Include(new EmailValidator());
        Include(new PasswordValidator());
    }
}

public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, Response>
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<UserModel> _userManager;


    public UpdateEmailCommandHandler(UserManager<UserModel> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Response> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var token = await _userManager.GenerateChangeEmailTokenAsync(request.User, request.Email);

        var mailMessage = MessageTemplates.ChangeEmail(request.User.Id, request.Email, token);

        return await _emailSender.Send(request.Email, mailMessage);
    }
}