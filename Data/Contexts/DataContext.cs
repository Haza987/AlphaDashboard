using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<ProjectEntity> Projects { get; set; } = null!;
    public DbSet<MemberEntity> Members { get; set; } = null!;
    public DbSet<NotificationEntity> Notifications { get; set; } = null!;
    public DbSet<NotificationTypeEntity> NotificationTypes { get; set; } = null!;
    public DbSet<NotificationTargetGroupEntity> NotificationTargetGroups { get; set; } = null!;
    public DbSet<NotificationDismissedEntity> NotificationDismissed { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectEntity>()
            .HasMany(x => x.Members)
            .WithMany(x => x.Projects)
            .UsingEntity(j => j.ToTable("ProjectMembers"));
    }
}
