using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using Myshop.Entities.ViewsModels;
using Stripe.Checkout;
using System.Security.Claims;
using Utailties;

namespace Miso.Service.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfwork.Shopingcard.GetAll(u => u.ApplicationUserId == claim.Value,includeword:"product"),
                OrderHeader = new()
            };
            foreach(var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count) * (item.product.price);
            }
            return View(ShoppingCartVM);
        }
        [HttpGet]
		public IActionResult Summary()
		{
            var claimidentity = (ClaimsIdentity)User.Identity;
			var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM = new ShoppingCartVM()
			{
				CartsList = _unitOfwork.Shopingcard.GetAll(u => u.ApplicationUserId == claim.Value, includeword: "product"),
                OrderHeader = new()
			};
            ShoppingCartVM.OrderHeader.ApplicationUsers = _unitOfwork.Applicationuser.GetFirstorDefault(x=>x.Id==claim.Value);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUsers.Name;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUsers.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.City;
			ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.Phone;

			foreach (var item in ShoppingCartVM.CartsList)
			{
				ShoppingCartVM.OrderHeader.TotalPrice += (item.Count) * (item.product.price);
			}
			return View(ShoppingCartVM);
		}
		[HttpPost]
		[ActionName("summary")]
		[ValidateAntiForgeryToken]
		public IActionResult PostSummary(ShoppingCartVM model)
		{
            
                var claimidentity = (ClaimsIdentity)User.Identity;
                var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
                model.CartsList = _unitOfwork.Shopingcard.GetAll(u => u.ApplicationUserId == claim.Value, includeword: "product");
                model.OrderHeader.OrderStatus = SD.Pending;
                model.OrderHeader.PaymentStatus = SD.Pending;
                model.OrderHeader.OrderDate = DateTime.Now;
                model.OrderHeader.ApplicationUsersId = claim.Value;

                foreach (var item in model.CartsList)
                {
                    model.OrderHeader.TotalPrice += (item.Count) * (item.product.price);
                }
                _unitOfwork.OrderHeader.Add(model.OrderHeader);
                _unitOfwork.Complete();
                foreach (var item in model.CartsList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        ProductId = item.ProductId,
                        Price = item.product.price,
                        OrderHeaderId = model.OrderHeader.Id,
                        Count = item.Count
                    };
                    //orderDetail.Product = null;
                    //orderDetail.OrderHeader = null;
                    _unitOfwork.OrderDetail.Add(orderDetail);
                    _unitOfwork.Complete();
                }
            
            var domain = "https://localhost:44373/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={model.OrderHeader.Id}",
                CancelUrl = domain +$"customer/cart/index"
            };
            foreach (var item in model.CartsList) 
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.product.price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.product.Name,
                        },
                    },
                    Quantity = item.Count,

                };
                options.LineItems.Add(sessionlineoption);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            model.OrderHeader.SessionId = session.Id;
            _unitOfwork.Complete();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(301);
		}
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfwork.OrderHeader.GetFirstorDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
			orderHeader.PaymentIntentId = session.PaymentIntentId;

			if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfwork.OrderHeader.UpdateOrderStatus( id, SD.Approve, SD.Approve);
                _unitOfwork.Complete();

            }
            List<ShopingCard> shopingCarts = _unitOfwork.Shopingcard.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUsersId).ToList();
            _unitOfwork.Shopingcard.RemoveRang(shopingCarts);
            _unitOfwork.Complete();
            var count = _unitOfwork.Shopingcard.GetAll(x => x.ApplicationUserId == orderHeader.ApplicationUsersId).ToList().Count() ;

            return View(id);

        }

		public IActionResult Plus(int cartid)
        {
            var shoppingcart = _unitOfwork.Shopingcard.GetFirstorDefault(x => x.ShopingCardId == cartid);
            _unitOfwork.Shopingcard.increaseCount(shoppingcart, 1);
            _unitOfwork.Complete();
            return RedirectToAction("Index");
        }
		public IActionResult Muins(int cartid)
		{
			var shoppingcart = _unitOfwork.Shopingcard.GetFirstorDefault(x => x.ShopingCardId == cartid);
            if (shoppingcart.Count <= 1)
            {
                _unitOfwork.Shopingcard.Remove(shoppingcart);
                var count = _unitOfwork.Shopingcard.GetAll(x=>x.ApplicationUserId == shoppingcart.ApplicationUserId).ToList().Count()-1;
                //_unitOfwork.Complete();
                HttpContext.Session.SetInt32(SD.SessionKey, count);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfwork.Shopingcard.DecreaseCount(shoppingcart, 1);
            }
			_unitOfwork.Complete();
			return RedirectToAction("Index");
		}
		public IActionResult Remove(int cartid)
		{
			var shoppingcart = _unitOfwork.Shopingcard.GetFirstorDefault(x => x.ShopingCardId == cartid);
			_unitOfwork.Shopingcard.Remove(shoppingcart);
			_unitOfwork.Complete();
            var count = _unitOfwork.Shopingcard.GetAll(x => x.ApplicationUserId == shoppingcart.ApplicationUserId).ToList().Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction("Index");
		}

	}
}
