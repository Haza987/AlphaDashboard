using Business.Dtos;
using Business.Models;
using Microsoft.AspNetCore.Identity;
using WebApp.ViewModels;

namespace Business.Interfaces;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(UserDto dto);
    Task<IEnumerable<User>?> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<bool> DeleteUserAsync(string id);
    Task<bool> UpdateUserAsync(string id, UserUpdateDto updateDto);
    Task<bool> UserExistsAsync(string id);
    Task<SignInResult> SignInAsync(SignInViewModel model);
    Task SignOutAsync();
}