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
    public class ProductsRepositry : GenericRepositry<Products>,IProductsRepositry
    {
        private readonly ApplicationDbContext _context;
        public ProductsRepositry(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Products product)
        {
            var ProductsinDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (ProductsinDb != null)
            {
                ProductsinDb.Name = product.Name;
                ProductsinDb.Description = product.Description;
                ProductsinDb.Image = product.Image;
                ProductsinDb.price = product.price;
                ProductsinDb.CategoryId = product.CategoryId;

            }
        }
    }
}
