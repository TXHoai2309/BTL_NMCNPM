using System;
using System.Collections.Generic;

namespace BTL_LapTrinhWeb.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Name
        {
            get { return ProductName; }
            set { ProductName = value; }
        }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string ImageUrl
        {
            get { return Image; }
            set { Image = value; }
        }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ProductUrl { get; set; }
        public string Gender { get; set; }
        public decimal SalePercent { get; set; }
        public int Sale
        {
            get { return (int)SalePercent; }
            set { SalePercent = value; }
        }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string ImageBack
        {
            get { return Image1; }
            set { Image1 = value; }
        }

        public string ImageLeft
        {
            get { return Image2; }
            set { Image2 = value; }
        }

        public string ImageTop
        {
            get { return Image3; }
            set { Image3 = value; }
        }

        public string ImageUnder
        {
            get { return Image4; }
            set { Image4 = value; }
        }
        public string Size { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<Review> Reviews { get; set; }

        public Products()
        {
            Reviews = new List<Review>();
        }
    }
}