﻿using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    public string? UserImageFilePath { get; set; }

    public string Role { get; set; } = null!;
    public ICollection<NotificationDismissedEntity> DismissedNotifications { get; set; } = [];
}
