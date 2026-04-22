using BTL_LapTrinhWeb.BLL;
using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class Cart : System.Web.UI.Page
    {
        CartManager _cart = new CartManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            if (!(Session["UserLoggedIn"] is User user))
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
                return;
            }

            var data = _cart.GetCart(user.Id);
            var total = data.Sum(i => i.LineTotal);

            litTotalPurchase.Text = $"$ {total:F2}";
            litGrandTotal.Text = $"$ {total:F2}";
            rptCartItems.DataSource = data;
            rptCartItems.DataBind();
        }
        protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int productId = int.Parse(e.CommandArgument.ToString());

                var user = (User)Session["UserLoggedIn"];

                _cart.DeleteByProduct(user.Id, productId);

                LoadCart();
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int cartId = int.Parse(((LinkButton)sender).CommandArgument);

            _cart.Delete(cartId);

            LoadCart();
        }

        public decimal GetTotal(List<CartItem> cart)
        {
            decimal total = 0;

            foreach (var item in cart)
            {
                total += item.LineTotal;
            }

            return total;
        }
        protected void rptCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (CartItem)e.Item.DataItem;

                Literal litSale = (Literal)e.Item.FindControl("litSaleInfo");

                if (item.Sale > 0)
                {
                    litSale.Visible = true;
                    litSale.Text = $"<p style='color:red;'>Sale: -{item.Sale}%</p>";
                }
            }
        }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
        }
        protected void btnSecureCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Checkout.aspx");
        }
        protected void btnContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }
        protected void search_button_Click(object sender, ImageClickEventArgs e) { }
    }
}