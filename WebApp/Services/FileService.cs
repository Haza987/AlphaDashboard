using System.Diagnostics;

namespace WebApp.Services;

// Github Copilot suggested using a service instead of a controller for the image upload,
// this is so I can use the same service in multiple controllers.

public class FileService(IWebHostEnvironment env) : IFileService
{
    private readonly IWebHostEnvironment _env = env;

    public async Task<string?> SaveFileAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }


        var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", folderName);
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{folderName}/{fileName}";
    }
}
