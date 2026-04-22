using BTL_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class Trangchu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            if (!IsPostBack)
            {
                BindBestSellers();
                BindArticles();
            }
        }

        // ================= USER SESSION =================
        private void HandleUserSession()
        {
            if (Session["UserLoggedIn"] != null)
            {
                User currentUser = (User)Session["UserLoggedIn"];

                lnkSignIn.Text = currentUser.Name;

                lnkSignIn.Click -= lnkSignIn_Click;
                lnkSignIn.Click += lnkMyAccount_Click;
            }
        }

        // ================= DATA BIND =================
        private void BindBestSellers()
        {
            var menProducts = Application["MenProductList"] as List<Products>;

            if (menProducts == null || !menProducts.Any())
                return;

            var bestSellersList = menProducts
                                    .OrderByDescending(p => p.Price)
                                    .Take(4)
                                    .ToList();

            rptBestSellers.DataSource = bestSellersList;
            rptBestSellers.DataBind();
        }

        private void BindArticles()
        {
            var articleList = new List<Article>
            {
                new Article {
                    Title = "Air Jordan 1 Fragment x Travis Scott",
                    ImageUrl = "~/Assets/images/Trangchu/blog_img1.png",
                    ArticleUrl = "~/Presentation/Blog1.aspx"
                },
                new Article {
                    Title = "Puma LaMelo Ball MB.04",
                    ImageUrl = "~/Assets/images/Trangchu/blog_img2.png",
                    ArticleUrl = "~/Presentation/Blog2.aspx"
                },
                new Article {
                    Title = "Air Jordan 4 Off-White",
                    ImageUrl = "~/Assets/images/Trangchu/blog_img3.png",
                    ArticleUrl = "~/Presentation/Blog3.aspx"
                }
            };

            rptArticles.DataSource = articleList;
            rptArticles.DataBind();
        }

        // ================= SEARCH =================
        protected void search_button_Click(object sender, ImageClickEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchTerm))
                return;

            switch (searchTerm)
            {
                case "men":
                    Response.Redirect(ResolveUrl("~/Presentation/Men.aspx"));
                    break;
                case "women":
                    Response.Redirect(ResolveUrl("~/Presentation/Women.aspx"));
                    break;
                case "kids":
                    Response.Redirect(ResolveUrl("~/Presentation/Kids.aspx"));
                    break;
                case "sale":
                    Response.Redirect(ResolveUrl("~/Presentation/Sale.aspx"));
                    break;
                default:
                    Response.Redirect(ResolveUrl("~/Presentation/Men.aspx?search=" + Server.UrlEncode(searchTerm)));
                    break;
            }
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

        // ================= BUTTON =================
        protected void btnShopNowHero_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Men.aspx");
        }

        protected void btnShopNowSneaker_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Men.aspx?category=Sneakers");
        }

        protected void btnShopNowSale_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Sale.aspx");
        }

        protected void btnCategoryWomen_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Women.aspx");
        }

        protected void btnCategoryMen_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Men.aspx");
        }

        protected void btnCategoryKids_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Kids.aspx");
        }

        // ================= OTHER =================
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
            // TODO: xử lý subscribe sau
        }
    }

    // ================= MODEL =================
    public class Article
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ArticleUrl { get; set; }
    }
}