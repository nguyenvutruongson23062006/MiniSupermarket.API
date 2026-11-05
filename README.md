# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI — BUỔI 5

## 📚 Thông tin

- **Môn học:** Lập trình Ứng dụng .NET Core
- **Mã môn:** 229162
- **Buổi thực hành:** Buổi 5 — ADO.NET nâng cao, Stored Procedure và giao dịch POS Checkout
- **Ngôn ngữ:** C#
- **Framework:** .NET 8
- **Backend:** ASP.NET Core Web API
- **Frontend:** Windows Forms
- **Database:** Microsoft SQL Server
- **Truy cập dữ liệu:** Entity Framework Core và ADO.NET

## 🏪 Giới thiệu

### Đề tài

Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — **Siêu thị Mini Tổng hợp Đời Sống Xanh**.

### Phạm vi Buổi 5

Buổi 5 bổ sung khả năng truy cập SQL Server trực tiếp bằng ADO.NET, xây dựng báo cáo bằng Stored Procedure và xử lý thanh toán POS bằng SqlTransaction.

Nội dung tập trung vào:

- Các thành phần cốt lõi của ADO.NET.
- Tra cứu sản phẩm theo mã vạch.
- Mô hình Connected và Disconnected.
- Báo cáo doanh thu bằng Stored Procedure.
- Thanh toán đồng bộ nhiều bảng bằng Transaction.
- Rollback khi có lỗi.
- Hủy hóa đơn và hoàn trả dữ liệu liên quan.

## 🏗️ Kiến trúc hệ thống

```text
WinForms Client
      │
      │ HTTP Request + JWT Token
      ▼
ASP.NET Core Web API
      │
      │ ADO.NET / EF Core
      ▼
SQL Server
SonNguyenMiniSupermarketDb
```

- WinForms hiển thị giao diện và gửi yêu cầu.
- Web API kiểm tra dữ liệu, xác thực, phân quyền và xử lý nghiệp vụ.
- SQL Server lưu dữ liệu.
- Các chức năng ADO.NET của Buổi 5 được xử lý ở Backend.

## ✅ Nội dung đã thực hiện

### Tiết 1 — Thành phần ADO.NET

- Kết nối SQL Server bằng `SqlConnection`.
- Thực thi lệnh bằng `SqlCommand`.
- Truyền tham số bằng `SqlParameter`.
- Đọc kết quả bằng `SqlDataReader`.
- Làm việc với `DataTable` và `SqlDataAdapter`.
- Giải phóng tài nguyên bằng `using` và `await using`.

| Phương thức | Mục đích |
|---|---|
| `ExecuteScalar()` | Lấy một giá trị, như số lượng sản phẩm hoặc mã vừa tạo |
| `ExecuteNonQuery()` | Thực hiện INSERT, UPDATE, DELETE |
| `ExecuteReader()` | Đọc kết quả nhiều dòng, nhiều cột |

API sử dụng các phiên bản bất đồng bộ tương ứng khi phù hợp.

### Tiết 2 — Tra cứu Barcode và DataTable

- Tìm sản phẩm theo mã vạch bằng ADO.NET.
- Trả mã sản phẩm, tên, giá bán và tồn kho.
- Dùng tham số `@Barcode`, không ghép trực tiếp đầu vào vào câu SQL.
- Trả thông báo khi mã không tồn tại.
- Minh họa đọc trực tiếp bằng Reader.
- Minh họa nạp dữ liệu vào DataTable.
- Đếm sản phẩm theo nhóm.
- Lọc sản phẩm theo ngưỡng tồn kho.

### Tiết 3 — Stored Procedure báo cáo

- Báo cáo doanh thu theo thu ngân.
- Báo cáo doanh thu theo khoảng ngày.
- Top 5 sản phẩm bán chạy.
- Gọi thủ tục từ C# bằng `CommandType.StoredProcedure`.

### Tiết 4 — ACID và Transaction

- Thực hành `BeginTransaction()`, `Commit()` và `Rollback()`.
- Sử dụng cùng Connection và Transaction cho chuỗi thao tác.
- Minh họa hoàn tác khi có lỗi.

