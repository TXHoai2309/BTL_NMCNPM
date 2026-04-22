using System;
using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class FavoritesManager
    {
        private ProductRepository _repo = new ProductRepository();

        public List<object> GetFavoriteProducts(List<string> favoriteIds)
        {
            var allProducts = _repo.GetAllProducts();

            var result = new List<object>();

            foreach (var uniqueId in favoriteIds)
            {
                var parts = uniqueId.Split('-');
                if (parts.Length != 2) continue;

                string gender = parts[0];
                int id = int.Parse(parts[1]);

                var product = allProducts.FirstOrDefault(p =>
                    p.ProductID == id &&
                    p.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

                if (product != null)
                {
                    result.Add(new
                    {
                        product.ProductID,
                        product.Name,
                        product.Price,
                        product.ImageUrl,
                        product.Sale,
                        uniqueId
                    });
                }
            }

            return result;
        }

        public void Remove(string uniqueId, List<string> favoriteIds)
        {
            if (favoriteIds.Contains(uniqueId))
            {
                favoriteIds.Remove(uniqueId);
            }
        }

        public decimal GetDiscountPrice(decimal price, int sale)
        {
            return sale > 0 ? price * (1 - (decimal)sale / 100) : price;
        }
    }
}