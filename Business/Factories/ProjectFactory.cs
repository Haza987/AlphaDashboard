using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public class ProjectFactory
{
    public static ProjectEntity CreateEntity(ProjectDto dto) => new()
    {
        ProjectId = dto.ProjectId,
        ProjectName = dto.ProjectName,
        ClientName = dto.ClientName,
        ProjectDescription = dto.ProjectDescription,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        Budget = dto.Budget
    };

    
}
