using DeliCode.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliCode.Web.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? BookedDeliveryDate { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Du måste fylla i epost")]
        [EmailAddress(ErrorMessage = "Ogiltig epostadress")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Du måste fylla i förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Du måste fylla i efternamn")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "Du måste fylla i adress")]

        public string Address { get; set; }
        [Required(ErrorMessage = "Du måste fylla i postnummer")]

        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Du måste fylla i stad")]

        public string City { get; set; }
        [Required(ErrorMessage = "Du måste fylla i land")]

        public string Country { get; set; }

        public string Phone { get; set; }
        public string ShippingNotes { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal TotalPrice()
        {
            var sum=ShippingPrice+ OrderProducts.Sum(x => x.Price * x.Quantity);
            return sum;
        }

        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
