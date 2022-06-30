using System.Web;
using AutoMapper;
using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class ConfirmEmailCommand : IRequest<Response<TokenResponseDTO>>
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
}

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Response<TokenResponseDTO>>
{
    private readonly IEventBus _eventBus;
    private readonly JWTGenerator _jwtGenerator;
    private readonly ILogger<ConfirmEmailHandler> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;

    public ConfirmEmailHandler(
        IEventBus eventBus, IMapper mapper, UserManager<UserModel> userManager, ILogger<ConfirmEmailHandler> logger,
        JWTGenerator jwtGenerator)
    {
        _eventBus = eventBus;
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Response<TokenResponseDTO>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return ResponseFactory.Fail<TokenResponseDTO>(ErrorsList.UserIdNotFound(request.UserId));

        var token = HttpUtility.UrlDecode(request.Token);

        _logger.LogInformation($"requst token = {request.Token}");
        _logger.LogInformation($"token = {token}");

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        return result.Succeeded == false
            ? result.ToApplicationResponse<TokenResponseDTO>()
            : ResponseFactory.Ok(await _jwtGenerator.GenerateJwtSecurityToken(user));
    }
}