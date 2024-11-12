using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using Myshop.Entities.ViewsModels;
using Stripe;
using Utailties;

namespace Miso.Service.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public OrderController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfwork.OrderHeader.GetAll(includeword: "ApplicationUsers");
            return Json(new { data = orderHeaders });
        }
        public  IActionResult Details(int orderid) 
        {
            OrderViewModel orderVM = new OrderViewModel()
            {
               orderHeader = _unitOfwork.OrderHeader.GetFirstorDefault(u=>u.Id == orderid,includeword: "ApplicationUsers"),
               orderDetail = _unitOfwork.OrderDetail.GetAll(o=>o.OrderHeaderId==orderid,includeword: "Product")
            };
            return View(orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetails()
		{
            var orderdb = _unitOfwork.OrderHeader.GetFirstorDefault(u => u.Id == OrderViewModel.orderHeader.Id);

            orderdb.Name = OrderViewModel.orderHeader.Name;
            orderdb.Phone = OrderViewModel.orderHeader.Phone;
            orderdb.Address = OrderViewModel.orderHeader.Address;
            orderdb.City = OrderViewModel.orderHeader.City;
            if (OrderViewModel.orderHeader.Carrier is not null)
                orderdb.Carrier = OrderViewModel.orderHeader.Carrier;
            if (OrderViewModel.orderHeader.TrackingNumber is not null)
                orderdb.TrackingNumber = OrderViewModel.orderHeader.TrackingNumber;
            _unitOfwork.OrderHeader.Update(orderdb);
            _unitOfwork.Complete();
			TempData["Update"] = "Update Has been successfuly";

			return RedirectToAction("Details", "Order", new {orderid=orderdb.Id});
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProcessing()
		{
            _unitOfwork.OrderHeader.UpdateOrderStatus(OrderViewModel.orderHeader.Id, SD.Processing, null);
            _unitOfwork.Complete();
			
			TempData["Update"] = "OrderStatus Has been Updated successfuly";

			return RedirectToAction("Details", "Order", new { orderid = OrderViewModel.orderHeader.Id });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShipping()
		{
			var orderdb = _unitOfwork.OrderHeader.GetFirstorDefault(u => u.Id == OrderViewModel.orderHeader.Id);
			orderdb.TrackingNumber = OrderViewModel.orderHeader.TrackingNumber;
			orderdb.Carrier = OrderViewModel.orderHeader.Carrier;
            orderdb.OrderStatus = SD.Shipped;
            orderdb.ShippingDate = DateTime.Now;
            _unitOfwork.OrderHeader.Update(orderdb);
            _unitOfwork.Complete();
			

			TempData["Update"] = "OrderStatus Has been Shipped successfuly";

			return RedirectToAction("Details", "Order", new { orderid = OrderViewModel.orderHeader.Id });
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderdb = _unitOfwork.OrderHeader.GetFirstorDefault(u => u.Id == OrderViewModel.orderHeader.Id);

			if(orderdb.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderdb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);
                _unitOfwork.OrderHeader.UpdateOrderStatus(orderdb.Id,SD.Canceled,SD.Refund);
            }
            else
            {
				_unitOfwork.OrderHeader.UpdateOrderStatus(orderdb.Id, SD.Canceled, SD.Canceled);

			}
            _unitOfwork.Complete();
			TempData["Update"] = "Order Has been Canceled successfuly";

			return RedirectToAction("Details", "Order", new { orderid = OrderViewModel.orderHeader.Id });
		}
	}
}
