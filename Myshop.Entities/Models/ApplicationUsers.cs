using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Models
{
    public class ApplicationUsers:IdentityUser
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // public Guid userId { get; set; }
        [Required]
        public string Name {  get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
