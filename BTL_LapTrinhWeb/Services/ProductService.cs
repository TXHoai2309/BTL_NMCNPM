using System.Collections.Generic;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb.Services
{
    public class ProductService
    {
        private ProductManager _manager = new ProductManager();

        public List<Products> GetProducts(
            string gender,
            bool isSale,
            string category,
            string searchTerm,
            string priceFilter,
            string sortOption)
        {
            return _manager.GetProducts(
                gender,
                isSale,
                category,
                searchTerm,
                priceFilter,
                sortOption
            );
        }

        public Products GetByID(int id)
        {
            return _manager.GetByID(id);
        }

        public void UpdateProduct(Products p)
        {
            _manager.Update(p);
        }

        public void DeleteProduct(int id)
        {
            _manager.Delete(id);
        }

        public List<Products> Search(string keyword)
        {
            return _manager.Search(keyword);
        }

        public decimal GetDisplayPrice(Products product)
        {
            return _manager.GetFinalPrice(product);
        }

        public string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";

            return text.Length > maxLength
                ? text.Substring(0, maxLength) + "..."
                : text;
        }
    }
}