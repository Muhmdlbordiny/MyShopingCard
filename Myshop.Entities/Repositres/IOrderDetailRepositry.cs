using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositres
{
    public interface IOrderDetailRepositry:IGenericRepositry<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
