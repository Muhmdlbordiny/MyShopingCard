using Microsoft.EntityFrameworkCore;
using Myshop.DataAccess;
using Myshop.DataAccess.Data;
using Myshop.DataAccess.Implementaion;
using Myshop.Entities.Repositres;
using Microsoft.AspNetCore.Identity;
using Utailties;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Miso.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>
                (option=>option.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromHours(10))
                .AddDefaultTokenProviders()
               .AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUnitOfwork,UnitOfWork>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();


            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "customer",
                pattern: "{area=customer}/{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
