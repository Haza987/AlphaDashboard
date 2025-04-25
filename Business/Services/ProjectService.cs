using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IMemberRepository memberRepository, DataContext context) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly DataContext _context = context;

    public async Task<bool> CreateProjectAsync(ProjectDto dto)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            if (await _projectRepository.ExistsAsync(x => x.ProjectName == dto.ProjectName))
            {
                Debug.WriteLine($"Project with name {dto.ProjectName} already exists.");
                return false;
            }

            var members = (await _memberRepository.GetAllAsync())?.ToList() ?? [];
            Debug.WriteLine($"Retrieved Members: {string.Join(", ", members.Select(m => m.FirstName + " " + m.LastName))}");

            var highestPID = await _projectRepository.GetAllAsync();
            var lastPID = highestPID!
                .Select(x => int.TryParse(x.ProjectId.Substring(2), out var id) ? id : 0)
                .DefaultIfEmpty(101)
                .Max();
            var nextPN = lastPID + 1;
            var newPN = $"P-{nextPN}";
            dto.ProjectId = newPN;

            Debug.WriteLine($"Generated ProjectId: {dto.ProjectId}");

            var project = ProjectFactory.CreateProjectEntity(dto, members);
            Debug.WriteLine($"Created ProjectEntity: {project.ProjectName}, ProjectId: {project.ProjectId}");

            var result = await _projectRepository.CreateAsync(project);
            if (result)
            {
                await _projectRepository.CommitTransactionAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in CreateProjectAsync: {ex.Message}");
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

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        // This is added to get the members of the project
        var projectEntity = await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (projectEntity == null)
        {
            return null;
        }

        var project = ProjectFactory.CreateProjectModel(projectEntity);
        return project;
    }

    public async Task<bool> UpdateProjectAsync(int id, ProjectUpdateDto updateDto)
    {
        await _projectRepository.BeginTransactionAsync();
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);

        if (projectEntity == null)
        {
            return false;
        }

        try
        {
            projectEntity = ProjectFactory.UpdateProject(projectEntity, updateDto);
            var result = await _projectRepository.UpdateAsync(projectEntity);
            await _projectRepository.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _projectRepository.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        await _projectRepository.BeginTransactionAsync();
        var projectEntity = await _projectRepository.GetAsync(x => x.Id == id);

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
