// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const AddToCartUrl = 'https://localhost:44385/Product/AddToCart?Id='

function AddToCart(Id) {
    fetch(AddToCartUrl + Id)
        .then(function (res) {
            
        })
};