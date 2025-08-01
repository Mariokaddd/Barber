using Barber.Data;
using Barber.Data.models;
using Barber.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Barber
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<ScheduleService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>(); // 👈 ADD THIS


            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();




            builder.Services.AddScoped<ScheduleService>();

            var app = builder.Build();

            // Seed roles and admin user
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Създаване на ролята Admin, ако не съществува
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Дефинирай админ потребител
                var adminEmail = "mariokaddd@abv.bg";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    // Ако няма такъв потребител, създай го
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                    };

                    var createAdminResult = await userManager.CreateAsync(adminUser, "Mario9701*");

                    if (createAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        // Тук можеш да логнеш грешките
                        throw new Exception("Failed to create admin user: " + string.Join(", ", createAdminResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    // Ако потребителят съществува, но няма ролята Admin — добави я
                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
