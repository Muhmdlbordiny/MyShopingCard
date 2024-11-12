using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositres
{
    public interface IOrderHeaderRepositry:IGenericRepositry<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateOrderStatus(int id,string OrderStatus,string PaymentStatus);
    }
}
