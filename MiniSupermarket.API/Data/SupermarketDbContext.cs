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

        // Bảng nhóm hàng
        public DbSet<NhomHang> NhomHangs { get; set; }

        // Bảng sản phẩm
        public DbSet<SanPham> SanPhams { get; set; }

        // Bảng khách hàng
        public DbSet<KhachHang> KhachHangs { get; set; }

        // Bảng đơn hàng
        public DbSet<DonHang> DonHangs { get; set; }

        // Bảng chi tiết đơn hàng
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        // Bảng người dùng
        public DbSet<NguoiDung> NguoiDungs { get; set; }


        // ============================================================
        // CẤU HÌNH DỮ LIỆU MỒI BAN ĐẦU
        // ============================================================

        // Cấu hình dữ liệu mẫu ban đầu cho database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ========================================================
            // 1. SEED 15 NHÓM HÀNG
            // ========================================================

            modelBuilder.Entity<NhomHang>().HasData(

                new NhomHang
                {
                    MaNhomHang = 1,
                    TenNhomHang = "Thực phẩm hữu cơ",
                    MoTa = "Các sản phẩm thực phẩm hữu cơ, tự nhiên"
                },

                new NhomHang
                {
                    MaNhomHang = 2,
                    TenNhomHang = "Đồ uống thiên nhiên",
                    MoTa = "Nước uống, trà và các sản phẩm có nguồn gốc tự nhiên"
                },

                new NhomHang
                {
                    MaNhomHang = 3,
                    TenNhomHang = "Sản phẩm chăm sóc cá nhân",
                    MoTa = "Các sản phẩm chăm sóc cá nhân thân thiện với môi trường"
                },

                new NhomHang
                {
                    MaNhomHang = 4,
                    TenNhomHang = "Đồ dùng gia đình xanh",
                    MoTa = "Đồ dùng gia đình có thể tái sử dụng"
                },

                new NhomHang
                {
                    MaNhomHang = 5,
                    TenNhomHang = "Sản phẩm tái chế",
                    MoTa = "Các sản phẩm được làm từ vật liệu tái chế"
                },

                new NhomHang
                {
                    MaNhomHang = 6,
                    TenNhomHang = "Rau củ hữu cơ",
                    MoTa = "Các loại rau củ được trồng theo phương pháp hữu cơ"
                },

                new NhomHang
                {
                    MaNhomHang = 7,
                    TenNhomHang = "Trái cây hữu cơ",
                    MoTa = "Các loại trái cây sạch và tự nhiên"
                },

                new NhomHang
                {
                    MaNhomHang = 8,
                    TenNhomHang = "Ngũ cốc",
                    MoTa = "Các loại ngũ cốc và hạt dinh dưỡng"
                },

                new NhomHang
                {
                    MaNhomHang = 9,
                    TenNhomHang = "Thực phẩm khô",
                    MoTa = "Các loại thực phẩm khô tiện dụng"
                },

                new NhomHang
                {
                    MaNhomHang = 10,
                    TenNhomHang = "Gia vị tự nhiên",
                    MoTa = "Các loại gia vị có nguồn gốc tự nhiên"
                },

                new NhomHang
                {
                    MaNhomHang = 11,
                    TenNhomHang = "Đồ dùng nhà bếp",
                    MoTa = "Các sản phẩm sử dụng trong nhà bếp"
                },

                new NhomHang
                {
                    MaNhomHang = 12,
                    TenNhomHang = "Đồ dùng cá nhân",
                    MoTa = "Các sản phẩm phục vụ nhu cầu cá nhân"
                },

                new NhomHang
                {
                    MaNhomHang = 13,
                    TenNhomHang = "Văn phòng phẩm xanh",
                    MoTa = "Văn phòng phẩm thân thiện với môi trường"
                },

                new NhomHang
                {
                    MaNhomHang = 14,
                    TenNhomHang = "Sản phẩm vệ sinh",
                    MoTa = "Các sản phẩm vệ sinh an toàn"
                },

                new NhomHang
                {
                    MaNhomHang = 15,
                    TenNhomHang = "Sản phẩm thân thiện môi trường",
                    MoTa = "Các sản phẩm góp phần bảo vệ môi trường"
                }
            );


            // ========================================================
            // 2. SEED 30 SẢN PHẨM
            // Mỗi nhóm hàng có 2 sản phẩm
            // ========================================================

            modelBuilder.Entity<SanPham>().HasData(

                // Nhóm hàng 1
                new SanPham
                {
                    MaSanPham = 1,
                    MaVach = "893000000001",
                    TenSanPham = "Gạo lứt hữu cơ",
                    GiaBan = 85000,
                    SoLuongTon = 50,
                    MaNhomHang = 1
                },

                new SanPham
                {
                    MaSanPham = 2,
                    MaVach = "893000000002",
                    TenSanPham = "Mật ong nguyên chất",
                    GiaBan = 120000,
                    SoLuongTon = 30,
                    MaNhomHang = 1
                },

                // Nhóm hàng 2
                new SanPham
                {
                    MaSanPham = 3,
                    MaVach = "893000000003",
                    TenSanPham = "Nước ép cam nguyên chất",
                    GiaBan = 35000,
                    SoLuongTon = 60,
                    MaNhomHang = 2
                },

                new SanPham
                {
                    MaSanPham = 4,
                    MaVach = "893000000004",
                    TenSanPham = "Trà xanh thiên nhiên",
                    GiaBan = 45000,
                    SoLuongTon = 45,
                    MaNhomHang = 2
                },

                // Nhóm hàng 3
                new SanPham
                {
                    MaSanPham = 5,
                    MaVach = "893000000005",
                    TenSanPham = "Xà phòng thiên nhiên",
                    GiaBan = 55000,
                    SoLuongTon = 35,
                    MaNhomHang = 3
                },

                new SanPham
                {
                    MaSanPham = 6,
                    MaVach = "893000000006",
                    TenSanPham = "Dầu gội thảo mộc",
                    GiaBan = 110000,
                    SoLuongTon = 25,
                    MaNhomHang = 3
                },

                // Nhóm hàng 4
                new SanPham
                {
                    MaSanPham = 7,
                    MaVach = "893000000007",
                    TenSanPham = "Túi vải tái sử dụng",
                    GiaBan = 45000,
                    SoLuongTon = 70,
                    MaNhomHang = 4
                },

                new SanPham
                {
                    MaSanPham = 8,
                    MaVach = "893000000008",
                    TenSanPham = "Ống hút tre",
                    GiaBan = 25000,
                    SoLuongTon = 80,
                    MaNhomHang = 4
                },

                // Nhóm hàng 5
                new SanPham
                {
                    MaSanPham = 9,
                    MaVach = "893000000009",
                    TenSanPham = "Sổ tay giấy tái chế",
                    GiaBan = 35000,
                    SoLuongTon = 50,
                    MaNhomHang = 5
                },

                new SanPham
                {
                    MaSanPham = 10,
                    MaVach = "893000000010",
                    TenSanPham = "Chậu cây nhựa tái chế",
                    GiaBan = 75000,
                    SoLuongTon = 25,
                    MaNhomHang = 5
                },

                // Nhóm hàng 6
                new SanPham
                {
                    MaSanPham = 11,
                    MaVach = "893000000011",
                    TenSanPham = "Cải xanh hữu cơ",
                    GiaBan = 30000,
                    SoLuongTon = 40,
                    MaNhomHang = 6
                },

                new SanPham
                {
                    MaSanPham = 12,
                    MaVach = "893000000012",
                    TenSanPham = "Cà rốt hữu cơ",
                    GiaBan = 28000,
                    SoLuongTon = 45,
                    MaNhomHang = 6
                },

                // Nhóm hàng 7
                new SanPham
                {
                    MaSanPham = 13,
                    MaVach = "893000000013",
                    TenSanPham = "Táo hữu cơ",
                    GiaBan = 65000,
                    SoLuongTon = 35,
                    MaNhomHang = 7
                },

                new SanPham
                {
                    MaSanPham = 14,
                    MaVach = "893000000014",
                    TenSanPham = "Chuối hữu cơ",
                    GiaBan = 40000,
                    SoLuongTon = 50,
                    MaNhomHang = 7
                },

                // Nhóm hàng 8
                new SanPham
                {
                    MaSanPham = 15,
                    MaVach = "893000000015",
                    TenSanPham = "Yến mạch nguyên hạt",
                    GiaBan = 90000,
                    SoLuongTon = 30,
                    MaNhomHang = 8
                },

                new SanPham
                {
                    MaSanPham = 16,
                    MaVach = "893000000016",
                    TenSanPham = "Hạt chia",
                    GiaBan = 75000,
                    SoLuongTon = 35,
                    MaNhomHang = 8
                },

                // Nhóm hàng 9
                new SanPham
                {
                    MaSanPham = 17,
                    MaVach = "893000000017",
                    TenSanPham = "Nấm hương khô",
                    GiaBan = 95000,
                    SoLuongTon = 25,
                    MaNhomHang = 9
                },

                new SanPham
                {
                    MaSanPham = 18,
                    MaVach = "893000000018",
                    TenSanPham = "Mộc nhĩ khô",
                    GiaBan = 70000,
                    SoLuongTon = 30,
                    MaNhomHang = 9
                },

                // Nhóm hàng 10
                new SanPham
                {
                    MaSanPham = 19,
                    MaVach = "893000000019",
                    TenSanPham = "Muối biển tự nhiên",
                    GiaBan = 20000,
                    SoLuongTon = 60,
                    MaNhomHang = 10
                },

                new SanPham
                {
                    MaSanPham = 20,
                    MaVach = "893000000020",
                    TenSanPham = "Tiêu đen hữu cơ",
                    GiaBan = 45000,
                    SoLuongTon = 50,
                    MaNhomHang = 10
                },

                // Nhóm hàng 11
                new SanPham
                {
                    MaSanPham = 21,
                    MaVach = "893000000021",
                    TenSanPham = "Hộp đựng thực phẩm thủy tinh",
                    GiaBan = 95000,
                    SoLuongTon = 30,
                    MaNhomHang = 11
                },

                new SanPham
                {
                    MaSanPham = 22,
                    MaVach = "893000000022",
                    TenSanPham = "Muỗng tre",
                    GiaBan = 25000,
                    SoLuongTon = 70,
                    MaNhomHang = 11
                },

                // Nhóm hàng 12
                new SanPham
                {
                    MaSanPham = 23,
                    MaVach = "893000000023",
                    TenSanPham = "Khăn tay cotton",
                    GiaBan = 40000,
                    SoLuongTon = 45,
                    MaNhomHang = 12
                },

                new SanPham
                {
                    MaSanPham = 24,
                    MaVach = "893000000024",
                    TenSanPham = "Bình nước cá nhân",
                    GiaBan = 85000,
                    SoLuongTon = 40,
                    MaNhomHang = 12
                },

                // Nhóm hàng 13
                new SanPham
                {
                    MaSanPham = 25,
                    MaVach = "893000000025",
                    TenSanPham = "Sổ tay tái chế",
                    GiaBan = 35000,
                    SoLuongTon = 50,
                    MaNhomHang = 13
                },

                new SanPham
                {
                    MaSanPham = 26,
                    MaVach = "893000000026",
                    TenSanPham = "Bút thân thiện môi trường",
                    GiaBan = 15000,
                    SoLuongTon = 80,
                    MaNhomHang = 13
                },

                // Nhóm hàng 14
                new SanPham
                {
                    MaSanPham = 27,
                    MaVach = "893000000027",
                    TenSanPham = "Nước lau sàn sinh học",
                    GiaBan = 65000,
                    SoLuongTon = 35,
                    MaNhomHang = 14
                },

                new SanPham
                {
                    MaSanPham = 28,
                    MaVach = "893000000028",
                    TenSanPham = "Nước rửa chén sinh học",
                    GiaBan = 60000,
                    SoLuongTon = 40,
                    MaNhomHang = 14
                },

                // Nhóm hàng 15
                new SanPham
                {
                    MaSanPham = 29,
                    MaVach = "893000000029",
                    TenSanPham = "Túi giấy thân thiện môi trường",
                    GiaBan = 15000,
                    SoLuongTon = 100,
                    MaNhomHang = 15
                },

                new SanPham
                {
                    MaSanPham = 30,
                    MaVach = "893000000030",
                    TenSanPham = "Bình giữ nhiệt inox",
                    GiaBan = 180000,
                    SoLuongTon = 25,
                    MaNhomHang = 15
                }
            );


            // ========================================================
            // 3. SEED 15 KHÁCH HÀNG
            // ========================================================

            modelBuilder.Entity<KhachHang>().HasData(

                new KhachHang
                {
                    MaKhachHang = 1,
                    HoTen = "Nguyễn Văn An",
                    SoDienThoai = "0900000001",
                    Email = "nguyenvanan@gmail.com",
                    DiaChi = "25, đường Nguyễn Văn Cừ, phường Nguyễn Cư Trinh, quận 1, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 2,
                    HoTen = "Trần Thị Bình",
                    SoDienThoai = "0900000002",
                    Email = "tranthibinh@gmail.com",
                    DiaChi = "18, đường Lê Lợi, phường Bến Nghé, quận 1, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 3,
                    HoTen = "Lê Văn Cường",
                    SoDienThoai = "0900000003",
                    Email = "levancuong@gmail.com",
                    DiaChi = "42, đường Phạm Văn Đồng, phường Hiệp Bình Chánh, thành phố Thủ Đức, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 4,
                    HoTen = "Phạm Thị Dung",
                    SoDienThoai = "0900000004",
                    Email = "phamthidung@gmail.com",
                    DiaChi = "76, đường Đồng Khởi, phường Bến Nghé, quận 1, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 5,
                    HoTen = "Hoàng Văn Minh",
                    SoDienThoai = "0900000005",
                    Email = "hoangvanminh@gmail.com",
                    DiaChi = "12, đường Nguyễn Trãi, phường Bến Thành, quận 1, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 6,
                    HoTen = "Nguyễn Thị Lan",
                    SoDienThoai = "0900000006",
                    Email = "nguyenthilan@gmail.com",
                    DiaChi = "35, đường Nguyễn Huệ, phường Bến Nghé, quận 1, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 7,
                    HoTen = "Trần Văn Hùng",
                    SoDienThoai = "0900000007",
                    Email = "tranvanhung@gmail.com",
                    DiaChi = "89, đường Xô Viết Nghệ Tĩnh, phường 21, quận Bình Thạnh, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 8,
                    HoTen = "Lê Thị Mai",
                    SoDienThoai = "0900000008",
                    Email = "lethimai@gmail.com",
                    DiaChi = "56, đường Võ Văn Ngân, phường Linh Chiểu, thành phố Thủ Đức, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 9,
                    HoTen = "Phạm Văn Nam",
                    SoDienThoai = "0900000009",
                    Email = "phamvannam@gmail.com",
                    DiaChi = "23, đường Nguyễn Văn Linh, phường Tân Phong, quận 7, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 10,
                    HoTen = "Hoàng Thị Hoa",
                    SoDienThoai = "0900000010",
                    Email = "hoangthihoa@gmail.com",
                    DiaChi = "101, đường Cách Mạng Tháng Tám, phường 7, quận 3, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 11,
                    HoTen = "Đỗ Văn Thành",
                    SoDienThoai = "0900000011",
                    Email = "dovanthanh@gmail.com",
                    DiaChi = "64, đường Lê Văn Việt, phường Tăng Nhơn Phú A, thành phố Thủ Đức, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 12,
                    HoTen = "Vũ Thị Hương",
                    SoDienThoai = "0900000012",
                    Email = "vuthihuong@gmail.com",
                    DiaChi = "28, đường Hoàng Văn Thụ, phường 4, quận Tân Bình, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 13,
                    HoTen = "Nguyễn Văn Long",
                    SoDienThoai = "0900000013",
                    Email = "nguyenvanlong@gmail.com",
                    DiaChi = "45, đường Trần Hưng Đạo, phường Phú Cường, thành phố Thủ Dầu Một, tỉnh Bình Dương"
                },

                new KhachHang
                {
                    MaKhachHang = 14,
                    HoTen = "Trần Thị Ngọc",
                    SoDienThoai = "0900000014",
                    Email = "tranthingoc@gmail.com",
                    DiaChi = "73, đường Điện Biên Phủ, phường Đa Kao, quận 1, Thành phố Hồ Chí Minh"
                },

                new KhachHang
                {
                    MaKhachHang = 15,
                    HoTen = "Lê Văn Phúc",
                    SoDienThoai = "0900000015",
                    Email = "levanphuc@gmail.com",
                    DiaChi = "19, đường Nguyễn Tất Thành, phường Phước Nguyên, thành phố Bà Rịa, tỉnh Bà Rịa - Vũng Tàu"
                }
            );


            // ========================================================
            // 4. SEED 3 NGƯỜI DÙNG
            // ========================================================

            modelBuilder.Entity<NguoiDung>().HasData(

                new NguoiDung
                {
                    MaNguoiDung = 1,
                    TenDangNhap = "admin",
                    MatKhau = "123456",
                    HoTen = "Quản trị viên",
                    VaiTro = "Admin",
                    DangHoatDong = true
                },

                new NguoiDung
                {
                    MaNguoiDung = 2,
                    TenDangNhap = "cashier1",
                    MatKhau = "123456",
                    HoTen = "Nguyễn Văn Thuận",
                    VaiTro = "Cashier",
                    DangHoatDong = true
                },

                new NguoiDung
                {
                    MaNguoiDung = 3,
                    TenDangNhap = "cashier2",
                    MatKhau = "123456",
                    HoTen = "Trần Văn Nam",
                    VaiTro = "Cashier",
                    DangHoatDong = true
                }
            );


            // ========================================================
            // 5. SEED 10 ĐƠN HÀNG
            // ========================================================

            modelBuilder.Entity<DonHang>().HasData(

                new DonHang
                {
                    MaDonHang = 1,
                    MaKhachHang = 1,
                    MaNguoiDung = 1,
                    NgayDatHang = new DateTime(2026, 10, 1, 9, 0, 0),
                    TongTien = 205000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 2,
                    MaKhachHang = 2,
                    MaNguoiDung = 2,
                    NgayDatHang = new DateTime(2026, 10, 1, 10, 0, 0),
                    TongTien = 115000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 3,
                    MaKhachHang = 3,
                    MaNguoiDung = 2,
                    NgayDatHang = new DateTime(2026, 10, 2, 11, 0, 0),
                    TongTien = 165000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 4,
                    MaKhachHang = 4,
                    MaNguoiDung = 3,
                    NgayDatHang = new DateTime(2026, 10, 2, 14, 0, 0),
                    TongTien = 95000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 5,
                    MaKhachHang = 5,
                    MaNguoiDung = 1,
                    NgayDatHang = new DateTime(2026, 10, 3, 9, 30, 0),
                    TongTien = 110000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 6,
                    MaKhachHang = 1,
                    MaNguoiDung = 2,
                    NgayDatHang = new DateTime(2026, 10, 3, 13, 0, 0),
                    TongTien = 88000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 7,
                    MaKhachHang = 2,
                    MaNguoiDung = 3,
                    NgayDatHang = new DateTime(2026, 10, 4, 10, 30, 0),
                    TongTien = 170000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 8,
                    MaKhachHang = 3,
                    MaNguoiDung = 1,
                    NgayDatHang = new DateTime(2026, 10, 5, 15, 0, 0),
                    TongTien = 165000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 9,
                    MaKhachHang = 4,
                    MaNguoiDung = 2,
                    NgayDatHang = new DateTime(2026, 10, 6, 16, 0, 0),
                    TongTien = 185000,
                    TrangThai = "Completed"
                },

                new DonHang
                {
                    MaDonHang = 10,
                    MaKhachHang = 5,
                    MaNguoiDung = 3,
                    NgayDatHang = new DateTime(2026, 10, 7, 17, 0, 0),
                    TongTien = 195000,
                    TrangThai = "Completed"
                }
            );


            // ========================================================
            // 6. SEED 20 CHI TIẾT ĐƠN HÀNG
            // Mỗi đơn hàng có 2 sản phẩm
            // ========================================================

            modelBuilder.Entity<ChiTietDonHang>().HasData(

                // Đơn hàng 1
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 1,
                    MaDonHang = 1,
                    MaSanPham = 1,
                    SoLuong = 1,
                    DonGia = 85000,
                    ThanhTien = 85000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 2,
                    MaDonHang = 1,
                    MaSanPham = 2,
                    SoLuong = 1,
                    DonGia = 120000,
                    ThanhTien = 120000
                },

                // Đơn hàng 2
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 3,
                    MaDonHang = 2,
                    MaSanPham = 3,
                    SoLuong = 2,
                    DonGia = 35000,
                    ThanhTien = 70000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 4,
                    MaDonHang = 2,
                    MaSanPham = 4,
                    SoLuong = 1,
                    DonGia = 45000,
                    ThanhTien = 45000
                },

                // Đơn hàng 3
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 5,
                    MaDonHang = 3,
                    MaSanPham = 5,
                    SoLuong = 1,
                    DonGia = 55000,
                    ThanhTien = 55000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 6,
                    MaDonHang = 3,
                    MaSanPham = 6,
                    SoLuong = 1,
                    DonGia = 110000,
                    ThanhTien = 110000
                },

                // Đơn hàng 4
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 7,
                    MaDonHang = 4,
                    MaSanPham = 7,
                    SoLuong = 1,
                    DonGia = 45000,
                    ThanhTien = 45000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 8,
                    MaDonHang = 4,
                    MaSanPham = 8,
                    SoLuong = 2,
                    DonGia = 25000,
                    ThanhTien = 50000
                },

                // Đơn hàng 5
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 9,
                    MaDonHang = 5,
                    MaSanPham = 9,
                    SoLuong = 1,
                    DonGia = 35000,
                    ThanhTien = 35000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 10,
                    MaDonHang = 5,
                    MaSanPham = 10,
                    SoLuong = 1,
                    DonGia = 75000,
                    ThanhTien = 75000
                },

                // Đơn hàng 6
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 11,
                    MaDonHang = 6,
                    MaSanPham = 11,
                    SoLuong = 2,
                    DonGia = 30000,
                    ThanhTien = 60000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 12,
                    MaDonHang = 6,
                    MaSanPham = 12,
                    SoLuong = 1,
                    DonGia = 28000,
                    ThanhTien = 28000
                },

                // Đơn hàng 7
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 13,
                    MaDonHang = 7,
                    MaSanPham = 13,
                    SoLuong = 2,
                    DonGia = 65000,
                    ThanhTien = 130000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 14,
                    MaDonHang = 7,
                    MaSanPham = 14,
                    SoLuong = 1,
                    DonGia = 40000,
                    ThanhTien = 40000
                },

                // Đơn hàng 8
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 15,
                    MaDonHang = 8,
                    MaSanPham = 15,
                    SoLuong = 1,
                    DonGia = 90000,
                    ThanhTien = 90000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 16,
                    MaDonHang = 8,
                    MaSanPham = 16,
                    SoLuong = 1,
                    DonGia = 75000,
                    ThanhTien = 75000
                },

                // Đơn hàng 9
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 17,
                    MaDonHang = 9,
                    MaSanPham = 19,
                    SoLuong = 2,
                    DonGia = 45000,
                    ThanhTien = 90000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 18,
                    MaDonHang = 9,
                    MaSanPham = 21,
                    SoLuong = 1,
                    DonGia = 95000,
                    ThanhTien = 95000
                },

                // Đơn hàng 10
                new ChiTietDonHang
                {
                    MaChiTietDonHang = 19,
                    MaDonHang = 10,
                    MaSanPham = 29,
                    SoLuong = 1,
                    DonGia = 15000,
                    ThanhTien = 15000
                },

                new ChiTietDonHang
                {
                    MaChiTietDonHang = 20,
                    MaDonHang = 10,
                    MaSanPham = 30,
                    SoLuong = 1,
                    DonGia = 180000,
                    ThanhTien = 180000
                }
            );
        }
    }
}