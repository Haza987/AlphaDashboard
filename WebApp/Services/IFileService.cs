
namespace WebApp.Services
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile file, string folderName);
        Task<string?> UpdateFileAsync(IFormFile newFile, string foldername, string? oldFilePath);
    }
}