using Business.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces
{
    public interface IMemberService
    {
        Task<bool> CreateMemberAsync(MemberDto dto);
    }
}