using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.DataAccess.Data;
using System.Security.Claims;
using Utailties;

namespace Miso.Service.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class UserssController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserssController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var clamisidentity = (ClaimsIdentity)User.Identity;
            var claim = clamisidentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;
            return View(_context.ApplicationUsers.Where(x => x.Id != userid).ToList());
        

        }
        public IActionResult LockUnLock(string? id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null | user.LockoutEnd<DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd=DateTime.Now;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Userss", new { area = "Admin" });
        }
    }
    
    
}
