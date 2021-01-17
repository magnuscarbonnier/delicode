using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliCode.Web.Components
{
    public class OrderComponentBase : ComponentBase
    {
        [Inject]
        ICartService CartService { get; set; }
        [Inject]
        IOrderService OrderService { get; set; }
        [Inject] 
        IProductService ProductService { get; set; }
        [Inject] 
        NavigationManager NavManager { get; set; }

        [Parameter]
        public string UserId { get; set; }

        protected Models.Order orderModel = new Models.Order();
        protected Cart cart = new Cart();

        protected bool isLoading = false;
        protected bool isValidPersonalDetails = false;
        protected bool isDeliverySelected = false;
        protected bool isPaymentSelected = false;
        protected bool isHomeDelivery = false;
        protected bool isHomeDeliveryBooked = false;
        protected string btndesign = "-outline";

        protected KeyValuePair<string, decimal> SelectedDeliverytype;
        protected Dictionary<string, decimal> DeliveryOptions = new Dictionary<string, decimal>();

        protected List<DateTime> AvailableDeliveryDates = new List<DateTime>();

        protected override async Task OnInitializedAsync()
        {
            await GetValidCart();

            InitializeOrderProducts();

            SetDeliveryOptionsAvailable();

            GetAvailableDeliveryDates();
        }

        protected async Task GetValidCart()
        {
            var cartItems = new List<CartItem>();
            var cartResponse = await CartService.GetCart();
            foreach (var cartItem in cartResponse.Items)
            {
                var productResponse = await ProductService.Get(cartItem.Product.Id);
                if (productResponse.Id != Guid.Empty && productResponse.AmountInStorage>=cartItem.Quantity)
                {
                    cartItems.Add(cartItem);
                }
                else if (productResponse.Id != Guid.Empty && productResponse.AmountInStorage < cartItem.Quantity && productResponse.AmountInStorage>0)
                {
                    cartItem.Quantity = productResponse.AmountInStorage;
                    cartItems.Add(cartItem);
                }
            }
            cartResponse.Items = cartItems;

            cart = cartResponse;
        }

        protected void InitializeOrderProducts()
        {
            foreach (var item in cart.Items)
            {
                orderModel.OrderProducts.Add(new OrderProduct { Name = item.Product.Name, Price = item.Product.Price, ProductId = item.Product.Id, Quantity = item.Quantity });
            }
            orderModel.Country = "Sverige";
        }

        protected void SetDeliveryOptionsAvailable()
        {
            DeliveryOptions.Add("Hemleverans", 99);
            DeliveryOptions.Add("Hämta i butik", 0);
        }

        protected void GetAvailableDeliveryDates()
        {
            for (int i = 2; i < 10; i++)
            {
                DateTime date = DateTime.Today.AddDays(i);
                AvailableDeliveryDates.Add(date);
            }
        }

        protected void SavePersonalDetails()
        {
            isValidPersonalDetails = true;
        }

        protected void SelectDelivery(KeyValuePair<string, decimal> selectedDelivery)
        {
            orderModel.ShippingPrice = selectedDelivery.Value;
            SelectedDeliverytype = selectedDelivery;
            isDeliverySelected = true;
            if (selectedDelivery.Key == "Hemleverans")
            {
                isHomeDelivery = true;
            }
            else
            {
                isHomeDelivery = false;
                isHomeDeliveryBooked = false;
                orderModel.BookedDeliveryDate = default;
            }
        }
        protected void SelectDate(DateTime date)
        {
            isHomeDeliveryBooked = true;
            orderModel.BookedDeliveryDate = date;
        }
        protected async Task PlaceOrder()
        {
            isLoading = true;
            var order = orderModel;
            if(UserId != null)
            {
                orderModel.UserId = UserId;
            }
            try
            {
                order = await OrderService.PlaceOrder(orderModel); 
              
                NavManager.NavigateTo($"/admin/editorder?orderid={order.Id}", true);
                isLoading = false;
            }
            catch
            {
                isDeliverySelected = false;
                isValidPersonalDetails = false;
                isHomeDelivery = false;
                isHomeDeliveryBooked = false;
                isPaymentSelected = false;
                isLoading = false;
            }
        }
    }
}
