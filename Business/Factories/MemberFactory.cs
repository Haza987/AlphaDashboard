using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity CreateEntity(MemberDto dto) => new()
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        UserName = dto.Email,
        Role = dto.Role ?? "User"
    };
}
