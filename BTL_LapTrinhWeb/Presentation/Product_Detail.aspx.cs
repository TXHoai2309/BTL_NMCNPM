using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb
{
    public partial class Product_Detail : System.Web.UI.Page
    {
        private ProductManager _pm = new ProductManager();
        private CartManager _cart = new CartManager();
        private ProductDetailManager _pdm = new ProductDetailManager();

        private int ProductID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProduct();
            }
        }

        private void LoadProduct()
        {
            var product = _pm.GetByID(ProductID);

            if (product == null)
            {
                Response.Redirect("~/Presentation/Trangchu.aspx");
                return;
            }

            product_name.InnerText = product.ProductName;
            product_description.InnerText = product.Description;

            decimal finalPrice = _pm.GetDiscountPrice(product);

            if (product.SalePercent > 0)
            {
                product_price.InnerHtml =
                    $"<span class='old-price'>${product.Price:F2}</span> " +
                    $"<span class='new-price'>${finalPrice:F2}</span>";
            }
            else
            {
                product_price.InnerText = $"${product.Price:F2}";
            }

            main_image.Src = ResolveUrl(product.Image ?? "~/Assets/images/no-image.png");

            imgThumbnailMain.Src = ResolveUrl(product.Image ?? "~/Assets/images/no-image.png");
            imgThumbnailBack.Src = ResolveUrl(product.Image1 ?? product.Image);
            imgThumbnailLeft.Src = ResolveUrl(product.Image2 ?? product.Image);
            imgThumbnailTop.Src = ResolveUrl(product.Image3 ?? product.Image);
            imgThumbnailUnder.Src = ResolveUrl(product.Image4 ?? product.Image);

            btnAddToBag.CommandArgument = product.ProductID.ToString();
            btnBuyAndCheckOut.CommandArgument = product.ProductID.ToString();

            LoadSimilarProducts(product);
        }

        private void LoadSimilarProducts(Products product)
        {
            var similar = _pdm.GetSimilarProducts(product);

            rptSimilarProducts.DataSource = similar;
            rptSimilarProducts.DataBind();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (!(Session["UserLoggedIn"] is User user))
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
                return;
            }

            int productId = int.Parse(((Button)sender).CommandArgument);

            _cart.Add(user.Id, productId, 0, 1);

            Response.Redirect("~/Presentation/Cart.aspx");
        }

        protected void btnFavorite_Click(object sender, EventArgs e)
        {
            string productId = ((Button)sender).CommandArgument;

            var favorites = Session["Favorites"] as List<string> ?? new List<string>();

            string key = $"Product-{productId}";

            if (favorites.Contains(key))
                favorites.Remove(key);
            else
                favorites.Add(key);

            Session["Favorites"] = favorites;
        }

        public bool IsFavorite(string productId)
        {
            var favorites = Session["Favorites"] as List<string> ?? new List<string>();
            return favorites.Contains($"Product-{productId}");
        }

        public string GetPriceDisplay(object dataItem)
        {
            decimal price = (decimal)DataBinder.Eval(dataItem, "Price");
            decimal sale = DataBinder.Eval(dataItem, "SalePercent") != DBNull.Value
                ? Convert.ToDecimal(DataBinder.Eval(dataItem, "SalePercent"))
                : 0;

            decimal finalPrice = sale > 0
                ? price * (1 - sale / 100)
                : price;

            if (sale > 0)
            {
                return $@"
                <span class='old-price'>${price:F2}</span>
                <span class='new-price'>${finalPrice:F2}</span>";
            }

            return $"${price:F2}";
        }

        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
        }

        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Favorites.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
        }
    }
}