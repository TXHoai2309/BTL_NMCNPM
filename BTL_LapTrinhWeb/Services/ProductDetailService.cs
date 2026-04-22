using System;
using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class ProductDetailService
    {
        private ProductDetailManager _manager = new ProductDetailManager();

        // ================= PRODUCT =================
        public Products GetProduct(int id)
        {
            return _manager.GetProduct(id);
        }

        public List<Products> GetSimilarProducts(Products current)
        {
            return _manager.GetSimilarProducts(current);
        }

        // ================= PRICE =================
        public decimal GetDisplayPrice(Products product)
        {
            return _manager.GetDiscountPrice(product);
        }

        // ================= CART =================
        public List<CartItem> AddToCart(
            List<CartItem> cart,
            Products product,
            int size,
            int quantity)
        {
            return _manager.AddToCart(cart, product, size, quantity);
        }

        // ================= REVIEW =================
        public List<Review> GetReviews(int productId, string username)
        {
            return _manager.GetProductReviews(productId, username);
        }

        public bool UserHasBoughtProduct(User user, int productId)
        {
            return _manager.UserHasBoughtProduct(user, productId);
        }

        // ================= FAVORITE =================
        public List<string> ToggleFavorite(List<string> favorites, string key)
        {
            return _manager.ToggleFavorite(favorites, key);
        }
    }
}