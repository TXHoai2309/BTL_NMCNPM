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
    }
}