using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Myshop.DataAccess.Data;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using Myshop.Entities.ViewsModels;
using System.Security.Claims;
using Utailties;

namespace Miso.Service.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UserssController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUsers> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserssController(ApplicationDbContext context, UserManager<ApplicationUsers> usermanager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usermanager = usermanager;
            _roleManager = roleManager;
            //_context = context;
        }
        public  IActionResult Index()
        {
            //var clamisidentity = (ClaimsIdentity)User.Identity;
            //var claim = clamisidentity.FindFirst(ClaimTypes.NameIdentifier);
            //string userid = claim.Value;
            //return View(_context.ApplicationUsers.Where(x => x.Id != userid).ToList());
            var users =  _usermanager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                UserName = u.UserName ?? "Invaild User Name",
                DisplayName = u.Name ?? "invaild Display Name",
                PhoneNumber = u.PhoneNumber ?? "Invaild Phone Number !",
                Email = u.Email ?? "Invaild Email!!",
                LockoutEnabled = u.LockoutEnabled,
                LockoutEnd = u.LockoutEnd,

                Roles = _usermanager.GetRolesAsync(u).Result

            }).ToList();
            return View(users);

        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            var allroles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRoleViewModel()
            {
                UserId = user?.Id ?? "Invaild UserId",
                UserName = user?.UserName ?? "Invaild User Name",
                Roles = allroles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r?.Name ?? "Invaild Role Name",
                    IsSelected =  _usermanager.IsInRoleAsync(user, r?.Name ?? "Invaild Role Name").Result
                }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            var user = await _usermanager.FindByIdAsync(model.UserId);
            var userRoles = await _usermanager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                {
                    await _usermanager.RemoveFromRoleAsync(user, role.Name);
                }
                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                {
                    await _usermanager.AddToRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("Index");
        }
        public  async Task <IActionResult> LockUnLock(string? id)
        {
            //var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}
            //if (user.LockoutEnabled == null || user.LockoutEnabled < DateTime.Now)
            //{

            //}

            
                var user = await _usermanager.FindByIdAsync(id);
                var userview = new UserViewModel()
                 {
                     Id = user.Id,
                     LockoutEnabled = user.LockoutEnabled,
                     LockoutEnd = user.LockoutEnd
                 };
                if (user == null)
                {
                    return NotFound();
                }
                if (user.LockoutEnd == null | user.LockoutEnd < DateTime.Now)
                {
                    user.LockoutEnd = DateTime.Now.AddYears(1);
                }
                else
                {
                    user.LockoutEnd = DateTime.Now;
                }

                var up = await _usermanager.UpdateAsync(user);
                if (up.Succeeded)
                {
                    await _context.SaveChangesAsync();

                }
            
            return RedirectToAction("Index", "Userss", new { area = "Admin" });
        }
    }


}
