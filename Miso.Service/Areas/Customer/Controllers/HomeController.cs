using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using System.Security.Claims;
using Utailties;
using X.PagedList.Extensions;

namespace Miso.Service.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        public HomeController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index(int? page )
        {
            var PageNumber = page ??1;
            int PageSize = 8;
            var products = _unitOfwork.Product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int Id )
        {
            
                ShopingCard card = new ShopingCard()
                {

                    ProductId = Id,
                    product =
                   _unitOfwork.Product.GetFirstorDefault(v => v.Id == Id, includeword: "Category"),

                    Count = 1
                };
            
            return View(card);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public IActionResult Details(ShopingCard shopingCard )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        
                        
                        var clamisidentity = (ClaimsIdentity)User.Identity;
                        var claim = clamisidentity.FindFirst(ClaimTypes.NameIdentifier);
                        shopingCard.ApplicationUserId = claim.Value;
                        shopingCard.ProductId = shopingCard.product.Id;
                        shopingCard.product = null!;
                    ShopingCard objcart = _unitOfwork.Shopingcard.GetFirstorDefault(
                        u=>u.ApplicationUserId == claim.Value && u.ProductId == shopingCard.ProductId
                        );
                       
                    if (objcart is null)
                    {
                        _unitOfwork.Shopingcard.Add(shopingCard);
                        _unitOfwork.Complete();

                        HttpContext.Session.SetInt32(
                            SD.SessionKey,
                            _unitOfwork.Shopingcard.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count()
                            );

                    }
                    else
                    {
                        _unitOfwork.Shopingcard.increaseCount(objcart, shopingCard.Count);
                        _unitOfwork.Complete();

                    }




                    
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                

            }

            return RedirectToAction(nameof(Index));
        }

    }
}
