using AutoMapper;
using FluentValidation;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.ApiGateway.Users.Events;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Response;
using LearningLantern.EventBus.Publisher;
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
    private readonly ILogger<UpdateNameCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<UserModel> _userManager;


    public UpdateNameCommandHandler(
        UserManager<UserModel> userManager, IEventBus eventBus, IMapper mapper, ILogger<UpdateNameCommandHandler> logger)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        request.User.FirstName = request.FirstName.Trim();
        request.User.LastName = request.LastName.Trim();

        var updateAsyncResult = await _userManager.UpdateAsync(request.User);

        try
        {
            if (updateAsyncResult.Succeeded)
                _eventBus.Publish(_mapper.Map<UserEvent>(request.User));
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Can't publish the event in {nameof(UpdateNameCommandHandler)}, Exception");
            _logger.LogCritical(e.Message);
        }

        return updateAsyncResult.ToApplicationResponse();
    }
}