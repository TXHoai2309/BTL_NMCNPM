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

    public void UpdateProduct(Products p)
    {
        _manager.Update(p);
    }

    public void DeleteProduct(int id)
    {
        _manager.Delete(id);
    }

    public string Truncate(string text, int maxLength)
    {
        return _manager.Truncate(text, maxLength);
    }

    public List<Products> Search(string keyword)
    {
        return _manager.Search(keyword);
    }

    public decimal GetDisplayPrice(Products product)
    {
        return _manager.GetDiscountPrice(product);
    }
}