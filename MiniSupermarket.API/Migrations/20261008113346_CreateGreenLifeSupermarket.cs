using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateGreenLifeSupermarket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Thực phẩm hữu cơ", "Các sản phẩm thực phẩm hữu cơ, tự nhiên" },
                    { 2, "Đồ uống thiên nhiên", "Nước uống, trà và các sản phẩm có nguồn gốc tự nhiên" },
                    { 3, "Sản phẩm chăm sóc cá nhân", "Các sản phẩm chăm sóc cá nhân thân thiện với môi trường" },
                    { 4, "Đồ dùng gia đình xanh", "Đồ dùng gia đình có thể tái sử dụng" },
                    { 5, "Sản phẩm tái chế", "Các sản phẩm được làm từ vật liệu tái chế" },
                    { 6, "Rau củ hữu cơ", "Các loại rau củ được trồng theo phương pháp hữu cơ" },
                    { 7, "Trái cây hữu cơ", "Các loại trái cây sạch và tự nhiên" },
                    { 8, "Ngũ cốc", "Các loại ngũ cốc và hạt dinh dưỡng" },
                    { 9, "Thực phẩm khô", "Các loại thực phẩm khô tiện dụng" },
                    { 10, "Gia vị tự nhiên", "Các loại gia vị có nguồn gốc tự nhiên" },
                    { 11, "Đồ dùng nhà bếp", "Các sản phẩm sử dụng trong nhà bếp" },
                    { 12, "Đồ dùng cá nhân", "Các sản phẩm phục vụ nhu cầu cá nhân" },
                    { 13, "Văn phòng phẩm xanh", "Văn phòng phẩm thân thiện với môi trường" },
                    { 14, "Sản phẩm vệ sinh", "Các sản phẩm vệ sinh an toàn" },
                    { 15, "Sản phẩm thân thiện môi trường", "Các sản phẩm góp phần bảo vệ môi trường" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "Email", "FullName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "TP. Hồ Chí Minh", "nguyenvanan@gmail.com", "Nguyễn Văn An", "0900000001" },
                    { 2, "TP. Hồ Chí Minh", "tranthibinh@gmail.com", "Trần Thị Bình", "0900000002" },
                    { 3, "Bình Dương", "levancuong@gmail.com", "Lê Văn Cường", "0900000003" },
                    { 4, "Đồng Nai", "phamthidung@gmail.com", "Phạm Thị Dung", "0900000004" },
                    { 5, "TP. Hồ Chí Minh", "hoangvanminh@gmail.com", "Hoàng Văn Minh", "0900000005" },
                    { 6, "Long An", "nguyenthilan@gmail.com", "Nguyễn Thị Lan", "0900000006" },
                    { 7, "TP. Hồ Chí Minh", "tranvanhung@gmail.com", "Trần Văn Hùng", "0900000007" },
                    { 8, "Bình Dương", "lethimai@gmail.com", "Lê Thị Mai", "0900000008" },
                    { 9, "Đồng Nai", "phamvannam@gmail.com", "Phạm Văn Nam", "0900000009" },
                    { 10, "TP. Hồ Chí Minh", "hoangthihoa@gmail.com", "Hoàng Thị Hoa", "0900000010" },
                    { 11, "Tây Ninh", "dovanthanh@gmail.com", "Đỗ Văn Thành", "0900000011" },
                    { 12, "TP. Hồ Chí Minh", "vuthihuong@gmail.com", "Vũ Thị Hương", "0900000012" },
                    { 13, "Bà Rịa - Vũng Tàu", "nguyenvanlong@gmail.com", "Nguyễn Văn Long", "0900000013" },
                    { 14, "TP. Hồ Chí Minh", "tranthingoc@gmail.com", "Trần Thị Ngọc", "0900000014" },
                    { 15, "Bình Phước", "levanphuc@gmail.com", "Lê Văn Phúc", "0900000015" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "FullName", "IsActive", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "Quản trị viên", true, "123456", "Admin", "admin" },
                    { 2, "Nguyễn Văn Thuận", true, "123456", "Cashier", "cashier1" },
                    { 3, "Trần Văn Nam", true, "123456", "Cashier", "cashier2" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "OrderDate", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 205000m, 1 },
                    { 2, 2, new DateTime(2026, 10, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 115000m, 2 },
                    { 3, 3, new DateTime(2026, 10, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 165000m, 2 },
                    { 4, 4, new DateTime(2026, 10, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 95000m, 3 },
                    { 5, 5, new DateTime(2026, 10, 3, 9, 30, 0, 0, DateTimeKind.Unspecified), "Completed", 110000m, 1 },
                    { 6, 1, new DateTime(2026, 10, 3, 13, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 88000m, 2 },
                    { 7, 2, new DateTime(2026, 10, 4, 10, 30, 0, 0, DateTimeKind.Unspecified), "Completed", 170000m, 3 },
                    { 8, 3, new DateTime(2026, 10, 5, 15, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 165000m, 1 },
                    { 9, 4, new DateTime(2026, 10, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 185000m, 2 },
                    { 10, 5, new DateTime(2026, 10, 7, 17, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 195000m, 3 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "893000000001", 1, 85000m, "Gạo lứt hữu cơ", 50 },
                    { 2, "893000000002", 1, 120000m, "Mật ong nguyên chất", 30 },
                    { 3, "893000000003", 2, 35000m, "Nước ép cam nguyên chất", 60 },
                    { 4, "893000000004", 2, 45000m, "Trà xanh thiên nhiên", 45 },
                    { 5, "893000000005", 3, 55000m, "Xà phòng thiên nhiên", 35 },
                    { 6, "893000000006", 3, 110000m, "Dầu gội thảo mộc", 25 },
                    { 7, "893000000007", 4, 45000m, "Túi vải tái sử dụng", 70 },
                    { 8, "893000000008", 4, 25000m, "Ống hút tre", 80 },
                    { 9, "893000000009", 5, 35000m, "Sổ tay giấy tái chế", 50 },
                    { 10, "893000000010", 5, 75000m, "Chậu cây nhựa tái chế", 25 },
                    { 11, "893000000011", 6, 30000m, "Cải xanh hữu cơ", 40 },
                    { 12, "893000000012", 6, 28000m, "Cà rốt hữu cơ", 45 },
                    { 13, "893000000013", 7, 65000m, "Táo hữu cơ", 35 },
                    { 14, "893000000014", 7, 40000m, "Chuối hữu cơ", 50 },
                    { 15, "893000000015", 8, 90000m, "Yến mạch nguyên hạt", 30 },
                    { 16, "893000000016", 8, 75000m, "Hạt chia", 35 },
                    { 17, "893000000017", 9, 95000m, "Nấm hương khô", 25 },
                    { 18, "893000000018", 9, 70000m, "Mộc nhĩ khô", 30 },
                    { 19, "893000000019", 10, 20000m, "Muối biển tự nhiên", 60 },
                    { 20, "893000000020", 10, 45000m, "Tiêu đen hữu cơ", 50 },
                    { 21, "893000000021", 11, 95000m, "Hộp đựng thực phẩm thủy tinh", 30 },
                    { 22, "893000000022", 11, 25000m, "Muỗng tre", 70 },
                    { 23, "893000000023", 12, 40000m, "Khăn tay cotton", 45 },
                    { 24, "893000000024", 12, 85000m, "Bình nước cá nhân", 40 },
                    { 25, "893000000025", 13, 35000m, "Sổ tay tái chế", 50 },
                    { 26, "893000000026", 13, 15000m, "Bút thân thiện môi trường", 80 },
                    { 27, "893000000027", 14, 65000m, "Nước lau sàn sinh học", 35 },
                    { 28, "893000000028", 14, 60000m, "Nước rửa chén sinh học", 40 },
                    { 29, "893000000029", 15, 15000m, "Túi giấy thân thiện môi trường", 100 },
                    { 30, "893000000030", 15, 180000m, "Bình giữ nhiệt inox", 25 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 85000m, 85000m },
                    { 2, 1, 2, 1, 120000m, 120000m },
                    { 3, 2, 3, 2, 70000m, 35000m },
                    { 4, 2, 4, 1, 45000m, 45000m },
                    { 5, 3, 5, 1, 55000m, 55000m },
                    { 6, 3, 6, 1, 110000m, 110000m },
                    { 7, 4, 7, 1, 45000m, 45000m },
                    { 8, 4, 8, 2, 50000m, 25000m },
                    { 9, 5, 9, 1, 35000m, 35000m },
                    { 10, 5, 10, 1, 75000m, 75000m },
                    { 11, 6, 11, 2, 60000m, 30000m },
                    { 12, 6, 12, 1, 28000m, 28000m },
                    { 13, 7, 13, 2, 130000m, 65000m },
                    { 14, 7, 14, 1, 40000m, 40000m },
                    { 15, 8, 15, 1, 90000m, 90000m },
                    { 16, 8, 16, 1, 75000m, 75000m },
                    { 17, 9, 19, 2, 90000m, 45000m },
                    { 18, 9, 21, 1, 95000m, 95000m },
                    { 19, 10, 29, 1, 15000m, 15000m },
                    { 20, 10, 30, 1, 180000m, 180000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
