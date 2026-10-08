using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    // DbContext đại diện cho phiên làm việc với cơ sở dữ liệu SQL Server
    public class SupermarketDbContext : DbContext
    {
        // Khởi tạo DbContext
        public SupermarketDbContext(
            DbContextOptions<SupermarketDbContext> options)
            : base(options)
        {
        }

        // ============================================================
        // KHAI BÁO CÁC BẢNG DỮ LIỆU
        // ============================================================

        // Bảng danh mục
        public DbSet<Category> Categories { get; set; }

        // Bảng sản phẩm
        public DbSet<Product> Products { get; set; }

        // Bảng khách hàng
        public DbSet<Customer> Customers { get; set; }

        // Bảng đơn hàng
        public DbSet<Order> Orders { get; set; }

        // Bảng chi tiết đơn hàng
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // Bảng người dùng
        public DbSet<User> Users { get; set; }


        // ============================================================
        // CẤU HÌNH DỮ LIỆU MỒI BAN ĐẦU
        // ============================================================

        // Cấu hình dữ liệu mẫu ban đầu cho database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ========================================================
            // 1. SEED 15 DANH MỤC
            // ========================================================

            modelBuilder.Entity<Category>().HasData(

                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Thực phẩm hữu cơ",
                    Description = "Các sản phẩm thực phẩm hữu cơ, tự nhiên"
                },

                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Đồ uống thiên nhiên",
                    Description = "Nước uống, trà và các sản phẩm có nguồn gốc tự nhiên"
                },

                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Sản phẩm chăm sóc cá nhân",
                    Description = "Các sản phẩm chăm sóc cá nhân thân thiện với môi trường"
                },

                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Đồ dùng gia đình xanh",
                    Description = "Đồ dùng gia đình có thể tái sử dụng"
                },

                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Sản phẩm tái chế",
                    Description = "Các sản phẩm được làm từ vật liệu tái chế"
                },

                new Category
                {
                    CategoryId = 6,
                    CategoryName = "Rau củ hữu cơ",
                    Description = "Các loại rau củ được trồng theo phương pháp hữu cơ"
                },

                new Category
                {
                    CategoryId = 7,
                    CategoryName = "Trái cây hữu cơ",
                    Description = "Các loại trái cây sạch và tự nhiên"
                },

                new Category
                {
                    CategoryId = 8,
                    CategoryName = "Ngũ cốc",
                    Description = "Các loại ngũ cốc và hạt dinh dưỡng"
                },

                new Category
                {
                    CategoryId = 9,
                    CategoryName = "Thực phẩm khô",
                    Description = "Các loại thực phẩm khô tiện dụng"
                },

                new Category
                {
                    CategoryId = 10,
                    CategoryName = "Gia vị tự nhiên",
                    Description = "Các loại gia vị có nguồn gốc tự nhiên"
                },

                new Category
                {
                    CategoryId = 11,
                    CategoryName = "Đồ dùng nhà bếp",
                    Description = "Các sản phẩm sử dụng trong nhà bếp"
                },

                new Category
                {
                    CategoryId = 12,
                    CategoryName = "Đồ dùng cá nhân",
                    Description = "Các sản phẩm phục vụ nhu cầu cá nhân"
                },

                new Category
                {
                    CategoryId = 13,
                    CategoryName = "Văn phòng phẩm xanh",
                    Description = "Văn phòng phẩm thân thiện với môi trường"
                },

                new Category
                {
                    CategoryId = 14,
                    CategoryName = "Sản phẩm vệ sinh",
                    Description = "Các sản phẩm vệ sinh an toàn"
                },

                new Category
                {
                    CategoryId = 15,
                    CategoryName = "Sản phẩm thân thiện môi trường",
                    Description = "Các sản phẩm góp phần bảo vệ môi trường"
                }
            );


            // ========================================================
            // 2. SEED 30 SẢN PHẨM
            // Mỗi danh mục có 2 sản phẩm
            // ========================================================

            modelBuilder.Entity<Product>().HasData(

                // Category 1
                new Product
                {
                    ProductId = 1,
                    Barcode = "893000000001",
                    ProductName = "Gạo lứt hữu cơ",
                    Price = 85000,
                    StockQuantity = 50,
                    CategoryId = 1
                },

                new Product
                {
                    ProductId = 2,
                    Barcode = "893000000002",
                    ProductName = "Mật ong nguyên chất",
                    Price = 120000,
                    StockQuantity = 30,
                    CategoryId = 1
                },

                // Category 2
                new Product
                {
                    ProductId = 3,
                    Barcode = "893000000003",
                    ProductName = "Nước ép cam nguyên chất",
                    Price = 35000,
                    StockQuantity = 60,
                    CategoryId = 2
                },

                new Product
                {
                    ProductId = 4,
                    Barcode = "893000000004",
                    ProductName = "Trà xanh thiên nhiên",
                    Price = 45000,
                    StockQuantity = 45,
                    CategoryId = 2
                },

                // Category 3
                new Product
                {
                    ProductId = 5,
                    Barcode = "893000000005",
                    ProductName = "Xà phòng thiên nhiên",
                    Price = 55000,
                    StockQuantity = 35,
                    CategoryId = 3
                },

                new Product
                {
                    ProductId = 6,
                    Barcode = "893000000006",
                    ProductName = "Dầu gội thảo mộc",
                    Price = 110000,
                    StockQuantity = 25,
                    CategoryId = 3
                },

                // Category 4
                new Product
                {
                    ProductId = 7,
                    Barcode = "893000000007",
                    ProductName = "Túi vải tái sử dụng",
                    Price = 45000,
                    StockQuantity = 70,
                    CategoryId = 4
                },

                new Product
                {
                    ProductId = 8,
                    Barcode = "893000000008",
                    ProductName = "Ống hút tre",
                    Price = 25000,
                    StockQuantity = 80,
                    CategoryId = 4
                },

                // Category 5
                new Product
                {
                    ProductId = 9,
                    Barcode = "893000000009",
                    ProductName = "Sổ tay giấy tái chế",
                    Price = 35000,
                    StockQuantity = 50,
                    CategoryId = 5
                },

                new Product
                {
                    ProductId = 10,
                    Barcode = "893000000010",
                    ProductName = "Chậu cây nhựa tái chế",
                    Price = 75000,
                    StockQuantity = 25,
                    CategoryId = 5
                },

                // Category 6
                new Product
                {
                    ProductId = 11,
                    Barcode = "893000000011",
                    ProductName = "Cải xanh hữu cơ",
                    Price = 30000,
                    StockQuantity = 40,
                    CategoryId = 6
                },

                new Product
                {
                    ProductId = 12,
                    Barcode = "893000000012",
                    ProductName = "Cà rốt hữu cơ",
                    Price = 28000,
                    StockQuantity = 45,
                    CategoryId = 6
                },

                // Category 7
                new Product
                {
                    ProductId = 13,
                    Barcode = "893000000013",
                    ProductName = "Táo hữu cơ",
                    Price = 65000,
                    StockQuantity = 35,
                    CategoryId = 7
                },

                new Product
                {
                    ProductId = 14,
                    Barcode = "893000000014",
                    ProductName = "Chuối hữu cơ",
                    Price = 40000,
                    StockQuantity = 50,
                    CategoryId = 7
                },

                // Category 8
                new Product
                {
                    ProductId = 15,
                    Barcode = "893000000015",
                    ProductName = "Yến mạch nguyên hạt",
                    Price = 90000,
                    StockQuantity = 30,
                    CategoryId = 8
                },

                new Product
                {
                    ProductId = 16,
                    Barcode = "893000000016",
                    ProductName = "Hạt chia",
                    Price = 75000,
                    StockQuantity = 35,
                    CategoryId = 8
                },

                // Category 9
                new Product
                {
                    ProductId = 17,
                    Barcode = "893000000017",
                    ProductName = "Nấm hương khô",
                    Price = 95000,
                    StockQuantity = 25,
                    CategoryId = 9
                },

                new Product
                {
                    ProductId = 18,
                    Barcode = "893000000018",
                    ProductName = "Mộc nhĩ khô",
                    Price = 70000,
                    StockQuantity = 30,
                    CategoryId = 9
                },

                // Category 10
                new Product
                {
                    ProductId = 19,
                    Barcode = "893000000019",
                    ProductName = "Muối biển tự nhiên",
                    Price = 20000,
                    StockQuantity = 60,
                    CategoryId = 10
                },

                new Product
                {
                    ProductId = 20,
                    Barcode = "893000000020",
                    ProductName = "Tiêu đen hữu cơ",
                    Price = 45000,
                    StockQuantity = 50,
                    CategoryId = 10
                },

                // Category 11
                new Product
                {
                    ProductId = 21,
                    Barcode = "893000000021",
                    ProductName = "Hộp đựng thực phẩm thủy tinh",
                    Price = 95000,
                    StockQuantity = 30,
                    CategoryId = 11
                },

                new Product
                {
                    ProductId = 22,
                    Barcode = "893000000022",
                    ProductName = "Muỗng tre",
                    Price = 25000,
                    StockQuantity = 70,
                    CategoryId = 11
                },

                // Category 12
                new Product
                {
                    ProductId = 23,
                    Barcode = "893000000023",
                    ProductName = "Khăn tay cotton",
                    Price = 40000,
                    StockQuantity = 45,
                    CategoryId = 12
                },

                new Product
                {
                    ProductId = 24,
                    Barcode = "893000000024",
                    ProductName = "Bình nước cá nhân",
                    Price = 85000,
                    StockQuantity = 40,
                    CategoryId = 12
                },

                // Category 13
                new Product
                {
                    ProductId = 25,
                    Barcode = "893000000025",
                    ProductName = "Sổ tay tái chế",
                    Price = 35000,
                    StockQuantity = 50,
                    CategoryId = 13
                },

                new Product
                {
                    ProductId = 26,
                    Barcode = "893000000026",
                    ProductName = "Bút thân thiện môi trường",
                    Price = 15000,
                    StockQuantity = 80,
                    CategoryId = 13
                },

                // Category 14
                new Product
                {
                    ProductId = 27,
                    Barcode = "893000000027",
                    ProductName = "Nước lau sàn sinh học",
                    Price = 65000,
                    StockQuantity = 35,
                    CategoryId = 14
                },

                new Product
                {
                    ProductId = 28,
                    Barcode = "893000000028",
                    ProductName = "Nước rửa chén sinh học",
                    Price = 60000,
                    StockQuantity = 40,
                    CategoryId = 14
                },

                // Category 15
                new Product
                {
                    ProductId = 29,
                    Barcode = "893000000029",
                    ProductName = "Túi giấy thân thiện môi trường",
                    Price = 15000,
                    StockQuantity = 100,
                    CategoryId = 15
                },

                new Product
                {
                    ProductId = 30,
                    Barcode = "893000000030",
                    ProductName = "Bình giữ nhiệt inox",
                    Price = 180000,
                    StockQuantity = 25,
                    CategoryId = 15
                }
            );


            // ========================================================
            // 3. SEED 15 KHÁCH HÀNG
            // ========================================================

            modelBuilder.Entity<Customer>().HasData(

                new Customer
                {
                    CustomerId = 1,
                    FullName = "Nguyễn Văn An",
                    PhoneNumber = "0900000001",
                    Email = "nguyenvanan@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 2,
                    FullName = "Trần Thị Bình",
                    PhoneNumber = "0900000002",
                    Email = "tranthibinh@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 3,
                    FullName = "Lê Văn Cường",
                    PhoneNumber = "0900000003",
                    Email = "levancuong@gmail.com",
                    Address = "Bình Dương"
                },

                new Customer
                {
                    CustomerId = 4,
                    FullName = "Phạm Thị Dung",
                    PhoneNumber = "0900000004",
                    Email = "phamthidung@gmail.com",
                    Address = "Đồng Nai"
                },

                new Customer
                {
                    CustomerId = 5,
                    FullName = "Hoàng Văn Minh",
                    PhoneNumber = "0900000005",
                    Email = "hoangvanminh@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 6,
                    FullName = "Nguyễn Thị Lan",
                    PhoneNumber = "0900000006",
                    Email = "nguyenthilan@gmail.com",
                    Address = "Long An"
                },

                new Customer
                {
                    CustomerId = 7,
                    FullName = "Trần Văn Hùng",
                    PhoneNumber = "0900000007",
                    Email = "tranvanhung@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 8,
                    FullName = "Lê Thị Mai",
                    PhoneNumber = "0900000008",
                    Email = "lethimai@gmail.com",
                    Address = "Bình Dương"
                },

                new Customer
                {
                    CustomerId = 9,
                    FullName = "Phạm Văn Nam",
                    PhoneNumber = "0900000009",
                    Email = "phamvannam@gmail.com",
                    Address = "Đồng Nai"
                },

                new Customer
                {
                    CustomerId = 10,
                    FullName = "Hoàng Thị Hoa",
                    PhoneNumber = "0900000010",
                    Email = "hoangthihoa@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 11,
                    FullName = "Đỗ Văn Thành",
                    PhoneNumber = "0900000011",
                    Email = "dovanthanh@gmail.com",
                    Address = "Tây Ninh"
                },

                new Customer
                {
                    CustomerId = 12,
                    FullName = "Vũ Thị Hương",
                    PhoneNumber = "0900000012",
                    Email = "vuthihuong@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 13,
                    FullName = "Nguyễn Văn Long",
                    PhoneNumber = "0900000013",
                    Email = "nguyenvanlong@gmail.com",
                    Address = "Bà Rịa - Vũng Tàu"
                },

                new Customer
                {
                    CustomerId = 14,
                    FullName = "Trần Thị Ngọc",
                    PhoneNumber = "0900000014",
                    Email = "tranthingoc@gmail.com",
                    Address = "TP. Hồ Chí Minh"
                },

                new Customer
                {
                    CustomerId = 15,
                    FullName = "Lê Văn Phúc",
                    PhoneNumber = "0900000015",
                    Email = "levanphuc@gmail.com",
                    Address = "Bình Phước"
                }
            );


            // ========================================================
            // 4. SEED 3 USER
            // ========================================================

            modelBuilder.Entity<User>().HasData(

                new User
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "123456",
                    FullName = "Quản trị viên",
                    Role = "Admin",
                    IsActive = true
                },

                new User
                {
                    UserId = 2,
                    Username = "cashier1",
                    Password = "123456",
                    FullName = "Nguyễn Văn Thuận",
                    Role = "Cashier",
                    IsActive = true
                },

                new User
                {
                    UserId = 3,
                    Username = "cashier2",
                    Password = "123456",
                    FullName = "Trần Văn Nam",
                    Role = "Cashier",
                    IsActive = true
                }
            );


            // ========================================================
            // 5. SEED 10 ĐƠN HÀNG
            // ========================================================

            modelBuilder.Entity<Order>().HasData(

                new Order
                {
                    OrderId = 1,
                    CustomerId = 1,
                    UserId = 1,
                    OrderDate = new DateTime(2026, 10, 1, 9, 0, 0),
                    TotalAmount = 205000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 2,
                    CustomerId = 2,
                    UserId = 2,
                    OrderDate = new DateTime(2026, 10, 1, 10, 0, 0),
                    TotalAmount = 115000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 3,
                    CustomerId = 3,
                    UserId = 2,
                    OrderDate = new DateTime(2026, 10, 2, 11, 0, 0),
                    TotalAmount = 165000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 4,
                    CustomerId = 4,
                    UserId = 3,
                    OrderDate = new DateTime(2026, 10, 2, 14, 0, 0),
                    TotalAmount = 95000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 5,
                    CustomerId = 5,
                    UserId = 1,
                    OrderDate = new DateTime(2026, 10, 3, 9, 30, 0),
                    TotalAmount = 110000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 6,
                    CustomerId = 1,
                    UserId = 2,
                    OrderDate = new DateTime(2026, 10, 3, 13, 0, 0),
                    TotalAmount = 88000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 7,
                    CustomerId = 2,
                    UserId = 3,
                    OrderDate = new DateTime(2026, 10, 4, 10, 30, 0),
                    TotalAmount = 170000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 8,
                    CustomerId = 3,
                    UserId = 1,
                    OrderDate = new DateTime(2026, 10, 5, 15, 0, 0),
                    TotalAmount = 165000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 9,
                    CustomerId = 4,
                    UserId = 2,
                    OrderDate = new DateTime(2026, 10, 6, 16, 0, 0),
                    TotalAmount = 185000,
                    Status = "Completed"
                },

                new Order
                {
                    OrderId = 10,
                    CustomerId = 5,
                    UserId = 3,
                    OrderDate = new DateTime(2026, 10, 7, 17, 0, 0),
                    TotalAmount = 195000,
                    Status = "Completed"
                }
            );


            // ========================================================
            // 6. SEED 20 CHI TIẾT ĐƠN HÀNG
            // Mỗi đơn hàng có 2 sản phẩm
            // ========================================================

            modelBuilder.Entity<OrderDetail>().HasData(

                // Order 1
                new OrderDetail
                {
                    OrderDetailId = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1,
                    UnitPrice = 85000,
                    TotalPrice = 85000
                },

                new OrderDetail
                {
                    OrderDetailId = 2,
                    OrderId = 1,
                    ProductId = 2,
                    Quantity = 1,
                    UnitPrice = 120000,
                    TotalPrice = 120000
                },

                // Order 2
                new OrderDetail
                {
                    OrderDetailId = 3,
                    OrderId = 2,
                    ProductId = 3,
                    Quantity = 2,
                    UnitPrice = 35000,
                    TotalPrice = 70000
                },

                new OrderDetail
                {
                    OrderDetailId = 4,
                    OrderId = 2,
                    ProductId = 4,
                    Quantity = 1,
                    UnitPrice = 45000,
                    TotalPrice = 45000
                },

                // Order 3
                new OrderDetail
                {
                    OrderDetailId = 5,
                    OrderId = 3,
                    ProductId = 5,
                    Quantity = 1,
                    UnitPrice = 55000,
                    TotalPrice = 55000
                },

                new OrderDetail
                {
                    OrderDetailId = 6,
                    OrderId = 3,
                    ProductId = 6,
                    Quantity = 1,
                    UnitPrice = 110000,
                    TotalPrice = 110000
                },

                // Order 4
                new OrderDetail
                {
                    OrderDetailId = 7,
                    OrderId = 4,
                    ProductId = 7,
                    Quantity = 1,
                    UnitPrice = 45000,
                    TotalPrice = 45000
                },

                new OrderDetail
                {
                    OrderDetailId = 8,
                    OrderId = 4,
                    ProductId = 8,
                    Quantity = 2,
                    UnitPrice = 25000,
                    TotalPrice = 50000
                },

                // Order 5
                new OrderDetail
                {
                    OrderDetailId = 9,
                    OrderId = 5,
                    ProductId = 9,
                    Quantity = 1,
                    UnitPrice = 35000,
                    TotalPrice = 35000
                },

                new OrderDetail
                {
                    OrderDetailId = 10,
                    OrderId = 5,
                    ProductId = 10,
                    Quantity = 1,
                    UnitPrice = 75000,
                    TotalPrice = 75000
                },

                // Order 6
                new OrderDetail
                {
                    OrderDetailId = 11,
                    OrderId = 6,
                    ProductId = 11,
                    Quantity = 2,
                    UnitPrice = 30000,
                    TotalPrice = 60000
                },

                new OrderDetail
                {
                    OrderDetailId = 12,
                    OrderId = 6,
                    ProductId = 12,
                    Quantity = 1,
                    UnitPrice = 28000,
                    TotalPrice = 28000
                },

                // Order 7
                new OrderDetail
                {
                    OrderDetailId = 13,
                    OrderId = 7,
                    ProductId = 13,
                    Quantity = 2,
                    UnitPrice = 65000,
                    TotalPrice = 130000
                },

                new OrderDetail
                {
                    OrderDetailId = 14,
                    OrderId = 7,
                    ProductId = 14,
                    Quantity = 1,
                    UnitPrice = 40000,
                    TotalPrice = 40000
                },

                // Order 8
                new OrderDetail
                {
                    OrderDetailId = 15,
                    OrderId = 8,
                    ProductId = 15,
                    Quantity = 1,
                    UnitPrice = 90000,
                    TotalPrice = 90000
                },

                new OrderDetail
                {
                    OrderDetailId = 16,
                    OrderId = 8,
                    ProductId = 16,
                    Quantity = 1,
                    UnitPrice = 75000,
                    TotalPrice = 75000
                },

                // Order 9
                new OrderDetail
                {
                    OrderDetailId = 17,
                    OrderId = 9,
                    ProductId = 19,
                    Quantity = 2,
                    UnitPrice = 45000,
                    TotalPrice = 90000
                },

                new OrderDetail
                {
                    OrderDetailId = 18,
                    OrderId = 9,
                    ProductId = 21,
                    Quantity = 1,
                    UnitPrice = 95000,
                    TotalPrice = 95000
                },

                // Order 10
                new OrderDetail
                {
                    OrderDetailId = 19,
                    OrderId = 10,
                    ProductId = 29,
                    Quantity = 1,
                    UnitPrice = 15000,
                    TotalPrice = 15000
                },

                new OrderDetail
                {
                    OrderDetailId = 20,
                    OrderId = 10,
                    ProductId = 30,
                    Quantity = 1,
                    UnitPrice = 180000,
                    TotalPrice = 180000
                }
            );
        }
    }
}