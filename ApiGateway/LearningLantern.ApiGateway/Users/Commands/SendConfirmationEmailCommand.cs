using System.Web;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Responses;
using LearningLantern.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class SendConfirmationEmailCommand : AuthorizedRequest<Response>
{
}

public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand, Response>
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<UserModel> _userManager;


    public SendConfirmationEmailCommandHandler(UserManager<UserModel> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Response> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(request.User));

        var mailMessage = MessageTemplates.ConfirmationEmail(request.User.Id, token);

        return await _emailSender.Send(request.User.Email, mailMessage);
    }
}