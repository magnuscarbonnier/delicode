using DeliCode.Web.Models;
using DeliCode.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        IInventoryService InventoryService { get; set; }
        [Inject]
        IProductService ProductService { get; set; }
        [Inject]
        NavigationManager NavManager { get; set; }

        [Parameter]
        public string UserId { get; set; }

        [CascadingParameter]
        protected EditContext CurrentEditContext { get; set; }
 
        protected Models.Order orderModel = new Models.Order();
        protected Cart cart = new Cart();

        protected bool isLoading = false;
        protected bool renderSelectDelivery = false;
        protected bool isHomeDeliveryBooked = false;
        protected bool renderPersonalDetails = false;
        protected bool renderDeliveryOptions = true;
        protected bool renderPayment = false;
        protected bool renderHomeDelivery = false;
        protected bool renderOrderButton = false;
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
            CurrentEditContext = new EditContext(orderModel);
        }

        protected async Task GetValidCart()
        {
            cart = await CartService.GetCart();
            if (cart.Items.Count > 0)
                renderSelectDelivery = true;
            else
                renderSelectDelivery = false;
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
            DeliveryOptions.Add(DeliveryAlternatives.hemLeverans, 99);
            DeliveryOptions.Add(DeliveryAlternatives.upphämtning, 0);
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
            bool isOrderModelValid = CurrentEditContext.Validate(); 
            if (isOrderModelValid)
            {
                renderPersonalDetails = false;
                renderPayment = true;
            }
            else
            {
                renderPersonalDetails = true;
                renderPayment = false;
            }
            renderOrderButton = false;
        }

        protected void SelectDelivery(KeyValuePair<string, decimal> selectedDelivery)
        {
            orderModel.ShippingPrice = selectedDelivery.Value;
            SelectedDeliverytype = selectedDelivery;
            
            if (selectedDelivery.Key == DeliveryAlternatives.hemLeverans)
            {
                renderHomeDelivery = true;
                isHomeDeliveryBooked = true;
                renderPersonalDetails = false;
            }
            else
            {
                renderHomeDelivery = false;
                isHomeDeliveryBooked = false;
                renderPersonalDetails = true;
                orderModel.BookedDeliveryDate = default;
            }
            
            renderPayment = false;
            renderOrderButton = false;
        }
        protected void SelectDate(DateTime date)
        {
            isHomeDeliveryBooked = true;
            renderPersonalDetails = true;
            orderModel.BookedDeliveryDate = date;
            renderOrderButton = false;
            renderPayment = false;
            renderHomeDelivery = false;
        }
        protected async Task PlaceOrder()
        {
            isLoading = true;
            renderDeliveryOptions = false;
            renderHomeDelivery = false;
            renderOrderButton = false;
            renderPayment = false;
            renderPersonalDetails = false;

            var order = orderModel;
            if (UserId != null)
            {
                orderModel.UserId = UserId;
            }
            try
            {
                order = await InventoryService.FinalizeOrder(orderModel);
                NavManager.NavigateTo($"/order/confirmorder?orderid={order.Id}", true);
            }
            catch
            {
                isLoading = false;
            }
        }
    }
}
