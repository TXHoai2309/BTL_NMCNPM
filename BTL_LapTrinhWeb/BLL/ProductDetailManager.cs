using System;
using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class ProductDetailManager
    {
        private ProductDetailRepository _repo = new ProductDetailRepository();

        // ================= PRODUCT =================
        public Products GetProduct(int id)
        {
            return _repo.GetProductById(id);
        }

        public List<Products> GetSimilarProducts(Products current)
        {
            return _repo.GetSimilarProducts(current);
        }

        // ================= PRICE =================
        public decimal GetDiscountPrice(Products p)
        {
            if (p.Sale > 0)
                return p.Price * (1 - (decimal)p.Sale / 100);

            return p.Price;
        }

        // ================= CART =================
        public List<CartItem> AddToCart(
            List<CartItem> cart,
            Products product,
            int size,
            int quantity)
        {
            var exist = cart.FirstOrDefault(i =>
                i.ProductID == product.ProductID && i.Size == size);

            if (exist != null)
            {
                exist.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Price = product.Price,
                    Sale = product.Sale,
                    Size = size,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            return cart;
        }

        // ================= REVIEW =================
        public List<Review> GetProductReviews(int productId, string username)
        {
            var reviews = _repo.GetAllReviews()
                .Where(r => r.ProductID == productId)
                .ToList();

            foreach (var r in reviews)
            {
                r.CanEdit = username != null && r.Author == username;
            }

            return reviews;
        }

        public bool UserHasBoughtProduct(User user, int productId)
        {
            if (user == null) return false;

            var orders = _repo.GetOrders();

            return orders.Any(o =>
                o.UserName.Equals(user.Name, StringComparison.OrdinalIgnoreCase)
                && o.Items.Any(i => i.ProductID == productId));
        }

        // ================= FAVORITE =================
        public List<string> ToggleFavorite(List<string> favorites, string key)
        {
            if (favorites.Contains(key))
                favorites.Remove(key);
            else
                favorites.Add(key);

            return favorites;
        }
    }
}