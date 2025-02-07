using ECommerce.Entities.Concrete;
using ECommerce.Entities.Models;
using ECommerceApp.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Business.Concrete
{
    public class CartService : ICartService
    {
        public void AddToCart(Cart cart, Product product)
        {
            CartLine cartLine = cart.CartLines.FirstOrDefault(c => c.Product.ProductId == product.ProductId);
            if (cartLine != null)
            {
                cartLine.Quantity++;
            }
            else
            {
                cart.CartLines.Add(new CartLine { Product = product, Quantity = 1 });
            }
        }

        public void Decrement(Cart cart, int productId)
        {
            var prod = cart.CartLines.FirstOrDefault(a => a.Product.ProductId == productId);
            if (prod is not null)
            {
                if (prod.Quantity > 1)
                {
                    prod.Quantity--;
                }
                else
                {
                    RemoveFromCart(cart, productId);
                }
            }
            else
            {
                throw new Exception(message:"Product Not Found"); 
            }
        }


        public void Increment(Cart cart, int productId)
        {
            var prod = cart.CartLines.FirstOrDefault(a => a.Product.ProductId == productId);
            if (prod != null)
            {
                prod.Quantity++;
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }

        public List<CartLine>? List(Cart cart)
        {
            return cart.CartLines;
        }

        public void RemoveFromCart(Cart cart, int productId)
        {
            var cartLine = cart.CartLines.FirstOrDefault(c => c.Product.ProductId == productId);
            if (cartLine != null)
            {
                cart.CartLines.Remove(cartLine);
            }
            else
            {
                throw new Exception("Product not Found");
            }
        }

    }
}
