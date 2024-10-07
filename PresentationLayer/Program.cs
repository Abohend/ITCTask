using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Implementations;
using EntityLayer.Models;
using EntityLayer.Repositories;
using PresentationLayer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.Lockout.AllowedForNewUsers = false;
})
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<FileService>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// seed data
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // seed roles
    var roles = new[]
    {
                    new IdentityRole("admin"),
                    new IdentityRole("user")
                };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role.Name!))
        {
            await roleManager.CreateAsync(role);
        }
    }

    // seed default admin
    if (await userManager.FindByEmailAsync("mhmdabohend@gmail.com") == null)
    {
        var admin = new ApplicationUser()
        {

            Name = "Mohamed Abohend",
            Email = "mhmdabohend@gmail.com",
            UserName = "mhmdabohend@gmail.com"
        };
        var res = await userManager.CreateAsync(admin, "Mm@12345");
        await userManager.AddToRoleAsync(admin, "admin");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Products}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();