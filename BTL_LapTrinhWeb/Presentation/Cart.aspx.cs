using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class Cart : System.Web.UI.Page
    {
        private CartService _service = new CartService();

        // ================= PAGE LOAD =================
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            if (!IsPostBack)
            {
                LoadCart();
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

        // ================= LOAD =================
        private void LoadCart()
        {
            var cart = _service.GetCart();

            if (cart.Any())
            {
                rptCartItems.DataSource = cart;
                rptCartItems.DataBind();
                pnlEmptyCart.Visible = false;
            }
            else
            {
                rptCartItems.DataSource = null;
                rptCartItems.DataBind();
                pnlEmptyCart.Visible = true;
            }

            UpdateTotals(cart);
        }

        // ================= TOTAL =================
        private void UpdateTotals(List<CartItem> cart)
        {
            decimal total = _service.GetTotal(cart);
            decimal delivery = 0;
            decimal grand = total + delivery;

            litTotalPurchase.Text = $"${total:F2}";
            litDelivery.Text = delivery == 0 ? "FREE" : $"${delivery:F2}";
            litGrandTotal.Text = $"${grand:F2}";

            btnSecureCheckout.Enabled = cart.Any();
        }

        // ================= REPEATER =================
        protected void rptCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem) return;

            var item = (CartItem)e.Item.DataItem;
            var litSale = (Literal)e.Item.FindControl("litSaleInfo");

            litSale.Visible = item.Sale > 0;

            if (item.Sale > 0)
            {
                litSale.Text = $"<p class='sale-info'>Sale: -{item.Sale}%</p>";
            }
        }

        protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Delete") return;

            string productNumber = e.CommandArgument.ToString();

            _service.RemoveItem(productNumber);

            LoadCart();
        }

        // ================= SEARCH =================
        protected void search_button_Click(object sender, ImageClickEventArgs e) { }

        // ================= NAV =================
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
            Response.Redirect("~/Presentation/Favorites.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
        }

        protected void btnContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }

        protected void btnSecureCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Checkout.aspx");
        }

        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e) { }
    }
}