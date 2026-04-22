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
            if (!IsPostBack)
            {
                CurrentCategory = "All";
            }

            LoadProducts();
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
    }
}