using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
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

            return View(viewModel);
        }
    }
}
