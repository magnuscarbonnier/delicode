using System;

namespace DeliCode.Web.Models
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
    }
}
