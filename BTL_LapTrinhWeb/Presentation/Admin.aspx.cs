using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb.Admin
{
    public partial class Admin : System.Web.UI.Page
    {
        // Sử dụng Manager trực tiếp để quản lý dữ liệu SQL
        private ProductManager _pm = new ProductManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền đăng nhập (Admin)
            if (Session["UserLoggedIn"] == null)
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid(string keyword = "")
        {
            // Nếu không có từ khóa, lấy toàn bộ. Nếu có, gọi hàm Search từ SQL
            if (string.IsNullOrEmpty(keyword))
            {
                gvProducts.DataSource = _pm.GetAll();
            }
            else
            {
                gvProducts.DataSource = _pm.Search(keyword);
            }

            gvProducts.DataKeyNames = new string[] { "ProductID" };
            gvProducts.DataBind();
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindGrid(txtSearch.Text.Trim());
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/AdminAddProduct.aspx");
        }

        // --- XỬ LÝ SỬA TRÊN GRIDVIEW ---
        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            BindGrid(txtSearch.Text.Trim());
        }

        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            BindGrid(txtSearch.Text.Trim());
        }

        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvProducts.Rows[e.RowIndex];
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);

            Products p = _pm.GetByID(id);

            if (p != null)
            {
                p.ProductName = (row.FindControl("txtName") as TextBox).Text;
                p.Description = (row.FindControl("txtDescription") as TextBox).Text;

                decimal.TryParse((row.FindControl("txtPrice") as TextBox).Text, out decimal price);
                decimal.TryParse((row.FindControl("txtSale") as TextBox).Text, out decimal sale);

                p.Price = price;
                p.SalePercent = sale;

                // 🔥 FIX CATEGORY
                DropDownList ddl = row.FindControl("ddlCategoryEdit") as DropDownList;
                if (ddl != null)
                {
                    p.CategoryID = int.Parse(ddl.SelectedValue);
                }

                // 🔥 FIX GENDER
                DropDownList ddlGender = row.FindControl("ddlGenderEdit") as DropDownList;
                if (ddlGender != null)
                {
                    p.Gender = ddlGender.SelectedValue;
                }

                _pm.Update(p);
            }

            gvProducts.EditIndex = -1;
            BindGrid(txtSearch.Text.Trim());
        }

        // --- XỬ LÝ XÓA ---
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);

            // Thực thi xóa trong SQL Server
            _pm.Delete(id);

            BindGrid(txtSearch.Text.Trim());
        }

        public string TruncateDescription(object description, int maxLength = 100)
        {
            if (description == null) return "";
            string text = description.ToString();
            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }
        protected void btnBackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("LogIn.aspx");
        }
    }
}