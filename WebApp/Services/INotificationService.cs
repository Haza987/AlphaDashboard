using Data.Entities;

namespace WebApp.Services
{
    public interface INotificationService
    {
        Task AddNotificationAsync(NotificationEntity notificationEntity, string userId = "anonymous");
        Task DismissNotificationAsync(string userId, string notificationId);
        Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);
    }
}