using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class UserService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, IUserRepository userRepository, UserFactory userFactory) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserFactory _userFactory = userFactory;

    #region CRUD operations
    public async Task<IdentityResult> CreateUserAsync(UserDto dto)
    {
        Debug.WriteLine("UserService, CreateUserAsync, method called");

        await _userRepository.BeginTransactionAsync();
        Debug.WriteLine("UserService, CreateUserAsync, transaction started");

        try
        {
            var member = _userFactory.CreateUserEntity(dto);
            Debug.WriteLine($"UserService, CreateUserAsync, user entity created: FirstName={member.FirstName}, LastName={member.LastName}, Email={member.Email}");

            var result = await _userManager.CreateAsync(member);
            Debug.WriteLine($"UserService, CreateUserAsync, user creation result: Succeeded={result.Succeeded}");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Debug.WriteLine($"UserService, CreateUserAsync, error: Code={error.Code}, Description={error.Description}");
                }
            }

            if (result.Succeeded)
            {
                await _userRepository.CommitTransactionAsync();
                Debug.WriteLine("UserService, CreateUserAsync, transaction committed");
            }

            return result;
        }
        catch (Exception ex)
        {
            await _userRepository.RollbackTransactionAsync();
            Debug.WriteLine($"UserService, CreateUserAsync, transaction rolled back due to exception: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<User>?> GetAllUsersAsync()
    {
        var userEntities = await _userRepository.GetAllAsync();
        var users = userEntities?.Select(UserFactory.CreateUserModel);
        return users;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        var userEntity = await _userRepository.GetAsync(x => x.Id == id);

        if (userEntity == null)
        {
            return null;
        }

        var user = UserFactory.CreateUserModel(userEntity);
        return user;
    }

    public async Task<bool> UpdateUserAsync(string id, UserUpdateDto updateDto)
    {
        await _userRepository.BeginTransactionAsync();
        var userEntity = await _userRepository.GetAsync(x => x.Id == id);

        if (userEntity == null)
        {
            return false;
        }

        try
        {
            userEntity = UserFactory.UpdateUser(userEntity, updateDto);
            var result = await _userManager.UpdateAsync(userEntity);
            if (result.Succeeded)
            {
                await _userRepository.CommitTransactionAsync();
            }

            return true;
        }
        catch
        {
            await _userRepository.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        await _userRepository.BeginTransactionAsync();
        var userEntity = await _userRepository.GetAsync(x => x.Id == id);

        if (userEntity == null)
        {
            return false;
        }

        try
        {
            var result = await _userManager.DeleteAsync(userEntity);
            if (result.Succeeded)
            {
                await _userRepository.CommitTransactionAsync();
            }
            return true;
        }
        catch
        {
            await _userRepository.RollbackTransactionAsync();
            return false;
        }
    }

    #endregion

    #region User operations
    public async Task<SignInResult> SignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
        }
        return SignInResult.Failed;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    #endregion
}