using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApp.Hubs;

namespace WebApp.Services;

public class NotificationService(DataContext context, IHubContext<NotificationHub> notificationHub) : INotificationService
{
    private readonly DataContext _context = context;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

    public async Task AddNotificationAsync(NotificationEntity notificationEntity, string userId = "anonymous")
    {
        if (string.IsNullOrEmpty(notificationEntity.Icon))
        {
            switch (notificationEntity.NotificationTypeId)
            {
                case 1:
                    notificationEntity.Icon = "/images/default-user.svg";
                    break;
                case 2:
                    notificationEntity.Icon = "/images/default-project.svg";
                    break;
                case 3:
                    notificationEntity.Icon = "/images/default-member.svg";
                    break;
            }
        }

        if (string.IsNullOrEmpty(notificationEntity.Message))
        {
            notificationEntity.Message = "No message provided.";
        }

        notificationEntity.Created = DateTime.UtcNow;

        _context.Add(notificationEntity);
        await _context.SaveChangesAsync();
        var notification = await GetNotificationsAsync(userId);

        var newNotification = notification.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
        {
            await _notificationHub.Clients.All.SendAsync("ReceiveNotification", new
            {
                id = newNotification.Id,
                icon = newNotification.Icon,
                message = newNotification.Message,
                created = newNotification.Created
            });
        }
    }

    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10)
    {
        var dismissedIds = await _context.NotificationDismissed
            .Where(x => x.UserId == userId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();

        return notifications;
    }

    public async Task DismissNotificationAsync(string userId, string notificationId)
    {
        var alreadyDismissed = await _context.NotificationDismissed.AnyAsync(x => x.NotificationId == notificationId && x.UserId == userId);
        if (!alreadyDismissed)
        {
            var dismissed = new NotificationDismissedEntity
            {
                NotificationId = notificationId,
                UserId = userId
            };

            _context.Add(dismissed);
            await _context.SaveChangesAsync();
            await _notificationHub.Clients.User(userId).SendAsync("NotificationDismissed", notificationId);
        }
    }
}
