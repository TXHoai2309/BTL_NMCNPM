using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;
using System;

namespace BTL_LapTrinhWeb
{
    public partial class ThankYou : System.Web.UI.Page
    {
        private ThankYouService _service = new ThankYouService();

        protected void Page_Load(object sender, EventArgs e)
        {
            SetupUserUI();

            if (!IsPostBack)
            {
                LoadOrder();
            }
        }

        // ================= USER =================
        private void SetupUserUI()
        {
            if (Session["UserLoggedIn"] is User user)
            {
                lnkSignIn.Text = user.Name;
                lnkSignIn.Click -= lnkSignIn_Click;
                lnkSignIn.Click += lnkMyAccount_Click;
            }
        }

        private User GetUser()
        {
            return Session["UserLoggedIn"] as User;
        }

        // ================= LOAD =================
        private void LoadOrder()
        {
            var orderCode = Session["LastOrderCode"] as string;
            var user = GetUser();

            if (string.IsNullOrEmpty(orderCode) || user == null)
            {
                Response.Redirect("~/Presentation/Trangchu.aspx");
                return;
            }

            var order = _service.GetOrder(orderCode, user.Name);

            if (order == null)
            {
                ResetUI();
                return;
            }

            BindData(order, user);
        }

        private void BindData(Order order, User user)
        {
            lblOrderCode.Text = order.OrderNumber;
            lblFullName.Text = user.Name;
            lblEmail.Text = user.Email;
            lblPhone.Text = user.Phone;

            lblAddress.Text = order.Address;
            lblCity.Text = order.City;
            lblPostal.Text = order.Postal;

            lblPayment.Text = order.PaymentMethod;
            lblGrandTotal.Text = $"${order.TotalAmount:F2}";

            rptOrderItems.DataSource = order.Items; // 🔥 QUAN TRỌNG
            rptOrderItems.DataBind();
        }

        private void ResetUI()
        {
            lblOrderCode.Text = "N/A";
            lblPayment.Text = "N/A";
            lblGrandTotal.Text = "$0.00";

            rptOrderItems.DataSource = null;
            rptOrderItems.DataBind();
        }

        // ================= NAV =================
        protected void btnHome_Click(object sender, EventArgs e)
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
            Response.Redirect(Session["UserLoggedIn"] != null
                ? "~/Presentation/Favorites.aspx"
                : "~/Presentation/LogIn.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["UserLoggedIn"] != null
                ? "~/Presentation/Cart.aspx"
                : "~/Presentation/LogIn.aspx");
        }

        protected void search_button_Click(object sender, System.Web.UI.ImageClickEventArgs e) { }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e) { }
    }
}