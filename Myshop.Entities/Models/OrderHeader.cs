using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myshop.Entities.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }
		public string ApplicationUsersId { get; set; }
		[ValidateNever]
		public ApplicationUsers ApplicationUsers { get; set; }
		public DateTime OrderDate { get; set; } 
		public DateTime ShippingDate { get; set; } 
		public decimal TotalPrice { get; set; }
		public string? OrderStatus { get; set; }
		public string? PaymentStatus { get; set; }
		public string? TrackingNumber { get; set; }
		public string? Carrier { get; set; }
		public DateTime PaymentDate { get; set; }
		//stripe
		public string? SessionId { get; set; }
		public string? PaymentIntentId { get; set; }
		//Data of User
		[Required(ErrorMessage = "Must  Name  is Required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Must  Address  is Required")]
		public string Address { get; set; }
		[Required(ErrorMessage ="Must  City  is Required")]
		public string City { get; set; }
		[Required(ErrorMessage ="Must Phone is Required ")]
		public string Phone { get; set; }
	}
}
