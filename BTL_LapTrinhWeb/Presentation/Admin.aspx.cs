using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.Services;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb.Admin
{
    public partial class Admin : System.Web.UI.Page
    {
        private readonly ProductService service = new ProductService();

        // ================= PAGE LOAD =================
        protected void Page_Load(object sender, EventArgs e)
        {
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

        // ================= BIND =================
        private void BindGrid(string keyword = "")
        {
            gvProducts.DataSource = service.Search(keyword); // ✔ đúng
            gvProducts.DataBind();
        }

        // ================= SEARCH =================
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindGrid(txtSearch.Text.Trim());
        }

        // ================= NAVIGATION =================
        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/AdminAddProduct.aspx");
        }

        protected void btnBackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation/LogIn.aspx");
        }

        // ================= GRID EDIT =================
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

        // ================= UPDATE =================
        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvProducts.Rows[e.RowIndex];

            FileUpload fuImage = row.FindControl("fuImageUrlEdit") as FileUpload;
            HiddenField hdnImage = row.FindControl("hdnImageUrl") as HiddenField;

            string gender = (row.FindControl("txtGender") as TextBox)?.Text;
            string imageUrl = hdnImage?.Value;

            // ===== HANDLE UPLOAD IMAGE =====
            if (fuImage != null && fuImage.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fuImage.FileName);
                    string folder = $"~/Assets/images/{gender}";
                    string serverFolder = Server.MapPath(folder);

                    if (!Directory.Exists(serverFolder))
                        Directory.CreateDirectory(serverFolder);

                    string fullPath = Path.Combine(serverFolder, fileName);
                    fuImage.SaveAs(fullPath);

                    imageUrl = folder + "/" + fileName;
                }
                catch { }
            }

            // ===== BUILD OBJECT =====
            Products p = new Products
            {
                ProductID = id,
                Name = (row.FindControl("txtName") as TextBox)?.Text,
                Category = (row.FindControl("txtCategory") as TextBox)?.Text,
                Gender = gender,
                Description = (row.FindControl("txtDescription") as TextBox)?.Text,
                ImageUrl = imageUrl
            };

            decimal.TryParse((row.FindControl("txtPrice") as TextBox)?.Text, out decimal price);
            int.TryParse((row.FindControl("txtSale") as TextBox)?.Text, out int sale);

            p.Price = price;
            p.Sale = sale;

            service.UpdateProduct(p);

            gvProducts.EditIndex = -1;
            BindGrid(txtSearch.Text.Trim());
        }

        // ================= DELETE =================
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            service.DeleteProduct(id);

            BindGrid(txtSearch.Text.Trim());
        }

        // ================= TRUNCATE =================
        public string TruncateDescription(object description, int maxLength = 100)
        {
            return service.Truncate(description?.ToString(), maxLength);
        }
    }
}