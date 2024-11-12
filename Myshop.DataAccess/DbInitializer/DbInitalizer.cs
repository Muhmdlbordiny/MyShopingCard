using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Myshop.DataAccess.Data;
using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utailties;


namespace Myshop.DataAccess.DbInitializer
{
    public class DbInitalizer : IDbInitalizer
    {
        private readonly UserManager<ApplicationUsers> _userManager;
       
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitalizer(ApplicationDbContext context,UserManager<ApplicationUsers> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public void Initializer()
        {
            //Migrate
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

            }
            catch(Exception )
            {
                throw;
            }
            //Roles
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult()|| _roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult()== null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

                //Users
                _userManager.CreateAsync(new ApplicationUsers
                {
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    Name ="Adminstritor",
                    PhoneNumber ="01276738534",
                    Address ="Alex",
                    City="Elmahmodia",
                    EmailConfirmed = true
                    
                },"P@$$word@12345").GetAwaiter().GetResult();
                ApplicationUsers user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
