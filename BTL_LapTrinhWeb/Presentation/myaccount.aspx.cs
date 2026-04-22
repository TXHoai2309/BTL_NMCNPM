using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class MyAccount : Page
    {
        private UserService _service = new UserService();

        // ================= PAGE LOAD =================
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

        // ================= LOAD USER =================
        private void LoadUser(int id)
        {
            var user = _service.GetUser(id);
            if (user == null) return;

            txtAddress1.Text = user.Address1;
            txtAddress2.Text = user.Address2;
            txtPostal.Text = user.Postal;
            txtCity.Text = user.City;
            txtCountry.Text = user.Country;
        }

        // ================= UPDATE =================
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!(Session["UserLoggedIn"] is User user)) return;

            User updated = new User
            {
                Id = user.Id,
                Address1 = txtAddress1.Text.Trim(),
                Address2 = txtAddress2.Text.Trim(),
                Postal = txtPostal.Text.Trim(),
                City = txtCity.Text.Trim(),
                Country = txtCountry.Text.Trim()
            };

            _service.UpdateUser(updated);

            // refresh session
            Session["UserLoggedIn"] = _service.GetUser(user.Id);

            lblUpdateMessage.Text = "Update successful!";
            lblUpdateMessage.Visible = true;
        }

        // ================= ORDER HISTORY =================
        private void LoadOrderHistory()
        {
            if (!(Session["UserLoggedIn"] is User user)) return;

            var orders = _service.GetOrders(user.Name);

            if (orders.Any())
            {
                gvOrder.DataSource = orders.Select(o => new
                {
                    o.OrderNumber,
                    Date = o.Date.ToString("dd/MM/yyyy HH:mm"),
                    Items = string.Join("<br/>",
                        o.Items.Select(i => $"{i.Name} (x{i.Quantity})")),
                    Total = $"${o.TotalAmount:F2}"
                });

                gvOrder.DataBind();
                gvOrder.Visible = true;
            }
            else
            {
                gvOrder.Visible = false;
            }
        }

        // ================= SEARCH =================
        protected void search_button_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }

        // ================= NAVIGATION =================
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

        // ================= TAB =================
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

        // ================= LOGOUT =================
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Presentation/Trangchu.aspx");
        }

        // ================= SUBSCRIBE =================
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
            // để trống (sau làm)
        }
    }
}