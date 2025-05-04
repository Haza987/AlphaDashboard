namespace WebApp.ViewModels;

public class FileUploadViewModel
{
    public IFormFile? File { get; set; }
    public string?FilePath { get; set; } = "/images/default-project.svg";
}
