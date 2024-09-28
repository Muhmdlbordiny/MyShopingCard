using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.ViewsModels
{
    public class ProductVM
    {
        public Products product { get; set; }
        [ValidateNever]

        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
