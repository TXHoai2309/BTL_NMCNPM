<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="BTL_LapTrinhWeb.OrderDetail" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Order Detail</title>

    <!-- DÙNG CHUNG STYLE -->
    <link rel="stylesheet" type="text/css" href="~/Assets/css/MyAccount.css" />

    <!-- CSS RIÊNG -->
    <style>
        .order-detail-box {
            background: #fff;
            padding: 20px;
            margin-top: 20px;
            border-radius: 8px;
        }

        .order-header {
            display: flex;
            gap: 50px; /* khoảng cách giữa 2 khối */
        }

        .order-info p {
            margin: 5px 0;
        }

        .timeline {
            margin-top: 20px;
            padding: 15px;
            background: #f5f5f5;
            border-radius: 6px;
        }

        .status {
            font-weight: bold;
            color: green;
        }

        .order-container {
            max-width: 900px; /* giới hạn chiều ngang */
            margin: 40px auto; /* căn giữa */
            padding: 20px;
        }

        .title {
            text-align: center;
            margin-bottom: 20px;
        }

        .order-detail-box {
            background: #fff;
            padding: 25px;
            border-radius: 12px; /* bo góc */
            box-shadow: 0 4px 12px rgba(0,0,0,0.1); /* đổ bóng */
        }

        /* header 2 cột */
        .order-header {
            display: flex;
            gap: 50px;
            flex-wrap: wrap;
        }

        /* mỗi cột */
        .order-info {
            flex: 1;
            min-width: 250px;
        }

        /* bảng */
        .table {
            margin-top: 20px;
            border-radius: 8px;
            overflow: hidden;
        }

        /* status box */
        .timeline {
            margin-top: 20px;
            padding: 15px;
            background: #f5f5f5;
            border-radius: 8px;
        }

        .product-img {
            width: 80px;
            height: 80px;
            object-fit: cover;
            border-radius: 12px;
            border: 1px solid #eee;
        }
        /* TABLE */
        .order-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        /* HEADER */
        .table-header th {
            padding: 12px;
            text-align: left;
            border-bottom: 2px solid #ddd;
        }

        /* ROW */
        .table-row td {
            padding: 15px;
            border-bottom: 1px solid #eee;
            vertical-align: middle;
        }

        /* COLUMN WIDTH (QUAN TRỌNG) */
        .order-table th:nth-child(1),
        .order-table td:nth-child(1) {
            width: 60%;
        }

        .order-table th:nth-child(2),
        .order-table td:nth-child(2) {
            width: 20%;
            text-align: center;
        }

        .order-table th:nth-child(3),
        .order-table td:nth-child(3) {
            width: 20%;
            text-align: right;
        }

        /* PRODUCT */
        .product-cell {
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .product-img {
            width: 70px;
            height: 70px;
            border-radius: 10px;
            object-fit: cover;
            border: 1px solid #eee;
        }

        /* HOVER */
        .table-row:hover {
            background: #fafafa;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <header class="header_container">
            <div class="header_place">
                <div class="search_place">
                    <asp:TextBox ID="search_box" runat="server" placeholder="Search Products" CssClass="search_box" />
                    <asp:ImageButton ID="search_button" runat="server" ImageUrl="~/Assets/images/Chung/kinhlup.png" alt="Search Icon" OnClick="search_button_Click" CssClass="search_button" />
                </div>
                <a href='<%= ResolveUrl("~/Presentation/Trangchu.aspx") %>' class="logo_place">
                    <b id="first_logo">IT</b><b id="last_logo"> Shoes</b>
                </a>
                <div class="user_actions">
                    <div class="account">
                        <asp:Image ID="imgAccount" runat="server" ImageUrl="~/Assets/images/Chung/account.png" alt="Account Icon" />
                        <asp:LinkButton ID="lnkSignIn" runat="server" Text="Sign in" OnClick="lnkSignIn_Click" />
                    </div>
                    <div class="favorites">
                        <asp:Image ID="imgFavorites" runat="server" ImageUrl="~/Assets/images/Chung/icon_favorite.png" alt="Favorites Icon" />
                        <asp:LinkButton ID="lnkFavorites" runat="server" Text="Favorites" OnClick="lnkFavorites_Click" />
                    </div>
                    <div class="cart">
                        <asp:Image ID="imgCart" runat="server" ImageUrl="~/Assets/images/Chung/giohang.png" alt="Cart Icon" />
                        <asp:LinkButton ID="lnkCart" runat="server" Text="Cart" OnClick="lnkCart_Click" />
                    </div>
                </div>
            </div>
            <nav class="menu">
                <ul id="menu_list">
                    <li><a href='<%= ResolveUrl("~/Presentation/Men.aspx") %>' class="menu_item">Men</a></li>
                    <li><a href='<%= ResolveUrl("~/Presentation/Women.aspx") %>' class="menu_item">Women</a></li>
                    <li><a href='<%= ResolveUrl("~/Presentation/Kids.aspx") %>' class="menu_item">Kids</a></li>
                    <li><a href="<%= ResolveUrl("~/Presentation/Sale.aspx") %>" class="menu_item">Sale</a></li>
                </ul>
            </nav>
        </header>

        <div class="banner_sale">
            <a>Free standard delivery on orders over $30</a>
        </div>

        <main class="main_content">

            <div class="order-container">
                <%-- thêm cái này --%>

                <h1 class="title">Order Detail</h1>

                <div class="order-detail-box">

                    <div class="order-header">
                        <div class="order-info">
                            <p>
                                <b>Order ID:</b>
                                <asp:Label ID="lblOrderID" runat="server" />
                            </p>
                            <p>
                                <b>Date:</b>
                                <asp:Label ID="lblDate" runat="server" />
                            </p>
                            <p>
                                <b>Total:</b>
                                <asp:Label ID="lblTotal" runat="server" />
                            </p>
                        </div>

                        <div class="order-info">
                            <p><b>Shipping Address:</b></p>
                            <asp:Label ID="lblAddress" runat="server" />
                        </div>
                    </div>

                    <hr />

                    <asp:GridView ID="gvDetail" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="order-table"
                        GridLines="None"
                        HeaderStyle-CssClass="table-header"
                        RowStyle-CssClass="table-row">

                        <Columns>

                           
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <div class="product-cell">

                                        <img src="/Assets/images/Kids/anh5_under.png" class="product-img" />

                                        <div class="product-info">
                                            <%# Eval("ProductName") %>
                                        </div>

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                            <asp:BoundField DataField="Price" HeaderText="Price" />

                        </Columns>

                    </asp:GridView>

                    <div class="timeline">
                        <p>
                            <b>Status:</b>
                            <asp:Label ID="lblStatus" runat="server" />
                        </p>
                    </div>

                </div>

            </div>
            <%-- đóng container --%>
        </main>

        <footer class="footer_container">
            <div class="shop_menu">
                <p><b>SHOP</b></p>
                <p><a href='<%= ResolveUrl("~/Presentation/Trangchu.aspx") %>' class="menu_item">Home</a></p>
                <p><a href='<%= ResolveUrl("~/Presentation/Women.aspx") %>' class="menu_item">Women</a></p>
                <p><a href='<%= ResolveUrl("~/Presentation/Men.aspx") %>' class="menu_item">Men</a></p>
                <p><a href='<%= ResolveUrl("~/Presentation/Kids.aspx") %>' class="menu_item">Kids</a></p>
                <p><a href='<%= ResolveUrl("~/Presentation/Sale.aspx") %>' class="menu_item">Sale</a></p>
            </div>
            <div class="custom_service_menu">
                <p><b>CUSTOM SERVICE</b></p>
                <p><a href="#" class="menu_item">FAQ</a></p>
                <p><a href="#" class="menu_item">Contact Us</a></p>
                <p><a href="#" class="menu_item">My Account</a></p>
            </div>
            <div class="other_menu">
                <p><b>OTHERS</b></p>
                <p><a href="#" class="menu_item">Articles</a></p>
            </div>
            <div class="icon_menu_container">
                <div class="icon_menu">
                    <img src='<%= ResolveUrl("~/Assets/images/Chung/facebook.png") %>' alt="Facebook" />
                    <img src='<%= ResolveUrl("~/Assets/images/Chung/X.png") %>' alt="Twitter" />
                    <img src='<%= ResolveUrl("~/Assets/images/Chung/insta.png") %>' alt="Instagram" />
                    <img id="youtube" src='<%= ResolveUrl("~/Assets/images/Chung/youtube.png") %>' alt="Youtube" />
                </div>
                <div class="text">
                    <p><b>SUBSCRIBE TO OUR NEWSLETTER</b></p>
                    <p>Get 10% discount on your next order!</p>
                </div>
                <div>
                    <asp:TextBox ID="email_input" runat="server" placeholder="Email" CssClass="email_input" TextMode="Email"></asp:TextBox>
                    <asp:Button ID="SUBSCRIBE_btn" runat="server" Text="SUBSCRIBE" CssClass="SUBSCRIBE_btn" OnClick="SUBSCRIBE_btn_Click" />
                </div>
            </div>
        </footer>

    </form>
</body>
</html>
