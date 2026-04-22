using System;
using System.Collections.Generic;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
            if (Session["UserLoggedIn"] != null)
            {
                User loggedInUser = (User)Session["UserLoggedIn"];
                lnkSignIn.Text = loggedInUser.Username;
            }
            else
            {
                lnkSignIn.Text = "Sign In";
            }
        }

        private void LoadProducts()
        {
            string category = CurrentSubCategory;
            string sort = ddlSort.SelectedValue;
            string price = ddlFilter.SelectedValue;
            string keyword = txtSearch.Text.Trim();

            // Truyền tham số "Kids" để lọc đúng giới tính trong DB
            var data = _pm.GetProducts(
                "Kids",
                false,
                category,
                keyword,
                price,
                sort
            );

            rptProducts.DataSource = data;
            rptProducts.DataBind();
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            CurrentSubCategory = ((LinkButton)sender).CommandArgument;
            LoadProducts();
        }

        protected void ddlSortFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadProducts();
        }
        public bool IsFavorite(string productId, string category)
        {
            // Lấy danh sách yêu thích từ Session đã khởi tạo ở Global.asax
            var favorites = Session["Favorites"] as List<string>;
            if (favorites == null) return false;

            // Kiểm tra key theo đúng định dạng bạn lưu (ví dụ: "Kids-1")
            string key = $"{category}-{productId}";
            return favorites.Contains(key);
        }

        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal finalPrice = _pm.GetFinalPrice(p);
            if (p.SalePercent > 0)
            {
                return $"<span class='old-price'>${p.Price:F2}</span> <span class='new-price'>${finalPrice:F2}</span>";
            }
            return $"<span>${p.Price:F2}</span>";
        }
        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
        }

        // Xử lý khi nhấn nút Favorites ở Header
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