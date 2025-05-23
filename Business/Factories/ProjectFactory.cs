﻿using Business.Dtos;
using Business.Models;
using Data.Entities;
using System.Diagnostics;

namespace Business.Factories;

public class ProjectFactory
{
    public static ProjectEntity CreateProjectEntity(ProjectDto dto, List<MemberEntity> members)
    {
        var filteredMembers = members.Where(m => dto.Members.Contains(m.Id)).ToList();

        return new ProjectEntity
        {
            ProjectId = dto.ProjectId!,
            ProjectImageFilePath = dto.ProjectImageFilePath,
            ProjectName = dto.ProjectName,
            ClientName = dto.ClientName,
            ProjectDescription = dto.ProjectDescription,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Budget = dto.Budget,
            IsCompleted = dto.IsCompleted,
            Members = filteredMembers
        };
    }

    public static Project CreateProjectModel(ProjectEntity entity)
    {
        return new Project
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            ProjectImageFilePath = entity.ProjectImageFilePath,
            ProjectName = entity.ProjectName,
            ClientName = entity.ClientName,
            ProjectDescription = entity.ProjectDescription,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            IsCompleted = entity.IsCompleted,
            Members = entity.Members?.Select(m => new MemberDto
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName
            }).ToList() ?? new List<MemberDto>()
        };
    }

    public static ProjectEntity UpdateProject(ProjectEntity projectEntity, ProjectUpdateDto projectUpdate)
    {
        projectEntity.ProjectImageFilePath = projectUpdate.ProjectImageFilePath ?? projectEntity.ProjectImageFilePath;
        projectEntity.ProjectName = projectUpdate.ProjectName ?? projectEntity.ProjectName;
        projectEntity.ClientName = projectUpdate.ClientName ?? projectEntity.ClientName;
        projectEntity.ProjectDescription = projectUpdate.ProjectDescription ?? projectEntity.ProjectDescription;
        projectEntity.StartDate = projectUpdate.StartDate ?? projectEntity.StartDate;
        projectEntity.EndDate = projectUpdate.EndDate ?? projectEntity.EndDate;
        projectEntity.Budget = projectUpdate.Budget ?? projectEntity.Budget;
        projectEntity.IsCompleted = projectUpdate.IsCompleted ?? projectEntity.IsCompleted;

        if (projectUpdate.Members != null)
        {
            projectEntity.Members = projectEntity.Members?
                .Where(m => projectUpdate.Members.Contains(m.Id))
                .ToList();
        }

        return projectEntity;
    }
}
