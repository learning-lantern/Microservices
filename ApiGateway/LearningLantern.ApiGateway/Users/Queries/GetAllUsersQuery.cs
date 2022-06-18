using AutoMapper;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.Common.Response;
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
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;

    public GetAllUsersHandler(IMapper mapper, UserManager<UserModel> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public Task<Response<IEnumerable<UserDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.Select(x => _mapper.Map<UserDTO>(x))
            .ToPaginatedList(request.PageNumber, request.PageSize);
        var response = ResponseFactory.Ok<IEnumerable<UserDTO>>(users);
        return Task.FromResult(response);
    }
}