using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;

namespace BTL_LapTrinhWeb
{
    public partial class Women : System.Web.UI.Page
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
        }

        private void LoadProducts()
        {
            // Lấy giá trị từ các điều khiển lọc trên giao diện
            string category = CurrentSubCategory;
            string sort = ddlSort.SelectedValue;
            string price = ddlFilter.SelectedValue;
            string keyword = txtSearch.Text.Trim();

            // Truyền tham số "Women" để lọc đúng giới tính trong DB
            var data = _pm.GetProducts(
                "Women",
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
        protected bool IsFavorite(string productId, string page)
        {
            var favorites = Session["Favorites"] as List<string>;

            if (favorites == null)
                return false;

            string key = $"Product-{productId}";

            return favorites.Contains(key);
        }
        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string productId = btn.CommandArgument;

            Response.Write("Bạn chọn sản phẩm: " + productId);
        }
    }
}