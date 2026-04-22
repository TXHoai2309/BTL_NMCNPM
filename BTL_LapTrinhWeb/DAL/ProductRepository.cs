using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_LapTrinhWeb.DAL
{
    public class ProductRepository
    {
        // 🔥 CHỈ GIỮ 1 METHOD DUY NHẤT
        public List<Products> GetAllProducts()
        {
            var products = System.Web.HttpContext.Current.Application["AllProducts"] as List<Products>;
            return products ?? new List<Products>();
        }

        public void Save(List<Products> products)
        {
            HttpContext.Current.Application["AllProducts"] = products;
        }
    }
}