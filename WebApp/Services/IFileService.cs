
namespace WebApp.Services
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile file, string folderName);
    }
}