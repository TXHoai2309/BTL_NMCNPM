using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;

namespace BTL_LapTrinhWeb
{
    public partial class Favorites : System.Web.UI.Page
    {
        private FavoritesService _service = new FavoritesService();

        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            if (!IsPostBack)
            {
                LoadFavorites();
            }
        }

        // ================= USER =================
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

        // ================= LOAD =================
        private void LoadFavorites()
        {
            var favoriteIds = GetFavoriteIds();

            if (favoriteIds.Count == 0)
            {
                pnlEmptyFavorites.Visible = true;
                rptFavorites.Visible = false;
                return;
            }

            var data = _service.GetFavorites(favoriteIds);

            pnlEmptyFavorites.Visible = false;
            rptFavorites.Visible = true;

            rptFavorites.DataSource = data;
            rptFavorites.DataBind();
        }

        // ================= PRICE =================
        public string GetPriceDisplay(object dataItem)
        {
            decimal price = (decimal)DataBinder.Eval(dataItem, "Price");
            int sale = (int)DataBinder.Eval(dataItem, "Sale");

            decimal finalPrice = _service.GetFinalPrice(price, sale);

            if (sale > 0)
            {
                return $@"
                <div class='price-container'>
                    <span class='old-price'>${price:F2}</span>
                    <span class='new-price'>${finalPrice:F2}</span>
                </div>";
            }

            return $"<div class='product_price'>${price:F2}</div>";
        }

        // ================= REMOVE =================
        protected void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;

            var favorites = GetFavoriteIds();
            favorites.Remove(id);

            Session["Favorites"] = favorites;

            LoadFavorites();
        }

        // ================= ADD TO CART =================
        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            string uniqueId = ((Button)sender).CommandArgument;

            var parts = uniqueId.Split('-');
            if (parts.Length != 2) return;

            Response.Redirect($"~/Presentation/Product_Detail.aspx?id={parts[1]}");
        }

        // ================= NAV =================
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