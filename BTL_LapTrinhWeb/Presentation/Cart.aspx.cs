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
                int cartId = int.Parse(e.CommandArgument.ToString()); // đây là CartID
                _cart.Delete(cartId); // xóa đúng 1 dòng theo CartID
                LoadCart();
            }
        }
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    int cartId = int.Parse(((LinkButton)sender).CommandArgument);

        //    _cart.Delete(cartId);

        //    LoadCart();
        //}


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
        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Favorites.aspx");
        }
        protected void lnkSignIn_Click(object sender, EventArgs e)
        {

            if (Session["UserLoggedIn"] != null)
            {
                Response.Redirect("~/Presentation/MyAccount.aspx");
            }
            else
            {
                // Nếu chưa, chuyển đến Log In
                Response.Redirect("~/Presentation/LogIn.aspx");
            }
        }
        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
        }
    }
}