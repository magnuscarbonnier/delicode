// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const AddToCartUrl = 'https://localhost:44385/Cart/Add?Id='
const GetCartUrl = 'https://localhost:44385/Cart/Get'

let CartCounter = 0;
let CartTotal = 0;

function AddToCart(Id) {
    fetch(AddToCartUrl + Id)
        .then(function (res) {
            GetCart()
        })
};

function GetCart() {
    fetch(GetCartUrl)
        .then(response => response.json())
        .then(data => {
            CartCounter = data.totalItemsInCart
            CartTotal = data.total
            document.getElementById('carttotal').innerHTML = CartTotal
            document.getElementById('cartquantity').innerHTML = CartCounter
        }
    )
}