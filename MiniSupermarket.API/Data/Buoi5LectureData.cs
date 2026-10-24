using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MiniSupermarket.API.Data;

// Chỉ dùng cho profile Buoi5. Không thay dữ liệu của database đang sử dụng.
public static class Buoi5LectureData
{
    public const string DatabaseName = "SonNguyenMiniSupermarketDb_Buoi5";

    public static async Task InitializeAsync(SupermarketDbContext db)
    {
        var connection = new SqlConnectionStringBuilder(db.Database.GetConnectionString());
        if (!string.Equals(connection.InitialCatalog, DatabaseName, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Dữ liệu bài giảng chỉ được tạo trong database {DatabaseName}.");

        if (await db.Database.CanConnectAsync())
        {
            // Chỉ nhận database đã được chính chế độ này khởi tạo.
            await using var checkConnection = new SqlConnection(connection.ConnectionString);
            await checkConnection.OpenAsync();
            using var check = new SqlCommand("""
                SELECT COUNT(*) FROM sys.extended_properties
                WHERE class=0 AND name=N'Buoi5LectureData'
                """, checkConnection);
            if (Convert.ToInt32(await check.ExecuteScalarAsync()) != 1)
                throw new InvalidOperationException("Database đã tồn tại nhưng không phải dữ liệu Buổi 5. Dừng để bảo vệ dữ liệu.");
            await db.Database.MigrateAsync();
            return;
        }

        await db.Database.MigrateAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();
        // Migrations tạo schema và dữ liệu nền. Chỉ điều chỉnh bộ mẫu trong database mới.
        await db.Database.ExecuteSqlRawAsync("""
            DELETE FROM dbo.ChiTietDonHang;
            DELETE FROM dbo.DonHang;
            DELETE FROM dbo.SanPham WHERE MaSanPham>20;

            UPDATE dbo.SanPham SET MaVach=N'893456789001',
                TenSanPham=N'Snack Khoai Tây O''Star Gói 60g', GiaBan=12500, SoLuongTon=250 WHERE MaSanPham=1;
            UPDATE dbo.SanPham SET MaVach=N'893456789002',
                TenSanPham=N'Bánh Quy Bơ Danisa Hộp Thiếc 454g', GiaBan=135000, SoLuongTon=40 WHERE MaSanPham=2;
            UPDATE dbo.SanPham SET MaVach=N'893456789003',
                TenSanPham=N'Kẹo Dẻo Chupa Chups Hương Trái Cây', GiaBan=15000, SoLuongTon=120 WHERE MaSanPham=3;
            UPDATE dbo.SanPham SET MaVach=N'893456789004',
                TenSanPham=N'Bánh Chocopie Orion Hộp 12 Cái', GiaBan=54000, SoLuongTon=60 WHERE MaSanPham=4;
            UPDATE dbo.SanPham SET MaVach=N'893456789005',
                TenSanPham=N'Nước Ngọt Coca Cola Lon 320ml', GiaBan=10000, SoLuongTon=320 WHERE MaSanPham=5;

            -- Tài liệu chỉ cho 5 sản phẩm chi tiết và các tổng doanh thu theo thu ngân,
            -- không cho toàn bộ dòng hàng của 15 hóa đơn. Các hóa đơn bên dưới được
            -- dựng lại để khớp chính xác số đơn, tổng tiền, tổng điểm đã công bố.
            UPDATE dbo.SanPham SET TenSanPham=N'Mặt hàng thực hành báo cáo',
                GiaBan=1000, SoLuongTon=1000 WHERE MaSanPham=6;
            UPDATE dbo.NguoiDung SET MatKhau=N'123456';
            UPDATE dbo.KhachHang SET DiemTichLuy=0;

            SET IDENTITY_INSERT dbo.DonHang ON;
            INSERT INTO dbo.DonHang
                (MaDonHang,MaKhachHang,MaNguoiDung,NgayDatHang,TongTien,TrangThai,MaHoaDon,TongSoLuong,DiemThuong)
            SELECT v.Id,1,u.MaNguoiDung,DATEADD(minute,v.Id,CONVERT(datetime2,'2026-09-15T09:00:00')),
                v.Total,N'Completed',CONCAT(N'HD-BUOI5-',v.Id),v.Total/1000,v.Points
            FROM (VALUES
                (1,N'cashier01',333000,33),(2,N'cashier01',333000,33),
                (3,N'cashier01',333000,33),(4,N'cashier01',323000,32),
                (5,N'cashier02',85000,8),(6,N'cashier02',85000,8),(7,N'cashier02',83000,8),
                (8,N'cashier03',715000,71),(9,N'cashier03',720000,72),
                (10,N'cashier04',160000,16),(11,N'cashier04',160000,16),(12,N'cashier04',164000,16),
                (13,N'cashier05',440000,44),(14,N'cashier05',440000,44),(15,N'cashier05',448000,44)
            ) v(Id,Username,Total,Points)
            JOIN dbo.NguoiDung u ON u.TenDangNhap=v.Username;
            SET IDENTITY_INSERT dbo.DonHang OFF;

            SET IDENTITY_INSERT dbo.ChiTietDonHang ON;
            INSERT INTO dbo.ChiTietDonHang (MaChiTietDonHang,MaDonHang,MaSanPham,SoLuong,DonGia,ThanhTien)
            SELECT MaDonHang,MaDonHang,6,TongSoLuong,1000,TongTien FROM dbo.DonHang;
            SET IDENTITY_INSERT dbo.ChiTietDonHang OFF;
            UPDATE dbo.KhachHang SET DiemTichLuy=(SELECT SUM(DiemThuong) FROM dbo.DonHang) WHERE MaKhachHang=1;
            DBCC CHECKIDENT ('dbo.DonHang', RESEED, 15) WITH NO_INFOMSGS;
            DBCC CHECKIDENT ('dbo.ChiTietDonHang', RESEED, 15) WITH NO_INFOMSGS;
            EXEC sys.sp_addextendedproperty @name=N'Buoi5LectureData', @value=N'1';
            """);
        await transaction.CommitAsync();
    }
}
