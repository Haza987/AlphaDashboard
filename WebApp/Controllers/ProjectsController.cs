using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
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
            Debug.WriteLine("[DEBUG] CreateProject called.");
            Debug.WriteLine($"[DEBUG] Form Data: ProjectName={form.ProjectName}, ClientName={form.ClientName}, Description={form.ProjectDescription}, StartDate={form.StartDate}, EndDate={form.EndDate}, Budget={form.Budget}");
            Debug.WriteLine($"[DEBUG] Members: {string.Join(", ", form.Members ?? new List<int>())}");

            if (ModelState.IsValid)
            {
                Debug.WriteLine("[DEBUG] ModelState is valid.");
                var result = await _projectService.CreateProjectAsync(form);
                Debug.WriteLine($"[DEBUG] CreateProjectAsync result: {result}");

                if (result)
                {
                    return RedirectToAction("Projects");
                }
            }
            else
            {
                Debug.WriteLine("[DEBUG] ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"[DEBUG] ModelState Error: {error.ErrorMessage}");
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
            Debug.WriteLine($"[DEBUG] UpdateProject called with ID: {id}");

            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                Debug.WriteLine($"[DEBUG] Project with ID {id} not found.");
                return NotFound();
            }

            Debug.WriteLine($"[DEBUG] Project Details:");
            Debug.WriteLine($"  - Project ID: {project.ProjectId}");
            Debug.WriteLine($"  - Members: {string.Join(", ", project.Members.Select(m => m.Id))}");

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

            Debug.WriteLine($"[DEBUG] ProjectUpdateDto created:");
            Debug.WriteLine($"  - Members: {string.Join(", ", updateDto.Members)}");

            var allMembers = _context.Members
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName
                })
                .ToList();

            Debug.WriteLine($"[DEBUG] All Members fetched from database:");
            foreach (var member in allMembers)
            {
                Debug.WriteLine($"  - Member ID: {member.Id}, Name: {member.FirstName} {member.LastName}");
            }

            ViewBag.Members = allMembers;

            return PartialView("ProjectPartials/_ProjectUpdate", updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(int id, ProjectUpdateDto form)
        {
            Debug.WriteLine("[DEBUG] UpdateProject called.");
            Debug.WriteLine($"[DEBUG] Form Data: ProjectName={form.ProjectName}, ClientName={form.ClientName}, Description={form.ProjectDescription}, StartDate={form.StartDate}, EndDate={form.EndDate}, Budget={form.Budget}");
            Debug.WriteLine($"[DEBUG] Members: {string.Join(", ", form.Members ?? new List<int>())}");

            if (ModelState.IsValid)
            {
                Debug.WriteLine("[DEBUG] ModelState is valid.");
                var result = await _projectService.UpdateProjectAsync(id, form);
                Debug.WriteLine($"[DEBUG] UpdateProjectAsync result: {result}");

                if (result)
                {
                    return RedirectToAction("Projects");
                }
            }
            else
            {
                Debug.WriteLine("[DEBUG] ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"[DEBUG] ModelState Error: {error.ErrorMessage}");
                }
            }

            return PartialView("ProjectPartials/_ProjectUpdate", form);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProject(int id)
        {
            Debug.WriteLine($"Received ID for deletion: {id}");
            var result = await _projectService.DeleteProjectAsync(id);
            if (result)
            {
                Debug.WriteLine($"Successfully deleted project with ID: {id}");
                return Ok();
            }
            Debug.WriteLine($"Failed to delete project with ID: {id}");
            return BadRequest("Failed to delete project.");
        }

    }
}
