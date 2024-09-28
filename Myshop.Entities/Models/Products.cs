using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Models
{
    public class Products
    {
        public int Id { get;set;}
        [Required]
        public string Name { get; set; }
        public string Description { get;set; }
        [DisplayName("Upload Image")]
        [ValidateNever]
        public string Image { get;set; }
        [Required]

        public decimal price { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]

        public Category Category { get; set; }

    }
}
