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
                if (form.Members != null && form.Members.Any())
                {
                    Debug.WriteLine("Members added to the project:");
                    Debug.WriteLine($"Member IDs: {string.Join(", ", form.Members)}");
                }
                else
                {
                    Debug.WriteLine("No members were added to the project.");
                }

                var result = await _projectService.CreateProjectAsync(form);
                if (result)
                {
                    return RedirectToAction("Projects");
                }
            }
            return View("ProjectPartials/_ProjectCreation", form);
        }
    }
}
