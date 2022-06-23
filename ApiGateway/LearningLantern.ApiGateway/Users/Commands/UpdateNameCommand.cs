using AutoMapper;
using FluentValidation;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.ApiGateway.Users.Models;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.Users.Commands;

public class UpdateNameDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}

public class UpdateNameCommand : AuthorizedWithPasswordRequest<Response>, IHavePersonName
{
    public UpdateNameCommand(UpdateNameDTO updateNameDTO)
    {
        FirstName = updateNameDTO.FirstName;
        LastName = updateNameDTO.LastName;
        Password = updateNameDTO.Password;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
{
    public UpdateNameCommandValidator()
    {
        Include(new PasswordValidator());
        Include(new PersonNameValidator());
    }
}

public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand, Response>
{
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;


    public UpdateNameCommandHandler(UserManager<UserModel> userManager, IEventBus eventBus, IMapper mapper)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _mapper = mapper;
    }

    public async Task<Response> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        request.User.FirstName = request.FirstName.Trim();
        request.User.LastName = request.LastName.Trim();

        var updateAsyncResult = await _userManager.UpdateAsync(request.User);

        if (updateAsyncResult.Succeeded) _eventBus.Publish(_mapper.Map<UpdateUserEvent>(request.User));

        return updateAsyncResult.ToApplicationResponse();
    }
}