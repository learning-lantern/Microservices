using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
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
    private readonly ILogger<SendConfirmationEmailCommandHandler> _logger;
    private readonly UserManager<UserModel> _userManager;

    public SendConfirmationEmailCommandHandler(
        UserManager<UserModel> userManager, IEmailSender emailSender, ILogger<SendConfirmationEmailCommandHandler> logger)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<Response> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(request.User);

        _logger.LogInformation($"confirm Email token {token}");
        var mailMessage = MessageTemplates.ConfirmationEmail(request.User.Id, token);

        return await _emailSender.Send(request.User.Email, mailMessage);
    }
}