| Đặc tính | Ý nghĩa |
|---|---|
| Atomicity | Các thao tác cùng thành công hoặc cùng hoàn tác |
| Consistency | Dữ liệu tuân thủ ràng buộc và quy tắc nghiệp vụ |
| Isolation | Kiểm soát các giao dịch đồng thời |
| Durability | Dữ liệu được xác nhận lưu sau Commit |

### Tiết 5 — POS Checkout

Một lần thanh toán thực hiện:

1. Kiểm tra tài khoản và giỏ hàng.
2. Kiểm tra khách hàng nếu có.
3. Tạo hóa đơn.
4. Kiểm tra sản phẩm, giá và tồn kho từ SQL.
5. Lưu chi tiết hóa đơn.
6. Trừ tồn kho.
7. Cập nhật tổng tiền và tổng số lượng.
8. Cộng điểm cho khách hàng.
9. Commit khi toàn bộ thao tác thành công.

Nếu có lỗi trong giao dịch, hệ thống Rollback những thay đổi đã thực hiện.

### Bài tập mở rộng — Hủy hóa đơn

- Kiểm tra hóa đơn và quyền hủy.
- Đọc chi tiết sản phẩm.
- Hoàn lại tồn kho.
- Điều chỉnh điểm thưởng.
- Xóa chi tiết hóa đơn và hóa đơn.
- Commit hoặc Rollback toàn bộ giao dịch hủy.

## 🗂️ Cấu trúc mã nguồn

```text
MiniSupermarket.API/
├── Controllers/
│   ├── AdoProductController.cs
│   ├── AdoReportsController.cs
│   └── CheckoutController.cs
├── Services/
│   └── AdoCheckoutService.cs
├── Models/
│   └── CheckoutDtos.cs
├── Migrations/
├── appsettings.json
└── Program.cs

MiniSupermarket.WinForms/
├── Services/
│   └── ApiClientService.cs
├── FormLogin.cs
└── FormPOS.cs

ThucHanhAdoNet_Tiet1/
└── Program.cs

ThucHanhTransaction_Tiet4/
└── Program.cs
```

| Thành phần | Vai trò |
|---|---|
| `AdoProductController` | Tra cứu barcode và thực hành đọc dữ liệu |
| `AdoReportsController` | Gọi Stored Procedure |
| `CheckoutController` | Nhận yêu cầu thanh toán và hủy hóa đơn |
| `AdoCheckoutService` | Xử lý nghiệp vụ trong Transaction |
| `CheckoutDtos` | Dữ liệu giỏ hàng và kết quả thanh toán |
| `FormPOS` | Giao diện quầy bán hàng |
| `ApiClientService` | HttpClient, địa chỉ API và Token |

Solution ứng dụng chính gồm API và WinForms. Hai chương trình Console phục vụ thực hành riêng theo tài liệu.

## 🗄️ Cấu hình cơ sở dữ liệu

### Database

Sử dụng database:

```text
SonNguyenMiniSupermarketDb
```

Server trên máy thực hành:

```text
.\SQLEXPRESS
```

### Chuỗi kết nối

Mở `MiniSupermarket.API/appsettings.json`, cấu hình:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SonNguyenMiniSupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Giữ lại các cấu hình khác trong file.

Nếu tên SQL Server trên máy khác, sửa `Server` tương ứng.

### Migration

Trong Visual Studio:

1. Chọn API làm Startup Project.
2. Mở **Tools → NuGet Package Manager → Package Manager Console**.
3. Chọn Default project là `MiniSupermarket.API`.
4. Chạy:

```powershell
Update-Database
```

Migration liên quan Buổi 5:

```text
20261022103235_CompleteBuoi5AdoCheckout
```

Cần đồng bộ database với phiên bản mã nguồn đang sử dụng.

### Stored Procedure

Kiểm tra tại:

```text
SonNguyenMiniSupermarketDb
└── Programmability
    └── Stored Procedures
```

Các thủ tục:

```text
dbo.sp_GetRevenueByCashier
dbo.sp_GetRevenueByDateRange
dbo.sp_GetTop5SellingProducts
```

