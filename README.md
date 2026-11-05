# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI — BUỔI 6

## 📚 Thông tin

- **Môn học:** Lập trình Ứng dụng .NET Core
- **Mã môn:** 229162
- **Buổi thực hành:** Buổi 6 — Entity Framework Core nâng cao, phân trang LINQ và tích hợp WinForms Client – Server
- **Ngôn ngữ:** C#
- **Framework:** .NET 8
- **Backend:** ASP.NET Core Web API
- **Frontend:** Windows Forms
- **Database:** Microsoft SQL Server
- **Truy cập dữ liệu:** Entity Framework Core và ADO.NET
- **Nhánh:** Buoi_6 — bản hoàn thiện hệ thống

## 🏪 Giới thiệu

### Đề tài

Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — **Siêu thị Mini Tổng hợp Đời Sống Xanh**.

### Phạm vi nhánh Buổi 6

Nhánh Buổi 6 tích hợp các chức năng từ các buổi trước và bổ sung:

- Truy vấn EF Core nâng cao.
- Phân trang tại máy chủ.
- Tìm kiếm và lọc bằng LINQ.
- Tích hợp đầy đủ API với WinForms.
- Phân trang khách hàng theo điểm thưởng.
- Dashboard và thống kê doanh thu.
- Cảnh báo tồn kho thấp.
- Tích điểm và đổi điểm giảm tiền.
- Xuất danh mục sản phẩm và hóa đơn ra CSV.

## 🏗️ Kiến trúc hệ thống

```text
Windows Forms
      │
      │ HTTP Request + JWT Bearer
      ▼
ASP.NET Core Web API
      │
      ├── EF Core: quản lý, tìm kiếm, phân trang
      │
      └── ADO.NET: barcode, Stored Procedure, Transaction
      │
      ▼
SQL Server
SonNguyenMiniSupermarketDb
```

Solution ứng dụng chính gồm hai project:

| Project | Vai trò |
|---|---|
| `MiniSupermarket.API` | Backend, xác thực và xử lý nghiệp vụ |
| `MiniSupermarket.WinForms` | Frontend và thao tác người dùng |

## ✅ Chức năng hệ thống

### Đăng nhập và phân quyền

- Đăng nhập bằng tài khoản và mật khẩu.
- Cấp JWT Token.
- WinForms lưu Token và gửi kèm yêu cầu.
- API bảo vệ endpoint bằng Authorize.
- Menu hiển thị theo vai trò.
- Các nút menu được xếp liền nhau khi những nút khác bị ẩn.

### Quản lý nhóm hàng

- Xem danh sách.
- Thêm, sửa, xóa theo quyền và điều kiện nghiệp vụ.
- Cung cấp nhóm hàng cho ComboBox sản phẩm.

### Quản lý sản phẩm

- Xem danh sách phân trang.
- Tìm theo tên hoặc mã vạch.
- Lọc theo nhóm hàng.
- Kết hợp tìm kiếm, lọc và chuyển trang.
- Thêm, sửa, xóa qua API.
- Hiển thị tên nhóm.
- Tô màu cảnh báo sản phẩm tồn kho từ 10 trở xuống.

### Quản lý khách hàng

- Xem và tìm kiếm khách hàng.
- Thêm và sửa thông tin theo quyền.
- Xóa dành cho Admin.
- Phân trang theo điểm tích lũy giảm dần.
- Hiển thị điểm tích lũy.
- Mã khách hàng được dùng làm thứ tự phụ khi bằng điểm.

### Quản lý tài khoản

- Quản lý tài khoản người dùng.
- Quản lý vai trò và trạng thái hoạt động.
- Chức năng quản trị dành cho Admin.

### Bán hàng POS

- Tra cứu sản phẩm theo barcode.
- Quét lặp để tăng số lượng.
- Điều chỉnh giỏ hàng.
- Tính tổng tiền và tiền thừa.
- Chặn thiếu tiền.
- Chọn khách hàng để tích điểm.
- Đổi điểm để giảm tiền.
- Thanh toán qua API trong Transaction.

### Hóa đơn và hủy giao dịch

- Lưu hóa đơn và chi tiết.
- Trừ tồn kho.
- Cập nhật điểm.
- Hủy hóa đơn qua API theo quyền.
- Hoàn kho và điều chỉnh điểm trong Transaction.

### Dashboard và báo cáo

