using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb
{
    public partial class Product_Detail : System.Web.UI.Page
    {
        private ProductDetailService _service = new ProductDetailService();
        private Products currentProduct;

        // ================= PAGE LOAD =================
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleUserSession();

            int id = GetProductId();
            currentProduct = _service.GetProduct(id);

            if (currentProduct == null)
            {
                product_name.InnerText = "Product not found";
                return;
            }

            if (!IsPostBack)
            {
                LoadProductInfo();
                LoadSimilarProducts();
                LoadReviews();
                CheckUserPermission();
            }

            UpdateFavoriteIcon();
        }

        // ================= HELPER =================
        private void HandleUserSession()
        {
            if (Session["UserLoggedIn"] is User user)
            {
                lnkSignIn.Text = user.Name;
                lnkSignIn.Click -= lnkSignIn_Click;
                lnkSignIn.Click += lnkMyAccount_Click;
            }
        }

        private int GetProductId()
        {
            int.TryParse(Request.QueryString["id"], out int id);
            return id;
        }

        // ================= PRODUCT =================
        private void LoadProductInfo()
        {
            product_name.InnerText = currentProduct.Name;
            product_description.InnerText = currentProduct.Description;

            decimal price = _service.GetDisplayPrice(currentProduct);

            if (currentProduct.Sale > 0)
            {
                product_sale_percent.InnerText = $"-{currentProduct.Sale}%";
                product_sale_percent.Visible = true;

                product_price.InnerHtml = $@"
                <div class='price-container'>
                    <span class='old-price'>${currentProduct.Price:F2}</span>
                    <span class='new-price'>${price:F2}</span>
                </div>";
            }
            else
            {
                product_price.InnerHtml = $"<span class='new-price'>${price:F2}</span>";
            }

            main_image.Src = ResolveUrl(currentProduct.ImageUrl);

            SetThumbnail(imgThumbnailMain, currentProduct.ImageUrl, true);
            SetThumbnail(imgThumbnailBack, currentProduct.ImageBack);
            SetThumbnail(imgThumbnailLeft, currentProduct.ImageLeft);
            SetThumbnail(imgThumbnailTop, currentProduct.ImageTop);
            SetThumbnail(imgThumbnailUnder, currentProduct.ImageUnder);

            string page = currentProduct.Gender == "Men" ? "Men.aspx" :
                          currentProduct.Gender == "Women" ? "Women.aspx" : "Kids.aspx";

            litBreadcrumb.Text = $@"
            <a href='{ResolveUrl("~/Presentation/Trangchu.aspx")}'>Home</a> >
            <a href='{ResolveUrl("~/Presentation/" + page)}'>{currentProduct.Gender}</a> >
            <span>{currentProduct.Name}</span>";
        }

        private void SetThumbnail(HtmlImage img, string url, bool active = false)
        {
            if (string.IsNullOrEmpty(url))
            {
                img.Visible = false;
                return;
            }

            img.Src = ResolveUrl(url);
            img.Visible = true;
            img.Attributes["class"] = active ? "thumbnail_image active-thumbnail" : "thumbnail_image";
        }

        // ================= SIMILAR =================
        private void LoadSimilarProducts()
        {
            var similar = _service.GetSimilarProducts(currentProduct);
            rptSimilarProducts.DataSource = similar;
            rptSimilarProducts.DataBind();
        }

        // ================= FAVORITE =================
        private void UpdateFavoriteIcon()
        {
            bool isFav = IsFavorite(currentProduct.ProductID.ToString(), currentProduct.Gender);

            btnFavoriteIcon.ImageUrl = isFav
                ? "~/Assets/images/Chung/icon_favorite_green.png"
                : "~/Assets/images/Chung/icon_favorite.png";

            btnFavoriteIcon.CommandArgument = currentProduct.ProductID.ToString();
        }

        protected void btnAddToFavorites_Click(object sender, EventArgs e)
        {
            string id = ((ImageButton)sender).CommandArgument;
            string key = $"{currentProduct.Gender}-{id}";

            var favorites = Session["Favorites"] as List<string> ?? new List<string>();
            favorites = _service.ToggleFavorite(favorites, key);

            Session["Favorites"] = favorites;

            Response.Redirect(Request.RawUrl);
        }

        public bool IsFavorite(string id, string category)
        {
            var favorites = Session["Favorites"] as List<string>;
            return favorites != null && favorites.Contains($"{category}-{id}");
        }

        // ================= CART =================
        protected void AddToBag_Click(object sender, EventArgs e)
        {
            HandleAddToCart(false);
        }

        protected void BuyAndCheckOut_Click(object sender, EventArgs e)
        {
            if (HandleAddToCart(true))
                Response.Redirect("~/Presentation/Checkout.aspx");
        }

        private bool HandleAddToCart(bool redirect)
        {
            if (Session["UserLoggedIn"] == null)
            {
                lblAddToBagMessage.Text = "Please log in!";
                lblAddToBagMessage.Visible = true;
                return false;
            }

            if (string.IsNullOrEmpty(ddlSize.SelectedValue))
            {
                lblSizeError.Text = "Select size!";
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                lblQuantityError.Text = "Invalid quantity!";
                return false;
            }

            int size = int.Parse(ddlSize.SelectedValue);

            var cart = Session["ShoppingCart"] as List<CartItem> ?? new List<CartItem>();

            cart = _service.AddToCart(cart, currentProduct, size, quantity);

            Session["ShoppingCart"] = cart;

            lblAddToBagMessage.Text = "Added!";
            lblAddToBagMessage.Visible = true;

            return true;
        }

        // ================= REVIEW =================
        private void LoadReviews()
        {
            var user = Session["UserLoggedIn"] as User;
            var reviews = _service.GetReviews(currentProduct.ProductID, user?.Username);

            rptReviews.DataSource = reviews;
            rptReviews.DataBind();
        }

        private void CheckUserPermission()
        {
            var user = Session["UserLoggedIn"] as User;

            pnlAddReview.Visible = _service.UserHasBoughtProduct(user, currentProduct.ProductID);
            lblNoReview.Visible = !pnlAddReview.Visible;
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
        protected void search_button_Click(object sender, ImageClickEventArgs e)
        {
            string keyword = search_box.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword)) return;

            switch (keyword)
            {
                case "men":
                    Response.Redirect("~/Presentation/Men.aspx");
                    break;
                case "women":
                    Response.Redirect("~/Presentation/Women.aspx");
                    break;
                case "kids":
                    Response.Redirect("~/Presentation/Kids.aspx");
                    break;
                case "sale":
                    Response.Redirect("~/Presentation/Sale.aspx");
                    break;
                default:
                    Response.Redirect("~/Presentation/Trangchu.aspx?search=" + keyword);
                    break;
            }
        }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
            string email = email_input.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                // bạn có thể thêm label nếu muốn
                return;
            }

            // lưu tạm vào Application (demo)
            var list = Application["Subscribers"] as List<string> ?? new List<string>();

            if (!list.Contains(email))
            {
                list.Add(email);
            }

            Application["Subscribers"] = list;

            // feedback đơn giản
            email_input.Text = "";
        }
        public string GetPriceDisplay(object dataItem)
        {
            var p = (Products)dataItem;
            decimal price = _service.GetDisplayPrice(p);

            if (p.Sale > 0)
            {
                return $@"
        <div class='price-container'>
            <span class='old-price'>${p.Price:F2}</span>
            <span class='new-price'>${price:F2}</span>
        </div>";
            }

            return $"<div class='product_price'>${price:F2}</div>";
        }
        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            // code của bạn
        }
        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            // TODO: xử lý sau
        }
        protected void rptReviews_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // TODO: xử lý sau
        }
    }
}