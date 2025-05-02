namespace WebApp.ViewModels;

public class FileUploadViewModel
{
    public IFormFile File { get; set; } = null!;
    public string? FilePath { get; set; }
}
