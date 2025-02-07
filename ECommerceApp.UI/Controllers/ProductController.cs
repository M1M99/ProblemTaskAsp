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

        public async Task<IActionResult> Index(int page = 1, int category = 0, bool isAZ = false, bool isHigherToLower = false)
        {
            var items = await _productService.GetAllByCategoryId(category);
            var pageSize = 5;
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
            if (isAZ)
            {
                if (!isHigherToLower)
                {
                    model.Products = model.Products.OrderBy(a => a.ProductName).ToList();
                }
                else { model.Products = model.Products.OrderByDescending(a => a.UnitPrice).ToList(); }
            }
            else
            {
                if (!isHigherToLower)
                {
                    model.Products = model.Products.OrderByDescending(a => a.ProductName).ToList();
                }
                else
                {
                    model.Products =model.Products.OrderBy(a => a.UnitPrice).ToList();
                }
            }
            if (isHigherToLower)
            {
                if (!isAZ)
                {
                    model.Products = model.Products.OrderByDescending(a => a.UnitPrice).ToList();
                }
                else { model.Products = model.Products.OrderBy(a => a.ProductName).ToList(); }

            }
            else
            {
                if (!isAZ)
                {
                    model.Products = model.Products.OrderBy(a => a.ProductName).ToList();
                }
                else
                {
                    model.Products = model.Products.OrderByDescending( a => a.ProductName).ToList();
                }
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
