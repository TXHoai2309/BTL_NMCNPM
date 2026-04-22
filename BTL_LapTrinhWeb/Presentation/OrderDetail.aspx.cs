using BTL_LapTrinhWeb.BLL;
using System;
using System.Data;

namespace BTL_LapTrinhWeb
{
    public partial class OrderDetail : System.Web.UI.Page
    {
        private OrderBLL _orderBLL = new OrderBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // =============================
            //// 1. CHECK LOGIN
            //// =============================
            //if (Session["UserID"] == null)
            //{
            //    Response.Redirect("~/Presentation/Login.aspx");
            //    return;
            //}

            //// =============================
            //// 2. LOAD DATA (FIRST LOAD ONLY)
            //// =============================
            //if (!IsPostBack)
            //{
            //    LoadOrderDetail();
            //}
            if (!IsPostBack)
            {
                // Fake data để test UI
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductName");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Price");

                dt.Rows.Add("Nike Air Force 1", "2", "$120");
                dt.Rows.Add("Adidas Ultra Boost", "1", "$150");

                gvDetail.DataSource = dt;
                gvDetail.DataBind();
            }
        }

        private void LoadOrderDetail()
        {
            try
            {
                // =============================
                // 3. VALIDATE ORDER ID
                // =============================
                if (Request.QueryString["orderId"] == null)
                {
                    RedirectToMyAccount();
                    return;
                }

                int orderId;
                if (!int.TryParse(Request.QueryString["orderId"], out orderId))
                {
                    RedirectToMyAccount();
                    return;
                }

                // =============================
                // 4. CALL BLL
                // =============================
                DataSet ds = _orderBLL.GetOrderDetail(orderId);

                if (ds == null || ds.Tables.Count < 2)
                {
                    ShowError("Order not found");
                    return;
                }

                DataTable orderTable = ds.Tables[0];
                DataTable detailTable = ds.Tables[1];

                if (orderTable.Rows.Count == 0)
                {
                    ShowError("Order not found");
                    return;
                }

                // =============================
                // 5. BIND ORDER INFO
                // =============================
                DataRow row = orderTable.Rows[0];

                lblOrderID.Text = row["OrderID"].ToString();
                lblDate.Text = Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy");

                decimal total = 0;
                decimal.TryParse(row["TotalAmount"].ToString(), out total);
                lblTotal.Text = "$" + total.ToString("N2");

                lblAddress.Text = row["Address"].ToString();

                string status = row["Status"].ToString();
                lblStatus.Text = GetStatusText(status);

                // =============================
                // 6. BIND PRODUCT LIST
                // =============================
                if (detailTable.Rows.Count > 0)
                {
                    gvDetail.DataSource = detailTable;
                    gvDetail.DataBind();
                }
                else
                {
                    gvDetail.EmptyDataText = "No products in this order";
                    gvDetail.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading order: " + ex.Message);
            }
        }

        // =============================
        // FORMAT STATUS (UI đẹp hơn)
        // =============================
        private string GetStatusText(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return "🕐 Pending";

                case "processing":
                    return "⚙️ Processing";

                case "shipping":
                    return "🚚 Shipping";

                case "delivered":
                    return "✅ Delivered";

                case "cancelled":
                    return "❌ Cancelled";

                default:
                    return status;
            }
        }

        // =============================
        // HANDLE ERROR
        // =============================
        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = System.Drawing.Color.Red;

            gvDetail.Visible = false;
        }

        // =============================
        // REDIRECT
        // =============================
        private void RedirectToMyAccount()
        {
            Response.Redirect("~/Presentation/MyAccount.aspx");
        }

        // =============================
        // HEADER EVENTS (giống MyAccount)
        // =============================
        protected void lnkSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Login.aspx");
        }

        protected void lnkFavorites_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Favorites.aspx");
        }

        protected void lnkCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/Cart.aspx");
        }

        protected void search_button_Click(object sender, EventArgs e)
        {
            string keyword = search_box.Text.Trim();
            Response.Redirect("~/Presentation/Search.aspx?q=" + keyword);
        }
        protected void SUBSCRIBE_btn_Click(object sender, EventArgs e)
        {
            // demo đơn giản
            string email = email_input.Text.Trim();

            // bạn có thể lưu DB hoặc chỉ thông báo
            lblStatus.Text = "Subscribed with: " + email;
        }
    }
}