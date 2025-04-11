using Business.Dtos;
using Business.Models;

namespace Business.Interfaces
{
    public interface IMemberService
    {
        Task<bool> CreateMemberAsync(MemberDto dto);
        Task<bool> DeleteMemberAsync(int id);
        Task<IEnumerable<Member>?> GetAllMembersAsync();
        Task<Member?> GetMemberByIdAsync(int id);
        Task<bool> UpdateMemberAsync(int id, MemberUpdateDto updateDto);
    }
}