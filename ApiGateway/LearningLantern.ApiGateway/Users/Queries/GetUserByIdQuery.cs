using AutoMapper;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Queries;

public class GetUserByIdQuery : IRequest<Response<UserDTO>>
{
    public string UserId { get; set; } = null!;
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<UserDTO>>
{
    private readonly IMapper mapper;
    private readonly UserManager<UserModel> userManager;

    public GetUserByIdQueryHandler(UserManager<UserModel> userManager, IMapper mapper)
    {
        this.userManager = userManager;
        this.mapper = mapper;
    }


    public async Task<Response<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        return user is null
            ? ResponseFactory.Fail<UserDTO>(ErrorsList.UserIdNotFound(request.UserId))
            : ResponseFactory.Ok(mapper.Map<UserDTO>(user));
    }
}