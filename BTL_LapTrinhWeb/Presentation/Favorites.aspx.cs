using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb
{
    public partial class Favorites : System.Web.UI.Page
    {
        private ProductManager _pm = new ProductManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            if (!IsPostBack)
            {
                LoadFavorites();
            }
        }

        private void HandleUserSession()
        {
            if (Session["UserLoggedIn"] is User user)
            {
                lnkSignIn.Text = user.Name;

                lnkSignIn.Click -= lnkSignIn_Click;
                lnkSignIn.Click += lnkMyAccount_Click;
            }
        }

        private List<string> GetFavoriteIds()
        {
            return Session["Favorites"] as List<string> ?? new List<string>();
        }

        private void LoadFavorites()
        {
            var favoriteIds = GetFavoriteIds();

            if (favoriteIds.Count == 0)
            {
                pnlEmptyFavorites.Visible = true;
                rptFavorites.Visible = false;
                return;
            }

            // lấy tất cả sản phẩm từ SQL
            List<Products> allProducts = _pm.GetAll();

            // lọc theo danh sách favorite
            List<Products> data = new List<Products>();

            foreach (var item in favoriteIds)
            {
                var parts = item.Split('-');
                if (parts.Length != 2) continue;

                int productId;
                if (int.TryParse(parts[1], out productId))
                {
                    var product = allProducts.FirstOrDefault(p => p.ProductID == productId);
                    if (product != null)
                        data.Add(product);
                }
            }

            pnlEmptyFavorites.Visible = false;
            rptFavorites.Visible = true;

            rptFavorites.DataSource = data;
            rptFavorites.DataBind();
        }

        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal finalPrice = _pm.GetFinalPrice(p);

            if (p.SalePercent > 0)
            {
                return $@"
                <div class='price-container'>
                    <span class='old-price'>${p.Price:F2}</span>
                    <span class='new-price'>${finalPrice:F2}</span>
                </div>";
            }

            return $"<div class='product_price'>${p.Price:F2}</div>";
        }

        protected void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;

            var favorites = GetFavoriteIds();
            favorites.Remove(id);

            Session["Favorites"] = favorites;

            LoadFavorites();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            string uniqueId = ((Button)sender).CommandArgument;

            var parts = uniqueId.Split('-');
            if (parts.Length != 2) return;

            Response.Redirect($"~/Presentation/Product_Detail.aspx?id={parts[1]}");
        }

        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
        }

        protected void lnkMyAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/MyAccount.aspx");
        }

        protected void lnkFavorites_Click(object sender, EventArgs e) { }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
        }

        protected void lnkContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }

        protected void search_button_Click(object sender, EventArgs e) { }

        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e) { }
    }
}