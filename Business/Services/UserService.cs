using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class UserService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    //public async Task<> CreateUserAsync()
    //{
    //    await _memberRepository.BeginTransactionAsync();
    //    try
    //    {
    //        var member = MemberFactory.CreateEntity(dto);
    //        var result = await _userManager.CreateAsync(member, password);
    //        if (result.Succeeded)
    //        {
    //            await _memberRepository.CommitTransactionAsync();
    //        }
    //        return result;
    //    }
    //    catch
    //    {
    //        await _memberRepository.RollbackTransactionAsync();
    //        throw;
    //    }
    //}

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
}