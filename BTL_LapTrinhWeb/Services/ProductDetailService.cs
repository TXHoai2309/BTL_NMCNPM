using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class ProductDetailService
    {
        private ProductDetailManager _manager = new ProductDetailManager();

        public Products GetProduct(int id)
        {
            return _manager.GetProduct(id);
        }

        public List<Products> GetSimilarProducts(Products current)
        {
            return _manager.GetSimilarProducts(current);
        }
        public decimal GetDisplayPrice(Products product)
        {
            return _manager.GetDiscountPrice(product);
        }

        public List<string> ToggleFavorite(List<string> favorites, string key)
        {
            return _manager.ToggleFavorite(favorites, key);
        }
    }
}