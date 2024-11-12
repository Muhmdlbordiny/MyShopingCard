using Myshop.DataAccess.Data;
using Myshop.Entities.Repositres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.DataAccess.Implementaion
{
    public class UnitOfWork : IUnitOfwork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Category = new CategoryRepositry(context);
            Product = new ProductsRepositry(context);
            Shopingcard = new ShopingCardRepositry(context);
			OrderDetail = new OrderDetailRepositry(context);
            OrderHeader = new OrderHeaderRepositry(context);
            Applicationuser = new ApplicationuserRepositry(context);


		}
        public ICategoryRepositry Category { get; private set; }

        public IProductsRepositry Product { get; private set; }
        public IShopingCardRepositry Shopingcard { get; private set; }

		public IOrderDetailRepositry OrderDetail { get; private set; }

		public IOrderHeaderRepositry OrderHeader { get; private set; }
        public IApplicationUserRepositry Applicationuser { get; private set; }


		public int Complete()
        {
          return  _context.SaveChanges();

        }

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
