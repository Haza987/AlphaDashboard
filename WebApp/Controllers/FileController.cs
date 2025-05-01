using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class FileController(IWebHostEnvironment env) : Controller
{
    private readonly IWebHostEnvironment _env = env;

    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(FileUploadViewModel viewModel)
    {
        if(!ModelState.IsValid || viewModel.File == null || viewModel.File.Length == 0)
        {
            return View(viewModel);
        }

        var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadFolder);

        var filePath = Path.Combine(uploadFolder, $"{Guid.NewGuid()}_{Path.GetFileName(viewModel.File.FileName)}");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await viewModel.File.CopyToAsync(stream);
        }

        return View();
    }
}
