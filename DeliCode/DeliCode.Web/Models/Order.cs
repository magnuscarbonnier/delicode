using DeliCode.Library.Models;
using System;
using System.Collections.Generic;
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
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ShippingNotes { get; set; }
        public bool IsTestOrder { get; set; }
        public decimal TotalPrice()
        {
            return OrderProducts.Sum(x => x.Price * x.Quantity);
        }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
