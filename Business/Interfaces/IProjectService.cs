using Business.Dtos;
using Business.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(ProjectDto dto);
        Task<IEnumerable<Project>?> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<bool> UpdateProjectAsync(int id, ProjectUpdateDto updateDto);
        Task<bool> DeleteProjectAsync(int id);
    }
}