using BTL_LapTrinhWeb.Models;
using BTL_LapTrinhWeb.BLL;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BTL_LapTrinhWeb.Admin
{
    public partial class AdminAddProduct : System.Web.UI.Page
    {
        private ProductManager _pm = new ProductManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserLoggedIn"] == null)
            {
                Response.Redirect("~/Presentation/LogIn.aspx");
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string gender = ddlGender.SelectedValue;

                string mainImg = SaveFileAndGetPath(fuImageUrl, gender);
                string imgL = SaveFileAndGetPath(fuImageLeft, gender);
                string imgB = SaveFileAndGetPath(fuImageBack, gender);
                string imgT = SaveFileAndGetPath(fuImageTop, gender);
                string imgU = SaveFileAndGetPath(fuImageUnder, gender);

                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPrice.Text))
                {
                    lblErrorMessage.Text = "Vui lòng nhập tên và giá sản phẩm.";
                    lblErrorMessage.Visible = true;
                    return;
                }

                Products newProduct = new Products
                {
                    ProductName = txtName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    Image = mainImg,
                    Image1 = imgL,
                    Image2 = imgB,
                    Image3 = imgT,
                    Image4 = imgU,
                    CategoryID = int.Parse(txtCategory.Text),
                    Gender = gender,
                    Description = txtDescription.Text.Trim(),
                    SalePercent = string.IsNullOrEmpty(txtSale.Text) ? 0 : decimal.Parse(txtSale.Text),

                    // 🔥 thêm 2 cái này
                    Stock = 10,
                    Size = "",

                    CreatedAt = DateTime.Now
                };

                // ✅ FIX Ở ĐÂY
                _pm.Insert(newProduct);

                Response.Redirect("~/Presentation/Admin.aspx");
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Lỗi hệ thống: " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }

        private string SaveFileAndGetPath(FileUpload fileUploadControl, string gender)
        {
            if (fileUploadControl != null && fileUploadControl.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fileUploadControl.FileName);
                    string folderPath = "~/Assets/images/" + gender + "/";
                    string serverPath = Server.MapPath(folderPath);

                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }

                    fileUploadControl.SaveAs(serverPath + fileName);
                    return folderPath + fileName;
                }
                catch { return ""; }
            }
            return "";
        }
    }
}