using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;

namespace BTL_LapTrinhWeb.DAL
{
    public class ProductDetailRepository
    {
        private ProductRepository _repo = new ProductRepository();

        public Products GetProductById(int id)
        {
            return _repo.GetProductByID(id);
        }

        public List<Products> GetSimilarProducts(int categoryId, string gender, int productId)
        {
            return _repo.GetAllProducts()
                .Where(p => p.CategoryID == categoryId &&
                            p.Gender == gender &&
                            p.ProductID != productId)
                .Take(6)
                .ToList();
        }
    }
}