- Doanh thu hôm nay.
- Tổng số hóa đơn hoàn thành.
- Khách hàng mới hôm nay.
- Số mặt hàng tồn kho thấp.
- Biểu đồ cột doanh thu theo ngày trong khoảng chọn.
- Top 5 sản phẩm bán chạy.
- Báo cáo doanh thu theo thu ngân.
- Báo cáo theo khoảng ngày.

### Xuất dữ liệu

- Xuất toàn bộ danh mục sản phẩm ra CSV.
- Xuất hóa đơn theo khoảng ngày ra CSV.
- CSV có thể mở bằng Excel.
- Quyền báo cáo và xuất dữ liệu dành cho Admin.

## 🔐 Phân quyền hiện tại

| Chức năng | Admin | Cashier | Warehouse |
|---|---|---|---|
| Bán hàng POS | Có | Có | Không |
| Quản lý nhóm hàng | Có | Không | Có |
| Quản lý sản phẩm | Có | Không | Có |
| Xem, thêm, sửa khách hàng | Có | Có | Không |
| Xóa khách hàng | Có | Không | Không |
| Báo cáo và Dashboard | Có | Không | Không |
| Xuất CSV báo cáo | Có | Không | Không |
| Quản lý tài khoản | Có | Không | Không |

Cashier không được truy cập Báo cáo doanh thu.

Phân quyền được kiểm tra cả trên giao diện và API. Việc ẩn nút không thay thế kiểm tra quyền ở Backend.

## 🗂️ Cấu trúc mã nguồn

```text
MiniSupermarket.API/
├── Controllers/
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── CustomersController.cs
│   ├── UsersController.cs
│   ├── OrdersController.cs
│   ├── AdoProductController.cs
│   ├── AdoReportsController.cs
│   ├── CheckoutController.cs
│   ├── ReportsController.cs
│   └── ReportsController.Dashboard.cs
├── Data/
│   └── SupermarketDbContext.cs
├── Models/
├── Services/
│   └── AdoCheckoutService.cs
├── Migrations/
├── appsettings.json
└── Program.cs

MiniSupermarket.WinForms/
├── Services/
│   └── ApiClientService.cs
├── FormLogin.cs
├── FormMainShell.cs
├── FormProductManagement.cs
├── FormCategoryManagement.cs
├── FormCustomerManagement.cs
├── FormUserManagement.cs
├── FormPOS.cs
└── FormQuickReport.cs
```

`ReportsController.cs` và `ReportsController.Dashboard.cs` là hai phần của cùng một partial class.

Các Controller báo cáo có thể dùng chung tiền tố `api/reports`; endpoint được xác định bằng cả phương thức HTTP và đường dẫn đầy đủ.

## ⚙️ Yêu cầu môi trường

- Windows.
- .NET 8 SDK.
- SQL Server hoặc SQL Server Express.
- SQL Server Management Studio.
- Visual Studio với workload:
  - ASP.NET and web development.
  - .NET desktop development.

Kiểm tra SDK:

```powershell
dotnet --list-sdks
```

## 🗄️ Cấu hình database

### Database sử dụng

```text
Server:   .\SQLEXPRESS
Database: SonNguyenMiniSupermarketDb
```

### Chuỗi kết nối

