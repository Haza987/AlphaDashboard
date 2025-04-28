using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProjectsController(DataContext context, IProjectService projectService) : Controller
    {
        private readonly DataContext _context = context;
        private readonly IProjectService _projectService = projectService;

        [Route("projects")]
        public async Task<IActionResult> Projects()
        {
            int projectCount = _context.Projects.Count();
            int projectInProgress = _context.Projects.Count(p => !p.IsCompleted);
            int projectComplete = _context.Projects.Count(p => p.IsCompleted);

            var projects = await _projectService.GetAllProjectsAsync();

            var viewModel = new ProjectViewModel
            {
                Projects = projects?.ToList() ?? new List<Project>()
            };

            ViewBag.Title = "Projects";
            ViewBag.ProjectCount = projectCount;
            ViewBag.ProjectInProgress = projectInProgress;
            ViewBag.ProjectComplete = projectComplete;

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
            return PartialView("ProjectPartials/_ProjectCreation", new ProjectDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto form)
        {
            if (ModelState.IsValid)
            {
                var result = await _projectService.CreateProjectAsync(form);
                if (result)
                {
                    return RedirectToAction("Projects");
                }
            }

            return View("ProjectPartials/_ProjectCreation", form);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
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
                ProjectId = project.ProjectId,
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
                Debug.WriteLine($"Project with ID {id} not found.");
                return NotFound();
            }

            Debug.WriteLine($"Updating project status for ID {id}. Current IsCompleted: {project.IsCompleted}, New IsCompleted: {dto.IsCompleted}");

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
