using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class ProductManager
    {
        private ProductRepository repo = new ProductRepository();

        public List<Products> GetProducts(
            string gender,
            bool isSale,
            string category,
            string searchTerm,
            string priceFilter,
            string sortOption)
        {
            var data = repo.GetAllProducts();

            // ===== Gender =====
            if (!string.IsNullOrEmpty(gender))
                data = data.Where(p => p.Gender == gender).ToList();

            // ===== Sale =====
            if (isSale)
                data = data.Where(p => p.Sale > 0).ToList();

            // ===== Category (FIX CHÍNH) =====
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                int categoryId = 0;

                if (category == "Sneakers") categoryId = 1;
                else if (category == "Lifestyles/Casual") categoryId = 2;
                else if (category == "Sports") categoryId = 3;
                else if (category == "Boots") categoryId = 4;

                data = data.Where(p => p.CategoryID == categoryId).ToList();
            }

            // ===== Search =====
            if (!string.IsNullOrEmpty(searchTerm))
                data = data.Where(p => p.ProductName.ToLower().Contains(searchTerm.ToLower())).ToList();

            // ===== Price Filter =====
            if (priceFilter == "lt100")
                data = data.Where(p => p.Price < 100).ToList();
            else if (priceFilter == "100_500")
                data = data.Where(p => p.Price >= 100 && p.Price <= 500).ToList();
            else if (priceFilter == "gt500")
                data = data.Where(p => p.Price > 500).ToList();

            // ===== Sort =====
            switch (sortOption)
            {
                case "name_asc":
                    data = data.OrderBy(p => p.ProductName).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case "price_asc":
                    data = data.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    data = data.OrderByDescending(p => p.Price).ToList();
                    break;
            }

            return data;
        }

        public decimal GetFinalPrice(Products p)
        {
            if (p.SalePercent > 0)
                return p.Price * (1 - p.SalePercent / 100);

            return p.Price;
        }

        public List<Products> GetAll()
        {
            return repo.GetAllProducts();
        }
        public Products GetByID(int id)
        {
            return repo.GetProductByID(id);
        }

        public void Update(Products p)
        {
            repo.UpdateProduct(p);
        }

        public void Delete(int id)
        {
            repo.DeleteProduct(id);
        }

        public List<Products> Search(string keyword)
        {
            return repo.GetAllProducts()
                .Where(p => p.ProductName.ToLower().Contains(keyword.ToLower()))
                .ToList();
        }
        public void Insert(Products p)
        {
            repo.InsertProduct(p);
        }
    }
}