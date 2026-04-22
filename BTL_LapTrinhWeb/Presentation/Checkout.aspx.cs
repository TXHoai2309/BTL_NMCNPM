using BTL_LapTrinhWeb.BLL;
using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class Checkout : System.Web.UI.Page
    {
        private CheckoutService _service = new CheckoutService();

        // ================= PAGE LOAD =================
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            if (!IsPostBack)
            {
                FillUserInfo();
                LoadCartItems();
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

        private User GetCurrentUser()
        {
            return Session["UserLoggedIn"] as User;
        }

        //private List<CartItem> GetCart()
        //{
        //    return Session["ShoppingCart"] as List<CartItem> ?? new List<CartItem>();
        //}
        private CartManager _cartManager = new CartManager();

        private List<CartItem> GetCart()
        {
            var user = GetCurrentUser();
            if (user == null) return new List<CartItem>();
            return _cartManager.GetCart(user.Id); // dùng user.Id như Cart.aspx
        }
        // ================= LOAD =================
        private void FillUserInfo()
        {
            var user = GetCurrentUser();
            if (user == null) return;

            txtSurname.Text = user.Surname ?? "";
            txtName.Text = user.Name ?? "";
            txtEmail.Text = user.Email ?? "";
            txtPhone.Text = user.Phone ?? "";
            txtAddress1.Text = user.Address1 ?? "";
            txtAddress2.Text = user.Address2 ?? "";
            txtPostal.Text = user.Postal ?? "";
            txtCity.Text = user.City ?? "";
        }

        private void LoadCartItems()
        {
            var cart = GetCart();

            if (cart.Any())
            {
                rptCheckoutItems.DataSource = cart;
                rptCheckoutItems.DataBind();
                lblEmptyCart.Visible = false;
            }
            else
            {
                rptCheckoutItems.DataSource = null;
                rptCheckoutItems.DataBind();
                lblEmptyCart.Text = "Your cart is empty!";
                lblEmptyCart.Visible = true;
            }

            UpdateTotals(cart);
        }

        // ================= PRICE =================
        public string GetPrice(object priceObj, object saleObj, object qtyObj)
        {
            decimal price = Convert.ToDecimal(priceObj);
            int sale = Convert.ToInt32(saleObj);
            int qty = Convert.ToInt32(qtyObj);

            decimal finalPrice = price * (1 - (decimal)sale / 100);
            return $"${(finalPrice * qty):F2}";
        }

        private void UpdateTotals(List<CartItem> cart)
        {
            decimal total = _service.GetTotal(cart);
            decimal delivery = 0;
            decimal grand = total + delivery;

            litTotalPurchase.Text = $"${total:F2}";
            litDelivery.Text = delivery == 0 ? "FREE" : $"${delivery:F2}";
            litGrandTotal.Text = $"${grand:F2}";
        }

        // ================= NAVIGATION =================
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
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
            Response.Redirect("~/Presentation/Favorites.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
        }

        protected void search_button_Click(object sender, ImageClickEventArgs e) { }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e) { }

        // ================= VALIDATION =================
        private bool ValidateForm()
        {
            bool isValid = true;

            lblSurnameError.Text = lblNameError.Text = lblEmailError.Text = lblPhoneError.Text =
            lblAddress1Error.Text = lblCityError.Text = lblPostalError.Text = "";

            if (string.IsNullOrWhiteSpace(txtSurname.Text))
            {
                lblSurnameError.Text = "Please enter your surname.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblNameError.Text = "Please enter your name.";
                isValid = false;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblEmailError.Text = "Invalid email.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblPhoneError.Text = "Enter phone.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress1.Text))
            {
                lblAddress1Error.Text = "Enter address.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                lblCityError.Text = "Enter city.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPostal.Text))
            {
                lblPostalError.Text = "Enter postal.";
                isValid = false;
            }

            if (rblPaymentMethod.SelectedIndex == -1)
            {
                lblPaymentError.Text = "Select payment.";
                isValid = false;
            }

            return isValid;
        }

        // ================= ORDER =================
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            var cart = GetCart();
            var user = GetCurrentUser();

            if (!cart.Any())
            {
                lblEmptyCart.Text = "Giỏ hàng trống!";
                lblEmptyCart.Visible = true;
                return;
            }

            if (user == null)
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
                return;
            }

            var order = _service.ProcessOrder(
                user, cart,
                rblPaymentMethod.SelectedValue,
                txtAddress1.Text, txtCity.Text, txtPostal.Text
            );

            // Xóa cart trong DB sau khi đặt hàng thành công
            _cartManager.ClearCart(user.Id); // thêm method này vào CartManager
                                             // ❌ Bỏ dòng này: Session.Remove("ShoppingCart");

            Session["LastOrderCode"] = order.OrderNumber;
            Response.Redirect("~/Presentation/ThankYou.aspx");
        }

        
    }
}