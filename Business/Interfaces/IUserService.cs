using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<SignInResult> SignInAsync(string email, string password);
        Task SignOutAsync();
    }
}