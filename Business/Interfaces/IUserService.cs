using Business.Dtos;
using Business.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(UserDto dto);
    Task<bool> DeleteUserAsync(string id);
    Task<IEnumerable<User>?> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<SignInResult> SignInAsync(string email, string password);
    Task SignOutAsync();
    Task<bool> UpdateUserAsync(string id, UserUpdateDto updateDto);
}