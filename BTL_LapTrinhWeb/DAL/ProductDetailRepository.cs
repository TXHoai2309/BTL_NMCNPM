using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTL_LapTrinhWeb.Models;

namespace BTL_LapTrinhWeb.DAL
{
    public class ProductDetailRepository
    {
        public List<Products> GetAllProducts()
        {
            return HttpContext.Current.Application["AllProducts"] as List<Products>
                   ?? new List<Products>();
        }

        public Products GetProductById(int id)
        {
            return GetAllProducts().FirstOrDefault(p => p.ProductID == id);
        }

        public List<Products> GetSimilarProducts(Products current)
        {
            return GetAllProducts()
                .Where(p => p.Category == current.Category &&
                            p.Gender == current.Gender &&
                            p.ProductID != current.ProductID)
                .ToList();
        }

        public List<Review> GetAllReviews()
        {
            return HttpContext.Current.Application["AllReviews"] as List<Review>
                   ?? new List<Review>();
        }

        public void SaveReviews(List<Review> reviews)
        {
            HttpContext.Current.Application["AllReviews"] = reviews;
        }

        public List<Order> GetOrders()
        {
            return HttpContext.Current.Application["OrderHistory"] as List<Order>
                   ?? new List<Order>();
        }
    }
}