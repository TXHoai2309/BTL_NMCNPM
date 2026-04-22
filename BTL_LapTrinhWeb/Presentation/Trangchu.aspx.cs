using BTL_LapTrinhWeb.BLL;
using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTL_LapTrinhWeb
{
    public partial class Trangchu : System.Web.UI.Page
    {
        ProductManager pm = new ProductManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBestSellers();
            }
        }

        private void LoadBestSellers()
        {
            List<Products> products = pm.GetAll();

            var best = products.Take(8).ToList();

            rptBestSellers.DataSource = best;
            rptBestSellers.DataBind();
        }

        protected void search_button_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            var result = pm.GetAll()
                .FindAll(p => p.ProductName != null && p.ProductName.ToLower().Contains(keyword));

            rptBestSellers.DataSource = result;
            rptBestSellers.DataBind();
        }

        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
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
        protected void btnShopNowHero_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Products.aspx");
        }
        protected void btnShopNowSale_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Products.aspx?isSale=true");
        }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
        }
        protected void btnShopNowSneaker_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Men.aspx?category=Sneaker");
        }
        protected void btnCategoryWomen_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Women.aspx");
        }

        protected void btnCategoryMen_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Men.aspx");
        }

        protected void btnCategoryKids_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Kids.aspx");
        }
    }
}