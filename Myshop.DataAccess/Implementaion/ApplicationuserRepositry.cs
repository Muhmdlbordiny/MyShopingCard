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
    public class ApplicationuserRepositry : GenericRepositry<ApplicationUsers>, IApplicationUserRepositry
	{
        private readonly ApplicationDbContext _context;
        public ApplicationuserRepositry(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
       
    }
}