Dữ liệu bổ sung trực tiếp trong SQL trên máy thực hành không tự được chuyển theo Git.

## ⚙️ Yêu cầu môi trường

- Windows.
- .NET 8 SDK.
- SQL Server hoặc SQL Server Express.
- SQL Server Management Studio.
- Visual Studio có workload:
  - ASP.NET and web development.
  - .NET desktop development.

Kiểm tra SDK:

```powershell
dotnet --list-sdks
```

## ▶️ Chạy Backend — Web API

### Bằng Visual Studio

1. Mở `MiniSupermarket.API/MiniSupermarket.API.sln`.
2. Chọn `MiniSupermarket.API` làm Startup Project.
3. Chọn profile `https`.
4. Nhấn F5 hoặc Ctrl + F5.

### Bằng terminal

Chạy tại thư mục gốc repository:

```powershell
dotnet restore MiniSupermarket.API/MiniSupermarket.API.csproj
dotnet run --project MiniSupermarket.API --launch-profile https
```

Swagger:

```text
https://localhost:7163/swagger/index.html
```

Nếu cần tin cậy chứng chỉ HTTPS phát triển:

```powershell
dotnet dev-certs https --trust
```

Giữ Backend chạy khi sử dụng WinForms.

## 🖥️ Chạy Frontend — WinForms

### Địa chỉ API

Trong `MiniSupermarket.WinForms/Services/ApiClientService.cs`:

```csharp
BaseAddress = new Uri("https://localhost:7163/api/");
```

Địa chỉ phải khớp Backend đang chạy.

### Bằng Visual Studio

Sau khi API chạy, nhấn chuột phải project WinForms:

**Debug → Start New Instance**

### Bằng terminal

Mở terminal thứ hai tại thư mục gốc repository:

```powershell
dotnet restore MiniSupermarket.WinForms/MiniSupermarket.WinForms.csproj
dotnet run --project MiniSupermarket.WinForms
```

### Tài khoản mẫu

| Tài khoản | Mật khẩu |
|---|---|
| `admin01` | `123456` |
| `cashier01` | `123456` |

Tài khoản phải tồn tại và đang hoạt động trong database.

Dùng Admin để thử báo cáo; dùng Admin hoặc Cashier để thử thanh toán.

## 🧩 Chạy bài Console

### Tiết 1

```powershell
dotnet run --project ThucHanhAdoNet_Tiet1
```

### Tiết 4

```powershell
dotnet run --project ThucHanhTransaction_Tiet4
```

Bài cập nhật tồn kho có thể thay đổi dữ liệu thật. Nên dùng sản phẩm thử và ghi lại dữ liệu trước khi chạy.

## 🌐 API Buổi 5

| Phương thức | Endpoint | Chức năng |
|---|---|---|
| GET | `/api/ado-products/barcode/{barcode}` | Tra cứu mã vạch |
| GET | `/api/ado-products/filter-stock?minStock=10` | Lọc tồn kho theo ngưỡng |
| GET | `/api/ado-products/disconnected-cache` | Minh họa DataTable |
| GET | `/api/ado-products/count-by-category` | Đếm sản phẩm theo nhóm |
| GET | `/api/reports/revenue-by-cashier` | Doanh thu theo thu ngân |
| GET | `/api/reports/revenue-by-date-range` | Doanh thu theo khoảng ngày |
| GET | `/api/reports/top-selling-products` | Top 5 bằng Stored Procedure |
| POST | `/api/checkout` | Thanh toán |
| DELETE | `/api/checkout/cancel/{orderId}` | Hủy hóa đơn |

Endpoint `filter-stock` hiện lọc tồn kho nhỏ hơn hoặc bằng ngưỡng truyền vào.

## 🔐 Đăng nhập trên Swagger

1. Gọi `POST /api/auth/login`.
2. Nhập tài khoản:

```json
{
  "username": "admin01",
  "password": "123456"
}
```

3. Sao chép Token.
4. Bấm Authorize.
5. Gắn Token theo hướng dẫn hộp thoại.
6. Gọi API được bảo vệ.

