using Business.Dtos;
using Business.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(ProjectDto dto);
        Task<bool> DeleteProjectAsync(string projectId);
        Task<IEnumerable<Project>?> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(string projectId);
        Task<bool> UpdateProjectAsync(string projectId, ProjectUpdateDto updateDto);
    }
}