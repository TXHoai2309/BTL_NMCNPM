using BTL_LapTrinhWeb.BLL;
using BTL_LapTrinhWeb.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class MyAccount : Page
    {
        private UserManager _um = new UserManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            if (Session["UserLoggedIn"] is User user)
            {
                lblUserName.Text = user.Name;
                // Hiển thị tên người dùng thay cho chữ "Sign in"
                lnkSignIn.Text = !string.IsNullOrEmpty(user.Name) ? user.Name : user.Username;
            }
            else
            {
                // Nếu chưa đăng nhập mà vào trang này, nên đẩy về trang Login
                Response.Redirect("~/Presentation/LogIn.aspx");
            }

            if (!IsPostBack && Session["UserLoggedIn"] is User u)
            {
                LoadUser(u.Id);
            }
        }

        private void LoadUser(int id)
        {
            var user = _um.GetUserByID(id);

            if (user != null)
            {
                txtSurname.Text = user.Surname;
                txtName.Text = user.Name;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;
                txtQueQuan.Text = user.QueQuan;

                txtAddress1.Text = user.Address1;
                txtAddress2.Text = user.Address2;
                txtPostal.Text = user.Postal;
                txtCity.Text = user.City;
                txtCountry.Text = user.Country;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] is User currentUser)
            {
                var userToUpdate = _um.GetUserByID(currentUser.Id);

                if (userToUpdate != null)
                {
                    userToUpdate.Email = txtEmail.Text.Trim();
                    userToUpdate.Phone = txtPhone.Text.Trim();
                    userToUpdate.Surname = txtSurname.Text.Trim();
                    userToUpdate.Name = txtName.Text.Trim();
                    userToUpdate.QueQuan = txtQueQuan.Text.Trim();
                    userToUpdate.Address1 = txtAddress1.Text.Trim();
                    userToUpdate.Address2 = txtAddress2.Text.Trim();
                    userToUpdate.Postal = txtPostal.Text.Trim();
                    userToUpdate.City = txtCity.Text.Trim();
                    userToUpdate.Country = txtCountry.Text.Trim();

                    try
                    {
                        _um.UpdateUser(userToUpdate);

                        Session["UserLoggedIn"] = userToUpdate;

                    }
                    catch (Exception ex)
                    {
                        Response.Write("Lỗi: " + ex.Message);
                    }
                }
            }
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
            // Nếu click vào tên mình khi đã đăng nhập, chuyển hướng lại chính trang này
            if (Session["UserLoggedIn"] != null)
                Response.Redirect("~/Presentation/MyAccount.aspx");
            else
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
            Session.Abandon();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            Response.Redirect("~/Presentation/LogIn.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
        }
    }
}