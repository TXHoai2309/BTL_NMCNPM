using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class MyAccount : Page
    {
        private UserManager _um = new UserManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] is User user)
            {
                lblUserName.Text = user.Name;
                lnkSignIn.Text = user.Name;

                lnkSignIn.Click -= lnkSignIn_Click;
                lnkSignIn.Click += lnkMyAccount_Click;
            }

            if (!IsPostBack && Session["UserLoggedIn"] is User u)
            {
                LoadUser(u.Id);
            }
        }

        private void LoadUser(int id)
        {
            var user = _um.GetUserByID(id);
            if (user == null) return;

            txtAddress1.Text = user.Address1;
            txtAddress2.Text = user.Address2;
            txtPostal.Text = user.Postal;
            txtCity.Text = user.City;
            txtCountry.Text = user.Country;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!(Session["UserLoggedIn"] is User user)) return;

            var existing = _um.GetUserByID(user.Id);
            if (existing == null) return;

            existing.Address1 = txtAddress1.Text.Trim();
            existing.Address2 = txtAddress2.Text.Trim();
            existing.Postal = txtPostal.Text.Trim();
            existing.City = txtCity.Text.Trim();
            existing.Country = txtCountry.Text.Trim();

            _um.UpdateUser(existing);

            Session["UserLoggedIn"] = _um.GetUserByID(user.Id);

            lblUpdateMessage.Text = "Update successful!";
            lblUpdateMessage.Visible = true;
        }

        private void LoadOrderHistory()
        {
            if (!(Session["UserLoggedIn"] is User user)) return;

            // ⚠️ phần này bạn có thể sửa sau nếu chuyển sang SQL Orders
            // hiện tại giữ nguyên nếu chưa làm DB order
        }

        protected void search_button_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }

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

        protected void btnOrderTab_Click(object sender, EventArgs e)
        {
            updateInfor.Visible = false;
            orderHistory.Visible = true;

            btnOrderTab.CssClass = "tab-button active";
            btnUpdateTab.CssClass = "tab-button";

            LoadOrderHistory();
        }

        protected void btnUpdateTab_Click(object sender, EventArgs e)
        {
            updateInfor.Visible = true;
            orderHistory.Visible = false;

            btnUpdateTab.CssClass = "tab-button active";
            btnOrderTab.CssClass = "tab-button";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }

        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
        }
    }
}