Trong `MiniSupermarket.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SonNguyenMiniSupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Giữ nguyên các cấu hình khác của file.

Thay Server nếu máy sử dụng tên instance khác.

### Áp dụng Migration

Trong Package Manager Console:

- Startup Project: API.
- Default project: `MiniSupermarket.API`.

Chạy:

```powershell
Update-Database
```

Database cần áp dụng đầy đủ Migration của phiên bản mã nguồn đang chạy.

Bảng `__EFMigrationsHistory` ghi nhận các Migration đã áp dụng, cần giữ lại để EF Core quản lý lịch sử.

### Dữ liệu thực hành

Số lượng sản phẩm, khách hàng và hóa đơn phụ thuộc database thực tế.

Những dữ liệu được nhập bổ sung trực tiếp trên máy không tự được chuyển theo Git. Không nên cố định số trang hoặc số liệu báo cáo trong mã giao diện.

## ▶️ Chạy Backend — Web API

### Visual Studio

1. Mở `MiniSupermarket.API/MiniSupermarket.API.sln`.
2. Chọn `MiniSupermarket.API` làm Startup Project.
3. Chọn profile `https`.
4. Nhấn F5 hoặc Ctrl + F5.

### Terminal

Tại thư mục gốc repository:

```powershell
dotnet restore MiniSupermarket.API/MiniSupermarket.API.csproj
dotnet run --project MiniSupermarket.API --launch-profile https
```

Địa chỉ:

```text
https://localhost:7163
http://localhost:5167
```

Swagger:

```text
https://localhost:7163/swagger/index.html
```

Nếu cần tin cậy chứng chỉ phát triển:

```powershell
dotnet dev-certs https --trust
```

Không chạy nhiều phiên API trên cùng cổng.

## 🖥️ Chạy Frontend — WinForms

### Kiểm tra BaseAddress

Trong `MiniSupermarket.WinForms/Services/ApiClientService.cs`:

```csharp
BaseAddress = new Uri("https://localhost:7163/api/");
```

Địa chỉ phải khớp Backend.

### Visual Studio

Khi API đã chạy:

- Nhấn chuột phải project WinForms.
- Chọn **Debug → Start New Instance**.

Có thể dùng Multiple Startup Projects để chạy cả hai project.

### Terminal

Mở terminal thứ hai tại thư mục gốc:

```powershell
dotnet restore MiniSupermarket.WinForms/MiniSupermarket.WinForms.csproj
dotnet run --project MiniSupermarket.WinForms
```

Giữ terminal API hoạt động trong quá trình sử dụng.

### Tài khoản mẫu

| Tài khoản | Mật khẩu | Vai trò |
|---|---|---|
| `admin01` | `123456` | Admin |
| `cashier01` | `123456` | Cashier |

Dùng tài khoản Warehouse đang có trong database để thử quyền kho.

## 🌐 API phân trang Buổi 6

| Endpoint | Chức năng |
|---|---|
| `GET /api/products/paged` | Phân trang, tìm kiếm và lọc sản phẩm |
| `GET /api/customers/paged` | Phân trang khách hàng theo điểm |

### Tham số sản phẩm

| Tham số | Ý nghĩa |
|---|---|
| `keyword` | Từ khóa tên hoặc mã vạch |
| `categoryId` | Mã nhóm hàng |
| `pageNumber` | Trang cần lấy |
| `pageSize` | Số sản phẩm mỗi trang |

Ví dụ:

```text
/api/products/paged?pageNumber=1&pageSize=5
/api/products/paged?keyword=Gạo&pageNumber=1&pageSize=5
/api/products/paged?categoryId=1&pageNumber=1&pageSize=5
/api/products/paged?keyword=Gạo&categoryId=1&pageNumber=1&pageSize=5
/api/customers/paged?pageNumber=1&pageSize=5
```

## 📦 Kết quả phân trang

Kết quả có các trường:

```json
{
  "currentPage": 1,
  "pageSize": 5,
  "totalItems": 0,
  "totalPages": 0,
  "items": []
}
```

Ví dụ trên minh họa cấu trúc khi không có kết quả.

| Trường | Ý nghĩa |
|---|---|
| `currentPage` | Trang được trả về |
| `pageSize` | Kích thước trang |
| `totalItems` | Tổng số dòng sau tìm kiếm và lọc |
| `totalPages` | Tổng số trang của kết quả |
| `items` | Dữ liệu của trang hiện tại |

Thông tin sản phẩm gồm mã, barcode, tên, giá, tồn kho, mã nhóm và tên nhóm.

## 🔍 Tìm kiếm và lọc LINQ

### Tìm kiếm

```csharp
query = query.Where(p =>
    p.TenSanPham.ToLower().Contains(search)
    || p.MaVach.Contains(search));
```

Tên hoặc mã vạch phải chứa từ khóa.

### Lọc nhóm

```csharp
query = query.Where(p => p.MaNhomHang == categoryId);
```

### Thứ tự xử lý

```text
Tìm kiếm
    ↓
Lọc nhóm
    ↓
Đếm tổng số kết quả
    ↓
Sắp xếp
    ↓
Phân trang
    ↓
