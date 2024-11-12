using Microsoft.AspNetCore.Mvc;
using Myshop.Entities.Repositres;
using System.Security.Claims;
using Utailties;

namespace Miso.Service.ViewComponents
{

    public class CartViewComponent:ViewComponent
    {
        private readonly IUnitOfwork _unitOfwork;

        public CartViewComponent(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //user Login
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
                else
                {
                    HttpContext.Session.SetInt32
                        (
                        SD.SessionKey,
                        _unitOfwork.Shopingcard.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count()
                        );
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));

                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View( 0);
            } 
        }
    }
}
