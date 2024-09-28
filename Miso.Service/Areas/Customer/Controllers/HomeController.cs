using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using System.Security.Claims;

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
        public IActionResult Index()
        {
            var products = _unitOfwork.Product.GetAll();
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int id )
        {

            ShopingCard card = new ShopingCard()
            {

                ProductId = id,
                product =
               _unitOfwork.Product.GetFirstorDefault(v => v.Id == id, includeword: "Category"),

                Count = 1
            };
            return View(card);
            



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ShopingCard shopingCard )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                        
                        
                        var clamisidentity = (ClaimsIdentity)User.Identity;
                        var claim = clamisidentity.FindFirst(ClaimTypes.NameIdentifier);
                        shopingCard.ApplicationUserId = claim.Value;

                        _unitOfwork.Shopingcard.Add(shopingCard);


                        _unitOfwork.Complete();
                    
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
