using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services; // 🔥 QUAN TRỌNG
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ClearAllErrors();
            bool isValid = true;

            string ho = txtHoNguoiDung.Text.Trim();
            string ten = txtTenNguoiDung.Text.Trim();
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string xacNhanMatKhau = txtXacNhanMatKhau.Text.Trim();
            string email = txtEmail.Text.Trim();
            string soDienThoai = txtPhoneNumber.Text.Trim();
            string quequan = txtQueQuan.Text.Trim();

            // ===== VALIDATION (GIỮ NGUYÊN) =====
            if (string.IsNullOrEmpty(ho))
            {
                ShowError(lblHoNguoiDungError, "Surname cannot be empty");
                isValid = false;
            }

            if (string.IsNullOrEmpty(ten))
            {
                ShowError(lblTenNguoiDungError, "Name cannot be empty");
                isValid = false;
            }

            if (tenDangNhap.Length < 5)
            {
                ShowError(lblTenDangNhapError, "Username must be at least 5 characters long");
                isValid = false;
            }

            if (matKhau.Length < 6)
            {
                ShowError(lblMatKhauError, "Password must be at least 6 characters long");
                isValid = false;
            }

            if (string.IsNullOrEmpty(xacNhanMatKhau))
            {
                ShowError(lblXacNhanMatKhauError, "Please confirm your password");
                isValid = false;
            }
            else if (matKhau != xacNhanMatKhau)
            {
                ShowError(lblXacNhanMatKhauError, "Passwords do not match");
                isValid = false;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@gmail\.com$"))
            {
                ShowError(lblEmailError, "Invalid email format");
                isValid = false;
            }

            if (!Regex.IsMatch(soDienThoai, @"^0[1-9]\d{8}$"))
            {
                ShowError(lblPhoneNumberError, "Invalid phone number");
                isValid = false;
            }

            if (!Regex.IsMatch(quequan, @"^[A-Za-zÀ-ỹ\s]+$") || string.IsNullOrEmpty(quequan))
            {
                ShowError(lblQueQuanError, "Invalid hometown");
                isValid = false;
            }

            if (!isValid) return;

            // ===== GỌI SERVICE =====
            UserService service = new UserService();

            // check trùng username
            if (service.IsUsernameExists(tenDangNhap))
            {
                lblThongbao.Text = "Username already exists";
                lblThongbao.ForeColor = Color.Red;
                lblThongbao.Visible = true;
                return;
            }

            // tạo user
            User nguoiDungMoi = new User
            {
                Surname = ho,
                Name = ten,
                Username = tenDangNhap,
                Password = matKhau,
                Email = email,
                Phone = soDienThoai,
                QueQuan = quequan
            };

            bool success = service.Register(nguoiDungMoi);

            if (success)
            {
                lblThongbao.Text = "Registration successful";
                lblThongbao.ForeColor = Color.Green;
                lblThongbao.Visible = true;

                ClearFormFields();
                Response.Redirect("~/Presentation/LogIn.aspx");
            }
        }

        private void ClearFormFields()
        {
            txtHoNguoiDung.Text = "";
            txtTenNguoiDung.Text = "";
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
            txtEmail.Text = "";
            txtPhoneNumber.Text = "";
            txtQueQuan.Text = "";
        }

        private void ShowError(Label errorLabel, string message)
        {
            errorLabel.Text = message;
            errorLabel.Visible = true;
        }

        private void ClearAllErrors()
        {
            lblHoNguoiDungError.Visible = false;
            lblTenNguoiDungError.Visible = false;
            lblTenDangNhapError.Visible = false;
            lblMatKhauError.Visible = false;
            lblXacNhanMatKhauError.Visible = false;
            lblEmailError.Visible = false;
            lblPhoneNumberError.Visible = false;
            lblQueQuanError.Visible = false;
        }

        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Favorites.aspx");
            else
                Response.Redirect("~/LogIn.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Cart.aspx");
            else
                Response.Redirect("~/Presentation/LogIn.aspx");
        }
        protected void search_button_Click(object sender, ImageClickEventArgs e)
        {
            // tạm thời để trống hoặc redirect
        }
        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
        }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
            // tạm thời để trống
        }
    }

}