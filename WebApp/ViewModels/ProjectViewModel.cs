﻿using Business.Dtos;
using Business.Models;

namespace WebApp.ViewModels;

public class ProjectViewModel
{
    public List<Project> Projects { get; set; } = [];
    public ProjectCreationViewModel ProjectCreationViewModel { get; set; } = new();
    public ProjectDto ProjectDto { get; set; } = new();
    public ProjectUpdateDto ProjectUpdateDto { get; set; } = new();
    
}
