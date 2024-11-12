using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Repositres;
using Utailties;

namespace Miso.Service.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.AdminRole)]
	public class DashboardController : Controller
	{
		private readonly IUnitOfwork _unitOfwork;

		public DashboardController(IUnitOfwork unitOfwork)
		{
			_unitOfwork = unitOfwork;
		}
		public IActionResult Index()
		{
			ViewBag.Orders = _unitOfwork.OrderHeader.GetAll().Count();
			ViewBag.ApprovedOrders= _unitOfwork.OrderHeader.GetAll(x=>x.OrderStatus == SD.Approve).Count();
			ViewBag.Users = _unitOfwork.Applicationuser.GetAll().Count();
			ViewBag.Products = _unitOfwork.Product.GetAll().Count();
			return View();
		}
	}
}
