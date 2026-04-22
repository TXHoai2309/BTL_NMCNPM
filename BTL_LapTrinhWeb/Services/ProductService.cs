using BTL_LapTrinhWeb.Models;
using System.Collections.Generic;
using BTL_LapTrinhWeb.BLL;

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

    public decimal GetDisplayPrice(Products product)
    {
        return _manager.GetDiscountPrice(product);
    }
}