Trả JSON
```

## 📄 Phân trang tại máy chủ

```csharp
long offset = (long)(pageNumber - 1) * pageSize;
```

```csharp
.OrderBy(p => p.MaSanPham)
.Skip((int)offset)
.Take(pageSize)
```

| Lệnh | Vai trò |
|---|---|
| OrderBy | Xác định thứ tự |
| Skip | Bỏ qua các dòng thuộc trang trước |
| Take | Lấy số dòng của trang hiện tại |

Ví dụ mỗi trang 5 sản phẩm:

- Trang 1: bỏ qua 0, lấy tối đa 5.
- Trang 2: bỏ qua 5, lấy tối đa 5.
- Trang 3: bỏ qua 10, lấy tối đa 5.

Quy tắc sản phẩm hiện tại:

- Trang nhỏ hơn 1 được đưa về 1.
- Kích thước trang ngoài khoảng 1–50 được đưa về 5.

## 🔗 Include và AsNoTracking

### Include

```csharp
.Include(p => p.NhomHang)
```

Khai báo dữ liệu nhóm hàng liên quan.

DTO trả về có tên nhóm để WinForms hiển thị.

### AsNoTracking

```csharp
.AsNoTracking()
```

Dùng cho truy vấn chỉ đọc để tránh chi phí theo dõi Entity khi không cần sửa.

### Select sang DTO

API chọn các trường cần trả về bằng Select.

Với phép chiếu trực tiếp sang DTO, EF Core có thể tạo phép nối từ việc truy cập tên nhóm; kết quả không chứa Entity cũng không cần được theo dõi như Entity.

## 🔐 HttpClient và JWT

`ApiClientService` cung cấp:

- HttpClient dùng chung.
- Địa chỉ API.
- Hàm tạo request.
- Bearer Token.
- Thông báo lỗi thân thiện.

```http
Authorization: Bearer <Token>
```

Luồng:

```text
Đăng nhập
    ↓
Lưu Token trong SessionManager
    ↓
Gửi request từ Form
    ↓
ApiClientService đính kèm Token
    ↓
