using Business.Factories;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddIdentity<UserEntity, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<MemberFactory>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserFactory>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ProjectFactory>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        var exists = await roleManager.RoleExistsAsync(role);
        if (!exists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    if (!userManager.Users.Any())
    {
        var adminUser = new UserEntity
        {
            FirstName = "Admin",
            LastName = "Account",
            Email = "admin@domain.com",
            UserName = "admin@domain.com",
            Role = "Admin"
        };

        var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Homepage}/{id?}")
    .WithStaticAssets();

app.Run();
