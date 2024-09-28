using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Repositres
{
    public interface IProductsRepositry:IGenericRepositry<Products>
    {
        void Update(Products products);
    }
}
