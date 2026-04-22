using System;
using System.Collections.Generic;
using System.Linq;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.DAL;

namespace BTL_LapTrinhWeb.BLL
{
    public class ProductManager
    {
        private ProductRepository _repo = new ProductRepository();

        public List<Products> GetProducts(
            string gender,
            bool isSale,
            string category,
            string searchTerm,
            string priceFilter,
            string sortOption)
        {
            var products = _repo.GetAllProducts().AsEnumerable();

            if (!string.IsNullOrEmpty(gender))
            {
                products = products.Where(p =>
                    p.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
            }

            if (isSale)
            {
                products = products.Where(p => p.Sale > 0);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string term = searchTerm.ToLower();
                products = products.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Category.ToLower().Contains(term) ||
                    p.Gender.ToLower().Contains(term));
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                products = products.Where(p => p.Category == category);
            }

            switch (priceFilter)
            {
                case "lt100":
                    products = products.Where(p => p.Price < 100);
                    break;
                case "100_500":
                    products = products.Where(p => p.Price >= 100 && p.Price <= 500);
                    break;
                case "gt500":
                    products = products.Where(p => p.Price > 500);
                    break;
            }

            switch (sortOption)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
            }

            return products.ToList();
        }

        public decimal GetDiscountPrice(Products p)
        {
            if (p.Sale > 0)
            {
                return p.Price * (1 - (decimal)p.Sale / 100);
            }

            return p.Price;
        }
    }
}