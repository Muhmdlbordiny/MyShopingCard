using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositres
{
    public interface IUnitOfwork:IDisposable
    {
        ICategoryRepositry Category { get; }
        IProductsRepositry Product { get; }
        IShopingCardRepositry Shopingcard {  get; }
        public int Complete();
    }
}
