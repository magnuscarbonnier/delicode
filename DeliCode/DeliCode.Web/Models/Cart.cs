using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliCode.Web.Models
{
    public class Cart
    {
        public Guid SessionId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public Decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var product in Items)
                {
                    total += product.Total;
                }
                return total;
            }
        }
        public int TotalItemsInCart
        {
            get
            {
                return Items.Sum(x => x.Quantity);
            }
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Total
        {
            get
            {
                return Product.Price * Quantity;
            }   
        }
    }
}
