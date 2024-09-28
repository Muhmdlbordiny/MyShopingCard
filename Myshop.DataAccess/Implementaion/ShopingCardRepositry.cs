using Myshop.DataAccess.Data;
using Myshop.Entities.Models;
using Myshop.Entities.Repositres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.DataAccess.Implementaion
{
    public class ShopingCardRepositry : GenericRepositry<ShopingCard>,IShopingCardRepositry
    {
        private readonly ApplicationDbContext _context;
        public ShopingCardRepositry(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       
    }
}
