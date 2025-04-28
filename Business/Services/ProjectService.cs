using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                return false;
            }

            var members = (await _memberRepository.GetAllAsync())?.ToList() ?? [];

            var highestPID = await _projectRepository.GetAllAsync();
            var lastPID = highestPID!
                .Select(x => int.TryParse(x.ProjectId.Substring(2), out var id) ? id : 0)
                .DefaultIfEmpty(100)
                .Max();
            var nextPN = lastPID + 1;
            var newPN = $"P-{nextPN}";
            dto.ProjectId = newPN;

            var project = ProjectFactory.CreateProjectEntity(dto, members);

            var result = await _projectRepository.CreateAsync(project);
            if (result)
            {
                await _projectRepository.CommitTransactionAsync();
                return true;
            }
            return false;
        }
        catch (Exception)
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

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        var projectEntity = await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == id);

        var project = ProjectFactory.CreateProjectModel(projectEntity!);
        return project;
    }


    //Github copilot helped me update the project members database table.
    public async Task<bool> UpdateProjectAsync(int id, ProjectUpdateDto updateDto)
    {
        await _projectRepository.BeginTransactionAsync();
        var projectEntity = await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (projectEntity == null)
        {
            return false;
        }

        try
        {
            // Get the current member IDs associated with the project
            var currentMemberIds = projectEntity.Members?.Select(m => m.Id).ToList() ?? new List<int>();

            // Determine which members to add and which to remove
            var membersToAdd = updateDto.Members?.Except(currentMemberIds).ToList() ?? new List<int>();
            var membersToRemove = currentMemberIds.Except(updateDto.Members ?? new List<int>()).ToList();

            // Add new members
            foreach (var memberId in membersToAdd)
            {
                var member = await _context.Members.FindAsync(memberId);
                if (member != null)
                {
                    projectEntity.Members?.Add(member);
                }
            }

            // Remove members no longer associated with the project
            foreach (var memberId in membersToRemove)
            {
                var member = projectEntity.Members?.FirstOrDefault(m => m.Id == memberId);
                if (member != null)
                {
                    projectEntity.Members?.Remove(member);
                }
            }

            // Update other project properties
            projectEntity = ProjectFactory.UpdateProject(projectEntity, updateDto);

            // Save changes
            var result = await _projectRepository.UpdateAsync(projectEntity);
            await _projectRepository.CommitTransactionAsync();
            return result;
        }

        catch (Exception)
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
