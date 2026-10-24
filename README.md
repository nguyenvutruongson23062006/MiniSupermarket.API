# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI — BUỔI 5

## 📚 Thông tin

- **Môn học:** Lập trình Ứng dụng .NET Core — 229162
- **Buổi 5:** ADO.NET nâng cao, Stored Procedure và giao dịch POS Checkout
- **Công nghệ:** C#, .NET 8, ASP.NET Core Web API, WinForms, SQL Server

## ✅ Nội dung đã thực hiện

- Thực hành Console với `ExecuteScalar()`, `ExecuteNonQuery()` và `SqlDataReader()`.
- Tra cứu sản phẩm theo barcode bằng ADO.NET và tham số SQL.
- Sử dụng `DataTable`, `SqlDataAdapter` để tải dữ liệu và đếm sản phẩm theo nhóm.
- Lọc sản phẩm có tồn kho thấp.
- Xây dựng Stored Procedure báo cáo doanh thu theo thu ngân, khoảng ngày và top 5 sản phẩm bán chạy.
- Thanh toán POS bằng `SqlTransaction`: tạo hóa đơn, lưu chi tiết, trừ kho và cộng điểm.
- Rollback toàn bộ giao dịch khi sản phẩm không đủ tồn kho.
- Hủy hóa đơn, hoàn kho và thu hồi điểm thưởng.
- Kết nối WinForms POS với các API ADO.NET của Buổi 5.

## 🧪 Kết quả kiểm thử

- Build toàn bộ solution thành công, **0 warning, 0 error**.
- Các bài Console và kịch bản thanh toán, rollback, hủy đơn hoạt động đúng.
- Đã thanh toán trực tiếp trên WinForms và đối chiếu hóa đơn, tồn kho, điểm thưởng trong SQL Server.
- Barcode đạt dưới **100 ms** trong phiên kiểm tra trên máy thực hành.
- Bộ dữ liệu thực hành có **20 sản phẩm, 5 thu ngân, 15 hóa đơn**, tổng doanh thu **4.822.000 đồng**.

> Chi tiết hóa đơn mẫu được dựng lại để khớp các tổng số trong tài liệu.

## ▶️ Chạy Buổi 5

```powershell
dotnet run --project MiniSupermarket.API --launch-profile Buoi5
dotnet run --project MiniSupermarket.WinForms
```

- **Swagger:** https://localhost:7163/swagger/index.html
- **Database thực hành:** `SonNguyenMiniSupermarketDb_Buoi5`
- **Tài khoản mẫu:** `admin01` hoặc `cashier01` — mật khẩu `123456`.
- **Console:** chạy project `ThucHanhAdoNet_Tiet1` hoặc `ThucHanhTransaction_Tiet4`.

## 👨‍💻 Sinh viên

- **Họ tên:** Nguyễn Vũ Trường Sơn
- **MSSV:** 2124110109
- **Lớp học phần:** CCQ2411D
- **Đề tài:** Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core - Siêu thị Mini Tổng hợp Đời Sống Xanh
