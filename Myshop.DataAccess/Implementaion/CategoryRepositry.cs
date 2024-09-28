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
    public class CategoryRepositry : GenericRepositry<Category>,ICategoryRepositry
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepositry(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var CategoryinDb = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (CategoryinDb != null)
            {
                CategoryinDb.Name = category.Name;
                CategoryinDb.Description = category.Description;
                CategoryinDb.CreatedTime = DateTime.Now;
            }
        }
    }
}
