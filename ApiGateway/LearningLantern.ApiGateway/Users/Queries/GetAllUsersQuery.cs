using AutoMapper;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Queries;

public class GetAllUsersQuery : IRequest<Response<IEnumerable<UserDTO>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Response<IEnumerable<UserDTO>>>
{
    private readonly IMapper mapper;
    private readonly UserManager<UserModel> userManager;

    public GetAllUsersHandler(IMapper mapper, UserManager<UserModel> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
    }

    public Task<Response<IEnumerable<UserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = userManager.Users.Select(user => mapper.Map<UserDTO>(user))
            .ToPaginatedList(request.PageNumber, request.PageSize);
        
        return Task.FromResult(ResponseFactory.Ok<IEnumerable<UserDTO>>(users));
    }
}