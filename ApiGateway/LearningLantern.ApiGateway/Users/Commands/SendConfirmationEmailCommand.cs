using System.Web;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class SendConfirmationEmailCommand : AuthorizedRequest<Response<TokenResponseDTO>>
{
}

public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand, Response<TokenResponseDTO>>
{
    private readonly IEmailSender _emailSender;
    private readonly JWTGenerator _jwtGenerator;
    private readonly ILogger<SendConfirmationEmailCommandHandler> _logger;
    private readonly UserManager<UserModel> _userManager;

    public SendConfirmationEmailCommandHandler(
        UserManager<UserModel> userManager, IEmailSender emailSender, ILogger<SendConfirmationEmailCommandHandler> logger,
        JWTGenerator jwtGenerator)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _logger = logger;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Response<TokenResponseDTO>> Handle(
        SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(request.User);
        var encodingToken = HttpUtility.UrlEncode(token);
        _logger.LogInformation($"confirm Email token {token}");
        _logger.LogInformation($"confirm Email encodingToken {encodingToken}");
        var mailMessage = MessageTemplates.ConfirmationEmail(request.User.Id, encodingToken);

        var response = await _emailSender.Send(request.User.Email, mailMessage);

        return response.Succeeded
            ? ResponseFactory.Ok(await _jwtGenerator.GenerateJwtSecurityToken(request.User))
            : ResponseFactory.Fail<TokenResponseDTO>(response.Errors!);
    }
}