using Microsoft.EntityFrameworkCore;
using Myshop.DataAccess;
using Myshop.DataAccess.Data;
using Myshop.DataAccess.Implementaion;
using Myshop.Entities.Repositres;
using Microsoft.AspNetCore.Identity;
using Utailties;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Myshop.Entities.Models;
using Myshop.DataAccess.DbInitializer;
using System;

namespace Miso.Service
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Register Service
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("Stripe"));
            builder.Services.AddIdentity<ApplicationUsers, IdentityRole>
                (option => option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10)).AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
               .AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUnitOfwork, UnitOfWork>();
            //builder.Services.AddScoped<IDbInitalizer, DbInitalizer>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(); 
            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            #region Configure Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { SD.AdminRole, SD.CustomerRole, SD.EditorRole };
                foreach (var role in roles)
                {
                    if (!roleManger.RoleExistsAsync(role).GetAwaiter().GetResult())
                        roleManger.CreateAsync(new IdentityRole(role));
                }
            }
            using (var scope = app.Services.CreateScope())
            {

                var userManger =
                    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUsers>>();
                string email = "Admin@gmail.com";
                string password = "Test@52@12345";
                if (userManger.FindByEmailAsync(email).GetAwaiter().GetResult() is null)
                {
                    var user = new ApplicationUsers();
                    user.Name = "Adminstrator";
                    user.Email = email;
                    user.UserName = email;
                    user.Address = "Alex";
                    user.City = "Mahod";
                    user.PhoneNumber = "0128533333";
                    user.EmailConfirmed = true;
                    userManger.CreateAsync(user, password).GetAwaiter().GetResult();
                    userManger.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

                }

            }

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();


            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "customer",
                pattern: "{area=customer}/{controller=Home}/{action=Index}/{id?}");
            #endregion

           

            app.Run();
            
      }
        
    }
}
