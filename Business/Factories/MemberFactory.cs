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
        Role = dto.Role,
        Projects = dto.Projects.Select(p => new ProjectEntity
        {    
            ProjectName = p.ProjectName
        }).ToList()
    };
}
