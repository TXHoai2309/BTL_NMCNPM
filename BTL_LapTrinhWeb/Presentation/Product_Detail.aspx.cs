using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb
{
    public partial class Product_Detail : System.Web.UI.Page
    {
        private ProductManager _pm = new ProductManager();
        private CartManager _cart = new CartManager();
        private ProductDetailManager _pdm = new ProductDetailManager();

        private int ProductID => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { LoadProduct(); }
        }

        private void LoadProduct()
        {
            var product = _pm.GetByID(ProductID);
            if (product == null) { Response.Redirect("~/Presentation/Trangchu.aspx"); return; }

            // Đổ dữ liệu vào các control có runat="server"
            product_name.InnerText = product.ProductName;
            product_description.InnerText = product.Description;
            review_product_name.InnerText = product.ProductName;

            decimal finalPrice = _pm.GetFinalPrice(product);
            if (product.SalePercent > 0)
            {
                product_price.InnerHtml = $"<span class='old-price'>${product.Price:F2}</span> <span class='new-price'>${finalPrice:F2}</span>";
                product_sale_percent.Visible = true;
                product_sale_percent.InnerText = $"-{product.SalePercent}%";
            }
            else { product_price.InnerText = $"${product.Price:F2}"; }
            btnFavoriteIcon.ImageUrl = IsFavorite(product.ProductID.ToString())
                ? "~/Assets/images/Chung/icon_favorite_green.png"
                : "~/Assets/images/Chung/icon_favorite.png";
            btnFavoriteIcon.CommandArgument = product.ProductID.ToString();
            main_image.Src = ResolveUrl(product.Image ?? "~/Assets/images/no-image.png");
            imgThumbnailMain.Src = main_image.Src;
            imgThumbnailBack.Src = ResolveUrl(product.Image1 ?? product.Image);
            imgThumbnailLeft.Src = ResolveUrl(product.Image2 ?? product.Image);
            imgThumbnailTop.Src = ResolveUrl(product.Image3 ?? product.Image);
            imgThumbnailUnder.Src = ResolveUrl(product.Image4 ?? product.Image);

            LoadSimilarProducts(product);
        }

        private void LoadSimilarProducts(Products product)
        {
            rptSimilarProducts.DataSource = _pdm.GetSimilarProducts(product);
            rptSimilarProducts.DataBind();
        }

        // Sửa lỗi CS1061: Thiếu định nghĩa hàm Yêu thích
        protected void btnAddToFavorites_Click(object sender, ImageClickEventArgs e)
        {
            string id = ((ImageButton)sender).CommandArgument;
            var favorites = Session["Favorites"] as List<string> ?? new List<string>();
            string key = $"Product-{id}";

            if (favorites.Contains(key)) favorites.Remove(key);
            else favorites.Add(key);

            Session["Favorites"] = favorites;
            LoadProduct();
        }

        // Sửa lỗi CS1061: Thiếu định nghĩa hàm Cancel
        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlAddReview.Visible = false;
        }

        protected void btnSubmitReview_Click(object sender, EventArgs e) { /* Logic gửi review */ }
        protected void BuyAndCheckOut_Click(object sender, EventArgs e) { /* Logic thanh toán */ }
        protected void AddToBag_Click(object sender, EventArgs e) { /* Logic thêm giỏ hàng */ }

        public bool IsFavorite(string productId)
        {
            var favorites = Session["Favorites"] as List<string>;
            return favorites != null && favorites.Contains($"Product-{productId}");
        }

        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal finalPrice = _pm.GetFinalPrice(p);
            return p.SalePercent > 0 
                ? $"<span class='old-price'>${p.Price:F2}</span> <span class='new-price'>${finalPrice:F2}</span>" 
                : $"${p.Price:F2}";
        }

        // Navigation
        protected void lnkSignIn_Click(object sender, EventArgs e) => Response.Redirect("~/Presentation/LogIn.aspx");
        protected void lnkFavorites_Click(object sender, EventArgs e) => Response.Redirect("~/Presentation/Favorites.aspx");
        protected void lnkCart_Click(object sender, EventArgs e) => Response.Redirect("~/Presentation/Cart.aspx");
        protected void search_button_Click(object sender, ImageClickEventArgs e) => Response.Redirect($"~/Presentation/Men.aspx?search={Server.UrlEncode(search_box.Text)}");
        protected void rptReviews_ItemCommand(object source, RepeaterCommandEventArgs e) { }
    }
}