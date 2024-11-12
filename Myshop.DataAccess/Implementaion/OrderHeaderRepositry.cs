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
	public class OrderHeaderRepositry : GenericRepositry<OrderHeader>, IOrderHeaderRepositry
	{
		private readonly ApplicationDbContext _context;

		public OrderHeaderRepositry(ApplicationDbContext context):base(context)
		{
			_context = context;
		}
		public void Update(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
		}

		public void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus)
		{
			var orderfromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if(orderfromDb != null)
			{
				orderfromDb.OrderStatus = OrderStatus;
				orderfromDb.PaymentDate = DateTime.Now;
				if (PaymentStatus != null)
				{
					orderfromDb.PaymentStatus = PaymentStatus;
				}

				
				
			}
		}
	}
}
