using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Models
{
    public class ShopingCard
    {
        [Key]
        public int ShopingCardId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Products product { get; set; }
        [Range(1, 100, ErrorMessage = "Must be Range 1 to 100")]
        public int Count { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]

        public ApplicationUsers ApplicationUsers { get; set; }
    }
}
