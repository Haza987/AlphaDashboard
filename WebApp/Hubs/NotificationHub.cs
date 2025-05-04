using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }

    public async Task SendNotificationToAdmins(object notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }
}
