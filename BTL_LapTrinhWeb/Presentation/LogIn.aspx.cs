using BTL_LapTrinhWeb.Models;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTL_LapTrinhWeb.Services;

namespace BTL_LapTrinhWeb
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie rememberCookie = Request.Cookies["RememberMe"];
                if (rememberCookie != null)
                {
                    txtTenDangNhap.Text = rememberCookie.Value;
                    chkGhiNho.Checked = true;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = txtTenDangNhap.Text.Trim();
            string password = txtMatKhau.Text.Trim();

            // 👉 GỌI SERVICE
            UserService service = new UserService();
            User foundUser = service.Login(username, password);

            if (foundUser != null)
            {
                // 👉 COOKIE (GIỮ NGUYÊN LOGIC)
                if (chkGhiNho.Checked)
                {
                    HttpCookie rememberCookie = new HttpCookie("RememberMe", foundUser.Username)
                    {
                        Expires = DateTime.Now.AddDays(1)
                    };
                    Response.Cookies.Add(rememberCookie);
                }
                else
                {
                    if (Request.Cookies["RememberMe"] != null)
                    {
                        HttpCookie expiredCookie = new HttpCookie("RememberMe")
                        {
                            Expires = DateTime.Now.AddDays(-1)
                        };
                        Response.Cookies.Add(expiredCookie);
                    }
                }

                Session["UserLoggedIn"] = foundUser;

                // 👉 PHÂN ROLE (admin)
                if (foundUser.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/Presentation/Admin.aspx");
                }
                else
                {
                    Response.Redirect("~/Presentation/Trangchu.aspx");
                }
            }
            else
            {
                lblThongbao.Text = "The username or password is incorrect!";
                lblThongbao.Visible = true;
            }
        }

        protected void search_button_Click(object sender, ImageClickEventArgs e) { }

        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/SignIn.aspx");
        }

        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
            {
                Response.Redirect("~/Presentation/Favorites.aspx");
            }
            else
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
            }
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] != null)
            {
                Response.Redirect("~/Presentation/Cart.aspx");
            }
            else
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
                ///
            }
        }

        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e) { }
    }
}