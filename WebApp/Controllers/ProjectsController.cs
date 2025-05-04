using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProjectsController(DataContext context, IProjectService projectService, IFileService fileService, INotificationService notificationService) : Controller
    {
        private readonly DataContext _context = context;
        private readonly IProjectService _projectService = projectService;
        private readonly IFileService _fileService = fileService;
        private readonly INotificationService _notificationService = notificationService;

        [Authorize]
        [Route("projects")]
        public async Task<IActionResult> Projects(string filter = "ALL")
        {
            var projects = await _projectService.GetAllProjectsAsync();

            int projectCount = _context.Projects.Count();
            int projectInProgress = _context.Projects.Count(p => !p.IsCompleted);
            int projectComplete = _context.Projects.Count(p => p.IsCompleted);

            var viewModel = new ProjectViewModel
            {
                Projects = projects?.ToList() ?? new List<Project>()
            };

            ViewBag.Title = "Projects";
            ViewBag.ProjectCount = projectCount;
            ViewBag.ProjectInProgress = projectInProgress;
            ViewBag.ProjectComplete = projectComplete;
            ViewBag.Filter = filter.ToUpper();

            ViewBag.Members = _context.Members
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName
                })
                .ToList();


            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            var viewModel = new ProjectCreationViewModel
            {
                FileUploadViewModel = new FileUploadViewModel()
            };
            return PartialView("ProjectPartials/_ProjectCreation", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreationViewModel form)
        {
            if (ModelState.IsValid)
            {
                string uploadedFilePath;

                if (form.FileUploadViewModel?.File == null || form.FileUploadViewModel.File.Length == 0)
                {
                    uploadedFilePath = "/images/default-project.svg";
                }
                else
                {
                    uploadedFilePath = await _fileService.SaveFileAsync(form.FileUploadViewModel.File, "ProjectImages") ?? string.Empty;
                }

                var project = new ProjectDto
                {
                    ProjectName = form.ProjectName,
                    ClientName = form.ClientName,
                    ProjectDescription = form.ProjectDescription,
                    StartDate = form.StartDate,
                    EndDate = form.EndDate,
                    Budget = form.Budget,
                    IsCompleted = form.IsCompleted,
                    Members = form.Members?.Distinct().ToList() ?? new List<int>(),
                    ProjectImageFilePath = uploadedFilePath
                };

                var result = await _projectService.CreateProjectAsync(project);
                if (result)
                {
                    var notificationEntity = new NotificationEntity
                    {
                        Message = $"Project {project.ProjectName} created.",
                        NotificationTypeId = 2
                    };

                    await _notificationService.AddNotificationAsync(notificationEntity);
                }
                return RedirectToAction("Projects");            
            }
            return View("ProjectPartials/_ProjectCreation", form);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return Json(project);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var updateDto = new ProjectUpdateDto
            {
                id = project.Id,
                ProjectId = project.ProjectId,
                ProjectImageFilePath = project.ProjectImageFilePath,
                ProjectName = project.ProjectName,
                ClientName = project.ClientName,
                ProjectDescription = project.ProjectDescription,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                IsCompleted = project.IsCompleted,
                Members = project.Members.Select(m => m.Id).ToList()
            };

            var allMembers = _context.Members
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName
                })
                .ToList();

            ViewBag.Members = allMembers;
            ViewData["ProjectId"] = project.Id;

            return Json(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(int id, ProjectUpdateDto dto)
        {
            dto.Members = dto.Members?.Distinct().ToList();

            if (ModelState.IsValid)
            {
                var result = await _projectService.UpdateProjectAsync(id, dto);

                if (result)
                {
                    return RedirectToAction("Projects");
                }
            }
            return PartialView("ProjectPartials/_ProjectUpdate", dto);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Failed to delete project.");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProjectStatus(int id, ProjectUpdateDto dto)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            project.IsCompleted = dto.IsCompleted ?? project.IsCompleted;

            var result = await _projectService.UpdateProjectAsync(id, new ProjectUpdateDto
            {
                IsCompleted = dto.IsCompleted
            });

            if (result)
            {
                return Ok();
            }

            return BadRequest("Failed to update project status.");
        }
    }
}
