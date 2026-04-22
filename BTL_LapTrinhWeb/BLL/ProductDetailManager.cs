using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class ProductDetailManager
    {
        private ProductDetailRepository _repo = new ProductDetailRepository();

        public Products GetProduct(int id)
        {
            return _repo.GetProductById(id);
        }

        public List<Products> GetSimilarProducts(Products current)
        {
            if (current == null) return new List<Products>();

            return _repo.GetSimilarProducts(
                current.CategoryID,
                current.Gender,
                current.ProductID
            );
        }

        public decimal GetDiscountPrice(Products p)
        {
            if (p.SalePercent > 0)
                return p.Price * (1 - p.SalePercent / 100);

            return p.Price;
        }

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