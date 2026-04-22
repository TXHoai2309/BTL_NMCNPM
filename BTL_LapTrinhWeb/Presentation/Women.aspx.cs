using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;

namespace BTL_LapTrinhWeb
{
    public partial class Women : System.Web.UI.Page
    {
        private ProductService _service = new ProductService();

        // ================= STATE =================
        private string CurrentSubCategory
        {
            get => ViewState["CurrentSubCategory"] as string ?? "All";
            set => ViewState["CurrentSubCategory"] = value;
        }

        private string CurrentSearchTerm
        {
            get => ViewState["CurrentSearchTerm"] as string ?? "";
            set => ViewState["CurrentSearchTerm"] = value;
        }

        // ================= PAGE LOAD =================
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            if (!IsPostBack)
            {
                LoadQueryParams();
                LoadProducts();
            }
        }

        private void HandleUserSession()
        {
            if (Session["UserLoggedIn"] is User currentUser)
            {
                lnkSignIn.Text = currentUser.Name;

                lnkSignIn.Click -= lnkSignIn_Click;
                lnkSignIn.Click += lnkMyAccount_Click;
            }
        }

        private void LoadQueryParams()
        {
            if (Request.QueryString["search"] != null)
            {
                CurrentSearchTerm = Request.QueryString["search"];
                txtSearch.Text = CurrentSearchTerm;
            }

            if (Request.QueryString["category"] != null)
            {
                CurrentSubCategory = Request.QueryString["category"];
            }
        }

        // ================= CORE =================
        private void LoadProducts()
        {
            var products = _service.GetProducts(
                "Women",               // gender
                false,                 // không phải sale
                CurrentSubCategory,
                CurrentSearchTerm,
                ddlFilter.SelectedValue,
                ddlSort.SelectedValue
            );

            rptProducts.DataSource = products;
            rptProducts.DataBind();
        }

        // ================= SEARCH =================
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchTerm)) return;

            switch (searchTerm)
            {
                case "men":
                    Response.Redirect("~/Presentation/Men.aspx");
                    break;
                case "women":
                    Response.Redirect("~/Presentation/Women.aspx");
                    break;
                case "kids":
                    Response.Redirect("~/Presentation/Kids.aspx");
                    break;
                case "sale":
                    Response.Redirect("~/Presentation/Sale.aspx");
                    break;
                default:
                    CurrentSearchTerm = searchTerm;
                    LoadProducts();
                    break;
            }
        }

        // ================= EVENTS =================
        protected void ddlSortFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;

            CurrentSubCategory = btn.CommandArgument;
            LoadProducts();
        }

        // ================= NAVIGATION =================
        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
        }

        protected void lnkMyAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/MyAccount.aspx");
        }

        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["UserLoggedIn"] != null
                ? "~/Presentation/Favorites.aspx"
                : "~/Presentation/LogIn.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["UserLoggedIn"] != null
                ? "~/Presentation/Cart.aspx"
                : "~/Presentation/LogIn.aspx");
        }

        // ================= FAVORITE =================
        protected void btnAddToFavorites_Click(object sender, ImageClickEventArgs e)
        {
            string productId = ((ImageButton)sender).CommandArgument;
            string key = $"Women-{productId}";

            var favorites = Session["Favorites"] as List<string> ?? new List<string>();

            if (favorites.Contains(key))
                favorites.Remove(key);
            else
                favorites.Add(key);

            Session["Favorites"] = favorites;

            LoadProducts();
        }

        public bool IsFavorite(string productId, string category)
        {
            var favorites = Session["Favorites"] as List<string>;
            return favorites != null && favorites.Contains($"{category}-{productId}");
        }

        // ================= BUY =================
        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            string productId = ((LinkButton)sender).CommandArgument;
            Response.Redirect($"~/Presentation/Product_Detail.aspx?id={productId}");
        }

        // ================= PRICE =================
        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal price = _service.GetDisplayPrice(p);

            if (p.Sale > 0)
            {
                return $@"
                <div class='price-container'>
                    <span class='old-price'>${p.Price:F2}</span>
                    <span class='new-price'>${price:F2}</span>
                </div>";
            }

            return $"<div class='product_price'>${price:F2}</div>";
        }
    }
}