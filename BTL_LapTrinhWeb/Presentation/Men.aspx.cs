using BTL_LapTrinhWeb.BLL;
using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class Men : System.Web.UI.Page
    {
        private ProductManager _pm = new ProductManager();

        private string CurrentCategory
        {
            get { return ViewState["Category"]?.ToString() ?? "All"; }
            set { ViewState["Category"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] is User loggedInUser)
            {
                lnkSignIn.Text = !string.IsNullOrEmpty(loggedInUser.Name) ? loggedInUser.Name : loggedInUser.Username;
            }
            else
            {
                lnkSignIn.Text = "Sign In";
            }

            if (!IsPostBack)
            {
                LoadProducts();
            }
        }
        private void LoadProducts()
        {
            string category = CurrentCategory;
            string sort = ddlSort.SelectedValue;
            string price = ddlFilter.SelectedValue;
            string keyword = txtSearch.Text.Trim();

            var data = _pm.GetProducts(
                "Men",
                false,
                category,
                keyword,
                price,
                sort
            );

            rptProducts.DataSource = data;
            rptProducts.DataBind();
        }

        // ===== CATEGORY CLICK =====
        protected void btnCategory_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;

            CurrentCategory = btn.CommandArgument;

            LoadProducts();
        }

        // ===== SORT + FILTER =====
        protected void ddlSortFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        // ===== SEARCH =====
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadProducts();
        }
        protected bool IsFavorite(string productId, string page)
        {
            var favorites = Session["Favorites"] as List<string>;

            if (favorites == null)
                return false;

            string key = $"Product-{productId}";

            return favorites.Contains(key);
        }
        // ===== PRICE =====
        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal final = _pm.GetFinalPrice(p);

            if (p.SalePercent > 0)
            {
                return $@"
                <div>
                    <span class='old-price'>${p.Price:F2}</span>
                    <span class='new-price'>${final:F2}</span>
                </div>";
            }

            return $"<div>${p.Price:F2}</div>";
        }
        protected void lnkSignIn_Click(object sender, EventArgs e)
        {

            if (Session["UserLoggedIn"] != null)
            {
                Response.Redirect("~/Presentation/MyAccount.aspx");
            }
            else
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
            }
        }
        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Presentation/Favorites.aspx");
            else
                Response.Redirect("~/Presentation/LogIn.aspx");
        }

        // Xử lý khi nhấn nút Cart ở Header
        protected void lnkCart_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Presentation/Cart.aspx");
            else
                Response.Redirect("~/Presentation/LogIn.aspx");
        }
        // Xử lý khi nhấn nút Yêu thích (Trái tim) trên từng sản phẩm
        protected void btnAddToFavorites_Click(object sender, ImageClickEventArgs e)
        {
            string productId = ((ImageButton)sender).CommandArgument;
            // Tạo key phân biệt để lưu vào Session
            string key = $"Product-{productId}";

            var favorites = Session["Favorites"] as List<string> ?? new List<string>();

            if (favorites.Contains(key))
                favorites.Remove(key);
            else
                favorites.Add(key);

            Session["Favorites"] = favorites;

            // Load lại dữ liệu để cập nhật icon (đổi màu trái tim)
            LoadProducts();
        }

        // Xử lý khi nhấn nút Buy Now
        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            string productId = ((LinkButton)sender).CommandArgument;
            // Chuyển hướng sang trang chi tiết sản phẩm kèm ID
            Response.Redirect($"~/Presentation/Product_Detail.aspx?id={productId}");
        }
    }
}