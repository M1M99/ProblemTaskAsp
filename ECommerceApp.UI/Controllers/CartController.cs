using ECommerce.Entities.Concrete;
using ECommerceApp.Business.Abstract;
using ECommerceApp.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartSessionService _cartSessionService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartSessionService cartSessionService, ICartService cartService, IProductService productService)
        {
            _cartSessionService = cartSessionService;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> AddToCart(int productId, int page, int category)
        {
            var productToBeAdded = await _productService.GetByIdAsync(productId);
            var cart = _cartSessionService.GetCart();

            _cartService.AddToCart(cart, productToBeAdded);
            _cartSessionService.SetCart(cart);

            TempData.Add("message", $"Your Product , {productToBeAdded.ProductName} was added successfully.");

            return RedirectToAction("Index", "Product", new { page = page, category = category });
        }

        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var cart = _cartSessionService.GetCart();
                _cartService.RemoveFromCart(cart, productId);
                _cartSessionService.SetCart(cart);
                TempData["message"] = "Product Removed From Cart.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        public async Task<IActionResult> Decrement(int productId)
        {
            try
            {
                var cart = _cartSessionService.GetCart();
                _cartService.Decrement(cart, productId);
                _cartSessionService.SetCart(cart);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("List");
            }
        }

        public async Task<IActionResult> Increment(int productId)
        {
            try
            {
                var cart = _cartSessionService.GetCart();
                _cartService.Increment(cart, productId);
                _cartSessionService.SetCart(cart);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return RedirectToAction("List");
            }
        }


        public IActionResult List()
        {
            var cart = _cartSessionService.GetCart();
            var model = new CartListViewModel
            {
                Cart = cart
            };
            return View(model);
        }
    }
}
