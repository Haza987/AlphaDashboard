using Business.Dtos;
using Business.Models;
using Data.Entities;
using System.Diagnostics;
using System.Linq;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity CreateMemberEntity(MemberDto dto, List<ProjectEntity> projects) => new()
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        JobTitle = dto.JobTitle,
        Address = dto.Address,
        DateOfBirth = dto.DateOfBirth,

        //Github Copilot helped me retrieve only the names of the projects
        Projects = projects
        .Where(p => dto.ProjectNames.Contains(p.ProjectName))
        .ToList()
    };

    public static Member CreateMemberModel(MemberEntity entity) => new Member
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email,
        PhoneNumber = entity.PhoneNumber,
        JobTitle = entity.JobTitle,
        Address = entity.Address,
        DateOfBirth = entity.DateOfBirth,

        Projects = entity.Projects?.Select(p => new ProjectDto
        {
            ProjectName = p.ProjectName,
        }).ToList() ?? []
    };

    public static MemberEntity UpdateMember(MemberEntity memberEntity, MemberUpdateDto memberUpdate)
    {
        memberEntity.FirstName = memberUpdate.FirstName ?? memberEntity.FirstName;
        memberEntity.LastName = memberUpdate.LastName ?? memberEntity.LastName;
        memberEntity.Email = memberUpdate.Email ?? memberEntity.Email;
        memberEntity.PhoneNumber = memberUpdate.PhoneNumber ?? memberEntity.PhoneNumber;
        memberEntity.JobTitle = memberUpdate.JobTitle ?? memberEntity.JobTitle;
        memberEntity.Address = memberUpdate.Address ?? memberEntity.Address;
        memberEntity.DateOfBirth = memberUpdate.DateOfBirth ?? memberEntity.DateOfBirth;

        if (memberUpdate.ProjectNames != null)
        {
            memberEntity.Projects = memberEntity.Projects?
                .Where(p => p.ProjectName != null && memberUpdate.ProjectNames.Contains(p.ProjectName))
                .ToList();
        }

        return memberEntity;
    }
}
