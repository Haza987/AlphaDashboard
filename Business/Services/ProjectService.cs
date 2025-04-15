using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IMemberRepository memberRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IMemberRepository _memberRepository = memberRepository;

    public async Task<bool> CreateProjectAsync(ProjectDto dto)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            var members = (await _memberRepository.GetAllAsync())?.ToList() ?? [];
            var project = ProjectFactory.CreateProjectEntity(dto, members);
            var result = await _projectRepository.CreateAsync(project);
            if (result)
            {
                await _projectRepository.CommitTransactionAsync();
                return true;
            }
            return false;
        }
        catch
        {
            await _projectRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<Project>?> GetAllProjectsAsync()
    {
        var projectEntities = await _projectRepository.GetAllAsync();
        var projects = projectEntities?.Select(ProjectFactory.CreateProjectModel);
        return projects;
    }

    public async Task<Project?> GetProjectByIdAsync(string projectId)
    {
        var projectEntity = await _projectRepository.GetAsync(x => x.ProjectId == projectId);
        if (projectEntity == null)
        {
            return null;
        }
        var project = ProjectFactory.CreateProjectModel(projectEntity);
        return project;
    }

    public async Task<bool> UpdateProjectAsync(string projectId, ProjectUpdateDto updateDto)
    {
        await _projectRepository.BeginTransactionAsync();
        var projectEntity = await _projectRepository.GetAsync(x => x.ProjectId == projectId);

        if (projectEntity == null)
        {
            return false;
        }

        try
        {
            var updatedProject = ProjectFactory.UpdateProject(projectEntity, updateDto);
            var result = await _projectRepository.UpdateAsync(updatedProject);

            if (result)
            {
                await _projectRepository.CommitTransactionAsync();
            }
            return true;
        }
        catch
        {
            await _projectRepository.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> DeleteProjectAsync(string projectId)
    {
        await _projectRepository.BeginTransactionAsync();
        var projectEntity = await _projectRepository.GetAsync(x => x.ProjectId == projectId);

        if (projectEntity == null)
        {
            return false;
        }


        try
        {
            var result = await _projectRepository.DeleteAsync(projectEntity);
            if (result)
            {
                await _projectRepository.CommitTransactionAsync();
            }

            return true;
        }
        catch
        {
            await _projectRepository.RollbackTransactionAsync();
            return false;
        }
    }
}
