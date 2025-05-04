using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApp.Hubs;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(IHubContext<NotificationHub> notificationHub, INotificationService notificationService) : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
        private readonly INotificationService _notificationService = notificationService;

        [HttpPost]
        public async Task<IActionResult> CreateNotification(NotificationEntity notificationEntity)
        {
            await _notificationService.AddNotificationAsync(notificationEntity);
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            else
            {
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                return Ok(notifications);
            }
        }

        [HttpPost("dismiss/{id}")]
        public async Task<IActionResult> DismissNotification(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _notificationService.DismissNotificationAsync(userId, id);
            await _notificationHub.Clients.User(userId).SendAsync("NotificationDismissed", id);
            return Ok(new { success = true });
        }
    }
}
