using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using AutoMapper;
using LearningLantern.ApiGateway.User.DTOs;
using LearningLantern.ApiGateway.User.Events;
using LearningLantern.ApiGateway.User.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.DependencyInjection;
using LearningLantern.Common.EventBus;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LearningLantern.ApiGateway.User.Repository;

public class UserRepository : IUserRepository
{
    private readonly IEmailSender _emailSender;
    private readonly IEventBus _eventBus;
    private readonly ILogger<UserRepository> _logger;
    private readonly IMapper _mapper;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly UserManager<UserModel> _userManager;


    public UserRepository(
        IEmailSender emailSender,
        IEventBus eventBus,
        ILogger<UserRepository> logger,
        IMapper mapper,
        SignInManager<UserModel> signInManager,
        UserManager<UserModel> userManager)
    {
        _emailSender = emailSender;
        _eventBus = eventBus;
        _logger = logger;
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<UserModel?> FindUserByIdAsync(string userId) => await _userManager.FindByIdAsync(userId);

    public async Task<Response> Signup(SignupDTO signupDTO)
    {
        var user = _mapper.Map<UserModel>(signupDTO);

        var createAsyncResult = await _userManager.CreateAsync(user, signupDTO.Password);

        if (createAsyncResult.Succeeded) _eventBus.Publish(_mapper.Map<CreateUserEvent>(user));

        return createAsyncResult.ToApplicationResponse();
    }

    public async Task<Response<SignInResponseDTO>> Login(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user is null)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.UserEmailNotFound(loginDTO.Email));

        var passwordSignInAsyncResult = await _signInManager.PasswordSignInAsync(
            loginDTO.Email, loginDTO.Password,
            true, false);

        if (passwordSignInAsyncResult.IsNotAllowed)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.SignInNotAllowed());
        if (passwordSignInAsyncResult.Succeeded == false)
            return ResponseFactory.Fail<SignInResponseDTO>(ErrorsList.SignInFailed());

        var result = new SignInResponseDTO(_mapper.Map<UserDTO>(user), await GenerateJwtSecurityToken(user));

        return ResponseFactory.Ok(result);
    }

    public async Task<Response> UpdateName(string userId, UpdateNameDTO updateNameDTO)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(userId));

        user.FirstName = updateNameDTO.FirstName.Trim();
        user.LastName = updateNameDTO.LastName.Trim();

        var updateAsyncResult = await _userManager.UpdateAsync(user);

        if (updateAsyncResult.Succeeded) _eventBus.Publish(_mapper.Map<UpdateUserEvent>(user));

        return updateAsyncResult.ToApplicationResponse();
    }

    public async Task<Response> UpdatePassword(string userId, UpdatePasswordDTO updatePasswordDTO)
    {
        var user = await FindUserByIdAsync(userId);
        if (user == null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(userId));

        var updateAsyncResult = await _userManager.ChangePasswordAsync(user, updatePasswordDTO.OldPassword,
            updatePasswordDTO.NewPassword);

        return updateAsyncResult.ToApplicationResponse();
    }

    public async Task<Response> UpdateEmail(string userId, UpdateEmailDTO updateEmailDTO, string? endPointUrl)
    {
        var user = await FindUserByIdAsync(userId);
        if (user == null) return ResponseFactory.Fail<string>(ErrorsList.UserIdNotFound(userId));

        if (await _userManager.CheckPasswordAsync(user, updateEmailDTO.Password) == false)
            return ResponseFactory.Fail(ErrorsList.IncorrectPassword(userId));

        var token = HttpUtility.UrlEncode(await _userManager.GenerateChangeEmailTokenAsync(user, updateEmailDTO.Email));

        var mailMessage = MessageTemplates.ChangeEmail(endPointUrl, user.Id, updateEmailDTO.Email, token);


        return await _emailSender.Send(updateEmailDTO.Email, mailMessage);
    }

    public async Task<Response> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(userId));

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded) _eventBus.Publish(_mapper.Map<UpdateUserEvent>(user));

        return result.ToApplicationResponse();
    }

    public async Task<Response> SendConfirmationEmail(string userEmail, string? endPointUrl)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null) return ResponseFactory.Fail(ErrorsList.UserEmailNotFound(userEmail));

        var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));

        var mailMessage = MessageTemplates.ConfirmationEmail(endPointUrl, user.Id, token);

        return await _emailSender.Send(userEmail, mailMessage);
    }

    public async Task<Response> ConfirmUpdateEmail(string userId, string newEmail, string token)
    {
        var user = await FindUserByIdAsync(userId);
        if (user == null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(userId));

        var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

        if (result.Succeeded) _eventBus.Publish(_mapper.Map<UpdateUserEvent>(user));

        return result.ToApplicationResponse();
    }

    public async Task<Response> DeleteUser(string userId, DeleteUserDTO deleteUserDTO)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return ResponseFactory.Fail(ErrorsList.UserIdNotFound(userId));
        await _signInManager.SignOutAsync();
        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded) _eventBus.Publish(new DeleteUserEvent {Id = userId});

        return result.ToApplicationResponse();
    }

    public Task<Response<IEnumerable<UserDTO>>> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = _userManager.Users.Select(x => _mapper.Map<UserDTO>(x)).ToPaginatedList(pageNumber, pageSize);
        var response = ResponseFactory.Ok<IEnumerable<UserDTO>>(users);
        return Task.FromResult(response);
    }

    public async Task<Response<UserDTO>> GetById(string userId)
    {
        var user = await FindUserByIdAsync(userId);

        return user is null
            ? ResponseFactory.Fail<UserDTO>(ErrorsList.UserIdNotFound(userId))
            : ResponseFactory.Ok(_mapper.Map<UserDTO>(user));
    }

    async private Task<string> GenerateJwtSecurityToken(UserModel user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            JWT.ValidIssuer,
            JWT.ValidAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(JWT.IssuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature)
        ));
    }
}