| Mã | Ý nghĩa |
|---|---|
| 200 | Thành công |
| 400 | Sai dữ liệu hoặc điều kiện nghiệp vụ |
| 401 | Chưa xác thực |
| 403 | Không đủ quyền |
| 404 | Không tìm thấy |

## 🧾 Quy tắc thanh toán

- Giá tính tiền lấy từ SQL.
- Thu ngân được xác định từ tài khoản đăng nhập.
- Kiểm tra lại tồn kho tại thời điểm thanh toán.
- Khách hàng được cộng 1 điểm cho mỗi 10.000đ, lấy phần nguyên.
- Khách vãng lai không được cộng điểm vào tài khoản khách hàng.

Ví dụ thanh toán cơ bản:

```json
{
  "customerId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2
    }
  ]
}
```

Cần chọn mã khách hàng, sản phẩm và số lượng phù hợp dữ liệu thực tế.

## 🧪 Kịch bản kiểm thử

### Tra cứu Barcode

- Mã tồn tại → 200 và đúng sản phẩm.
- Mã không tồn tại → 404.
- Đầu vào đặc biệt không được biến thành lệnh SQL.

### Báo cáo

- Admin gọi được báo cáo.
- Kết quả đối chiếu được với dữ liệu hóa đơn.
- Chọn khoảng ngày có dữ liệu khi thử báo cáo theo ngày.

### Thanh toán thành công

- Có hóa đơn và chi tiết.
- Tồn kho giảm đúng.
- Tổng tiền đúng.
- Điểm tăng đúng nếu có khách hàng.

### Rollback

- Gửi một giỏ có sản phẩm vượt tồn kho.
- Mong đợi lỗi nghiệp vụ.
- Không giữ lại hóa đơn dở dang.
- Tồn kho và điểm không bị thay đổi bởi lần thanh toán thất bại.

Identity có thể nhảy số sau Rollback; cần kiểm tra bản ghi thực tế thay vì chỉ nhìn số thứ tự.

### Hủy hóa đơn

- Dùng hóa đơn thử vừa tạo.
- Hủy thành công thì hoàn kho và điều chỉnh điểm.
- Chi tiết và hóa đơn bị xóa.
- Gọi lại mã đã hủy thì không tìm thấy.

### WinForms POS

- Quét mã ra đúng sản phẩm.
- Quét lặp thì tăng số lượng.
- Tổng tiền thay đổi theo số lượng.
- Nhập tiền khách đưa thì tính tiền thừa.
- Thiếu tiền thì chặn thanh toán và báo rõ lý do.

## 🛠️ Lỗi thường gặp

| Lỗi | Cách kiểm tra |
|---|---|
| Trùng cổng API | Không chạy nhiều API trên cùng cổng |
| WinForms không kết nối | Kiểm tra Backend và BaseAddress |
| Không kết nối SQL | Kiểm tra server, quyền và chuỗi kết nối |
| Thiếu bảng hoặc cột | Đồng bộ Migration |
| Thiếu Stored Procedure | Kiểm tra thủ tục và Migration |
| 401 | Đăng nhập và gắn Token lại |
| 403 | Dùng tài khoản đúng quyền |
| File build bị khóa | Stop ứng dụng trước khi build |

## 📋 Đối chiếu kết quả

- Kiểm tra code để xác nhận đúng kỹ thuật ADO.NET.
- Kiểm tra Swagger để xác nhận API hoạt động.
- Kiểm tra WinForms để xác nhận luồng giao diện.
- Đối chiếu SQL trước và sau để kiểm chứng Commit, Rollback và hủy đơn.
- Thời gian barcode dưới 100 ms cần được đo trong phiên kiểm thử thực tế.
- Số liệu báo cáo phụ thuộc database đang sử dụng.

## 👨‍💻 Sinh viên

- **Họ tên:** Nguyễn Vũ Trường Sơn
- **MSSV:** 2124110109
- **Lớp học phần:** CCQ2411D
- **Đề tài:** Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — Siêu thị Mini Tổng hợp Đời Sống Xanh
