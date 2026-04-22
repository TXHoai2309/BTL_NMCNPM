using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb
{
    public partial class Kids : System.Web.UI.Page
    {
        private ProductManager _pm = new ProductManager();

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
            if (Session["UserLoggedIn"] is User user)
            {
                lnkSignIn.Text = user.Name;

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

        // core
        private void LoadProducts()
        {
            List<Products> products = _pm.GetAll();

            // filter KIDS
            products = products
                .Where(p => p.Gender != null &&
                            p.Gender.Equals("Kids", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // category
            if (CurrentSubCategory != "All")
            {
                products = products
                    .Where(p => p.Category != null &&
                                p.Category.Equals(CurrentSubCategory, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // search
            if (!string.IsNullOrEmpty(CurrentSearchTerm))
            {
                products = products
                    .Where(p => p.ProductName != null &&
                                p.ProductName.ToLower().Contains(CurrentSearchTerm.ToLower()))
                    .ToList();
            }

            // sort
            switch (ddlSort.SelectedValue)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;

                case "sale":
                    products = products.OrderByDescending(p => p.SalePercent).ToList();
                    break;
            }

            rptProducts.DataSource = products;
            rptProducts.DataBind();
        }

        // search
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string search = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(search)) return;

            switch (search)
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
                    CurrentSearchTerm = search;
                    LoadProducts();
                    break;
            }
        }

        // event 
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

        // navigation
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
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Presentation/Favorites.aspx");
            else
                Response.Redirect("~/Presentation/LogIn.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Presentation/Cart.aspx");
            else
                Response.Redirect("~/Presentation/LogIn.aspx");
        }

        // favorites
        protected void btnAddToFavorites_Click(object sender, ImageClickEventArgs e)
        {
            string id = ((ImageButton)sender).CommandArgument;
            string key = $"Kids-{id}";

            var favorites = Session["Favorites"] as List<string> ?? new List<string>();

            if (favorites.Contains(key))
                favorites.Remove(key);
            else
                favorites.Add(key);

            Session["Favorites"] = favorites;

            LoadProducts();
        }

        public bool IsFavorite(string id, string category)
        {
            var favorites = Session["Favorites"] as List<string>;
            return favorites != null && favorites.Contains($"{category}-{id}");
        }

        // buy
        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            Response.Redirect($"~/Presentation/Product_Detail.aspx?id={id}");
        }

        // price display
        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal price = _pm.GetFinalPrice(p);

            if (p.SalePercent > 0)
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