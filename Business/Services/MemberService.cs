using Business.Dtos;
using Business.Factories;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, SignInManager<MemberEntity> signInManager)
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;

    public async Task<IdentityResult> CreateMemberAsync(MemberDto dto, string password)
    {
        var member = MemberFactory.CreateEntity(dto);
        var result = await _userManager.CreateAsync(member, password);
        return result;
    }

    public async Task<SignInResult> SignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
        }
        return SignInResult.Failed;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
