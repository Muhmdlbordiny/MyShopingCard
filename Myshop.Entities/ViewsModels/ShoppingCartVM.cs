using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.ViewsModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShopingCard> CartsList { get; set; }
        public decimal TotalCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
	}
}