API kiểm tra xác thực và quyền
```

## 🖱️ Luồng màn hình sản phẩm

### Tải màn hình

```csharp
await LoadCategoriesAsync();
await LoadProductsAsync(1);
```

### Tìm kiếm và lọc

- Đọc từ khóa và nhóm.
- Gọi lại trang 1.
- Hiển thị kết quả từ API.

Màn hình sản phẩm sử dụng endpoint `products/paged` với tham số keyword để tìm kiếm.

### Chuyển trang

- Trang trước: `_currentPage - 1`.
- Trang sau: `_currentPage + 1`.
- Giữ điều kiện tìm kiếm và nhóm.

### Thêm, sửa, xóa

| Thao tác | API |
|---|---|
| Thêm | `POST /api/products` |
| Sửa | `PUT /api/products/{id}` |
| Xóa | `DELETE /api/products/{id}` |

Sau thành công, Form tải lại dữ liệu.

Nếu xóa dòng cuối trang cuối, Form quay về trang cuối còn dữ liệu.

## 👥 Bài 6.1 — Phân trang khách hàng

- Gọi `GET /api/customers/paged`.
- Sắp xếp điểm tích lũy giảm dần.
- Dùng mã khách hàng làm thứ tự phụ.
- Hiển thị điểm và thông tin phân trang.
- Cashier được xem, thêm và sửa khách hàng.
- Xóa khách hàng dành cho Admin.

## 🗃️ Bài 6.2 — ComboBox lọc nhóm hàng

1. Tải nhóm hàng từ API.
2. Hiển thị tên nhóm trong ComboBox.
3. Đọc mã nhóm đang chọn.
4. Gửi categoryId tới API.
5. API lọc theo MaNhomHang.
6. Trả dữ liệu trang đầu của kết quả mới.

Có thể kết hợp nhóm với từ khóa.

## 🛒 POS, tích điểm và đổi điểm

### Bán hàng

- Barcode lấy thông tin sản phẩm.
- Quét lại tăng số lượng.
- Tổng tiền cập nhật theo giỏ hàng.
- Nhập tiền khách đưa để tính tiền thừa.
- Thiếu tiền thì chặn thanh toán.

### Tích điểm

- Khách hàng được cộng điểm theo số tiền thanh toán.
- Mỗi 10.000đ được 1 điểm, lấy phần nguyên.
- Khách vãng lai không được cộng điểm vào tài khoản.

### Đổi điểm

- Mỗi 1 điểm tương ứng giảm 1.000đ.
- Không được dùng quá số điểm hiện có.
- Tiền giảm không được vượt tiền hàng.
- Trừ điểm, cập nhật tiền thanh toán và các dữ liệu hóa đơn trong cùng Transaction.
- Hủy hóa đơn điều chỉnh lại điểm theo dữ liệu giao dịch.

## 📊 Dashboard và báo cáo

### Bốn thẻ tổng quan

- Doanh thu hôm nay.
- Tổng hóa đơn hoàn thành.
- Khách hàng mới hôm nay.
- Số sản phẩm tồn kho từ 10 trở xuống.

### Biểu đồ doanh thu

- Chọn ngày bắt đầu và kết thúc.
- Hiển thị doanh thu theo ngày.
- Dữ liệu được lấy từ API.

### Top 5 sản phẩm bán chạy

- Tổng hợp số lượng bán.
- Sắp xếp giảm dần.
- Hiển thị tối đa 5 sản phẩm.

### API báo cáo

| Endpoint | Chức năng |
|---|---|
| `/api/reports/dashboard-summary` | KPI tổng quan |
| `/api/reports/daily-revenue` | Doanh thu theo ngày |
| `/api/reports/best-sellers` | Top 5 theo khoảng ngày |
| `/api/reports/revenue-by-cashier` | Doanh thu thu ngân |
| `/api/reports/revenue-by-date-range` | Báo cáo Stored Procedure theo ngày |
| `/api/reports/top-selling-products` | Top 5 bằng Stored Procedure |

Các endpoint trên dùng phương thức GET và yêu cầu Admin.

## 📤 Xuất CSV

- Xuất danh mục sản phẩm.
- Xuất hóa đơn theo khoảng ngày.
- File CSV có thể mở bằng Excel.

API:

```text
GET /api/reports/export-products
GET /api/reports/export-orders
```

Cashier không được gọi các API xuất báo cáo này.

## 🧪 Kiểm thử hệ thống

### Swagger

1. Đăng nhập qua `POST /api/auth/login`.
2. Gắn Token bằng Authorize.
3. Thử các API phân trang.
4. Kiểm tra JSON và mã phản hồi.

### Phân trang

- Trang đầu trả đúng kích thước.
- Trang tiếp theo có dữ liệu đúng thứ tự.
- Không lặp dòng giữa các trang khi dữ liệu không thay đổi.
- Tổng số trang đúng với số kết quả.

### Tìm kiếm và lọc

- Từ khóa tên.
- Từ khóa mã vạch.
- Nhóm hàng.
- Kết hợp từ khóa và nhóm.
- Không có kết quả thì trả danh sách rỗng hợp lệ.

### CRUD WinForms

- Thêm sản phẩm thử.
- Tìm sản phẩm vừa thêm.
- Sửa và tải lại.
- Xóa sản phẩm thử.
- Đối chiếu SQL.

### Phân quyền

- Thiếu Token → 401.
- Sai quyền → 403.
- Cashier dùng được POS và khách hàng.
- Cashier không vào được báo cáo.
- Warehouse quản lý được sản phẩm và nhóm.
- Admin sử dụng các chức năng quản trị.

### POS và Transaction

- Quét mã đúng.
- Tăng số lượng.
- Tính tổng và tiền thừa.
- Chặn thiếu tiền.
- Thanh toán thành công.
- Thiếu tồn kho thì Rollback.
- Đổi điểm cập nhật đồng bộ.
- Hủy đơn hoàn kho và điều chỉnh điểm.

### Báo cáo

- KPI đối chiếu được với SQL.
- Biểu đồ theo đúng khoảng ngày.
- Top 5 sắp xếp đúng.
- CSV mở được và có dữ liệu phù hợp.

## 🛠️ Lỗi thường gặp

| Hiện tượng | Cách kiểm tra |
|---|---|
| API trùng cổng | Kiểm tra phiên API đang chạy |
| WinForms không tải dữ liệu | Kiểm tra Backend và BaseAddress |
| Lỗi HTTPS | Kiểm tra chứng chỉ phát triển |
| Không kết nối SQL | Kiểm tra chuỗi kết nối và SQL Server |
| Thiếu bảng hoặc cột | Áp dụng Migration |
| 401 | Đăng nhập lại và gắn Token |
| 403 | Kiểm tra vai trò |
| Tìm không có kết quả | Kiểm tra từ khóa và nhóm |
| Không xóa được sản phẩm | Kiểm tra dữ liệu liên quan |
| Build bị khóa file | Stop ứng dụng trước khi build |

## 📋 Ghi chú kiểm thử

Các lần kiểm tra đã bao gồm build, API, thao tác WinForms và đối chiếu SQL cho các luồng tương ứng.

Sau khi thay đổi code hoặc phân quyền cần kiểm tra lại phần bị ảnh hưởng.

Các thay đổi gần nhất về menu và quyền đã được build và kiểm tra HTTP; cần chạy lại ứng dụng để xác nhận giao diện bản mới.

Không coi số lượng dữ liệu, số trang hoặc thời gian phản hồi của một máy là kết quả cố định cho mọi môi trường.

## 👨‍💻 Sinh viên

- **Họ tên:** Nguyễn Vũ Trường Sơn
- **MSSV:** 2124110109
- **Lớp học phần:** CCQ2411D
- **Đề tài:** Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — Siêu thị Mini Tổng hợp Đời Sống Xanh
