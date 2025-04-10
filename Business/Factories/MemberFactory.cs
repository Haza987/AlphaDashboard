using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity CreateEntity(MemberDto dto) => new()
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        JobTitle = dto.JobTitle,
        Address = dto.Address,
        DateOfBirth = dto.DateOfBirth,

        //Just the name is needed
        Projects = dto.Projects.Select(ProjectFactory.CreateEntity).ToList()
    };
}
