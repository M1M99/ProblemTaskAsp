using ECommerce.Entities.Models;
using ECommerceApp.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        private static string lastHigherValue = "";
        private static string lastAzValue = "";

        public async Task<IActionResult> Index(int page = 1, int category = 0, bool isAZ = false, bool isHigherToLower = false)
        {
            var items = await _productService.GetAllByCategoryId(category);
            var pageSize = 10;
            var model = new ProductListViewModel
            {
                Products = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(items.Count / (decimal)pageSize),
                CurrentCategory = category,
                isAz = isAZ,
                isHigherToLower = isHigherToLower
            };

            if (isHigherToLower.ToString() != lastHigherValue)
            {
                if (!isHigherToLower)
                {
                    model.Products = model.Products.OrderBy(a => a.UnitPrice).ToList();
                }
                else { model.Products = model.Products.OrderByDescending(a => a.UnitPrice).ToList(); }
                lastHigherValue = isHigherToLower.ToString();
            }

            if (isAZ.ToString() != lastAzValue)
            {
                if (!isAZ)
                {
                    model.Products = model.Products.OrderBy(a => a.ProductName).ToList();
                }
                else { model.Products = model.Products.OrderByDescending(a => a.ProductName).ToList(); }
                lastAzValue = isAZ.ToString();
            }

            return View(model);
        }

        public async Task<IActionResult> Index2()
        {
            var items = await _productService.GetAllAsync();
            var model = new ProductListViewModel
            {
                Products = items,
            };
            model.Products = model.Products.OrderBy(a => a.UnitPrice).ToList();
            return Ok(model.Products.Select(a => a.UnitPrice));
        }
    }
}
