# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI — BUỔI 6

## 📚 Thông tin

- **Môn học:** Lập trình Ứng dụng .NET Core — 229162
- **Buổi 6:** Entity Framework Core nâng cao, phân trang LINQ và tích hợp WinForms Client – Server.
- **Công nghệ:** C#, .NET 8, ASP.NET Core Web API, Entity Framework Core, WinForms, SQL Server.

## ✅ Nội dung đã thực hiện

- Xây dựng API phân trang sản phẩm bằng LINQ `OrderBy()`, `Skip()` và `Take()`.
- Sử dụng `Include()` lấy tên nhóm hàng và `AsNoTracking()` cho truy vấn chỉ đọc.
- Tìm kiếm sản phẩm theo tên hoặc barcode, kết hợp lọc theo nhóm hàng.
- Trả kết quả phân trang gồm `currentPage`, `pageSize`, `totalItems`, `totalPages` và `items`.
- Xây dựng `ApiClientService` dùng chung HttpClient và tự đính kèm JWT.
- Hoàn thiện WinForms quản lý sản phẩm: chuyển trang, tìm kiếm, lọc nhóm và thêm/sửa/xóa qua API.
- **Bài 6.1:** Phân trang khách hàng theo điểm thưởng giảm dần.
- **Bài 6.2:** Lọc sản phẩm bằng ComboBox nhóm hàng trên WinForms.

## 🌐 API Buổi 6

| API | Chức năng |
|---|---|
| `GET /api/products/paged` | Phân trang, tìm kiếm và lọc sản phẩm |
| `GET /api/customers/paged` | Phân trang khách hàng theo điểm thưởng |

Ví dụ:

```text
/api/products/paged?pageNumber=1&pageSize=5
/api/products/paged?keyword=Gạo&categoryId=1&pageNumber=1&pageSize=5
/api/customers/paged?pageNumber=1&pageSize=5
```

## 🧪 Kết quả kiểm thử

- Build thành công, **0 warning, 0 error**.
- **23 kiểm tra API đạt** về phân trang, tìm kiếm, lọc nhóm, kiểm tra đầu vào và phân quyền.
- Đã kiểm tra trực tiếp trên WinForms: chuyển trang, tìm kiếm, lọc nhóm và đầy đủ thêm/sửa/xóa.
- Đối chiếu SQL xác nhận dữ liệu được lưu đúng; sản phẩm thử đã được xóa sau kiểm tra.

## ▶️ Chạy dự án

Sử dụng duy nhất database **`SonNguyenMiniSupermarketDb`** trên server **`.\SQLEXPRESS`**.

Chạy API:

```powershell
dotnet run --project MiniSupermarket.API --launch-profile https
```

Chạy WinForms trong terminal khác:

```powershell
dotnet run --project MiniSupermarket.WinForms
```

- **Swagger:** https://localhost:7163/swagger/index.html
- Đăng nhập bằng tài khoản **Admin hoặc Warehouse** để quản lý sản phẩm.

## 👨‍💻 Sinh viên

- **Họ tên:** Nguyễn Vũ Trường Sơn
- **MSSV:** 2124110109
- **Lớp học phần:** CCQ2411D
- **Đề tài:** Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core - Siêu thị Mini Tổng hợp Đời Sống Xanh
