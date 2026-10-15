# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI

## 📚 Thông tin

- **Môn học:** Lập trình Ứng dụng .NET Core
- **Mã môn:** 229162
- **Buổi thực hành:** Buổi 4 - Hoàn thiện hệ thống quản lý bán hàng
- **Ngôn ngữ:** C#
- **Framework:** .NET 8
- **Backend:** ASP.NET Core Web API
- **Client:** Windows Forms
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Xác thực:** JWT Bearer Authentication
- **API Documentation:** Swagger/OpenAPI

---

## 🏪 Giới thiệu

Trong Buổi 4, dự án được phát triển từ chức năng quản lý nhóm hàng của Buổi 3 thành một hệ thống quản lý siêu thị mini hoàn chỉnh.

Hệ thống bao gồm:

- Đăng nhập và xác thực người dùng bằng JWT.
- Phân quyền theo vai trò người dùng.
- Quản lý nhóm hàng.
- Quản lý sản phẩm.
- Quản lý khách hàng.
- Quản lý tài khoản người dùng.
- Bán hàng tại quầy POS.
- Thanh toán và tạo đơn hàng.
- Quản lý tồn kho.
- Xem báo cáo doanh thu.
- Kết nối Windows Forms với ASP.NET Core Web API.
- Lưu trữ dữ liệu trên SQL Server bằng Entity Framework Core.

### Đề tài

**Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core - Siêu thị Mini Tổng hợp Đời Sống Xanh**

---

## 🏗️ Kiến trúc hệ thống

Hệ thống sử dụng mô hình Client - Server:

```text
┌─────────────────────────────────────┐
│          Windows Forms Client       │
│                                     │
│  FormLogin                          │
│  FormMainShell                      │
│  FormPOS                            │
│  FormCategoryManagement             │
│  FormProductManagement              │
│  FormCustomerManagement             │
│  FormUserManagement                 │
│  FormQuickReport                    │
└────────────────┬────────────────────┘
                 │ HTTP/HTTPS
                 ▼
┌─────────────────────────────────────┐
│        ASP.NET Core Web API         │
│                                     │
│  AuthController                     │
│  CategoriesController               │
│  ProductsController                 │
│  CustomersController                │
│  OrdersController                   │
│  UsersController                    │
│  ReportsController                  │
│                                     │
│  JWT Authentication                 │
│  Role Authorization                 │
│  Entity Framework Core              │
└────────────────┬────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────┐
│            SQL Server               │
│                                     │
│  SonNguyenMiniSupermarketDb         │
└─────────────────────────────────────┘
Windows Forms đóng vai trò là ứng dụng giao diện người dùng.
ASP.NET Core Web API xử lý nghiệp vụ, xác thực, phân quyền và giao tiếp với cơ sở dữ liệu SQL Server.
Entity Framework Core được sử dụng để truy vấn và cập nhật dữ liệu.
💻 Công nghệ sử dụng
- C#
- .NET 8
- ASP.NET Core Web API
- Windows Forms
- Entity Framework Core
- SQL Server Express
- JWT Bearer Authentication
- Swagger/OpenAPI
- HttpClient
- System.Net.Http.Json
🗄️ Cơ sở dữ liệu
Thông tin kết nối
- SQL Server: .\SQLEXPRESS
- Database: SonNguyenMiniSupermarketDb
- Kiểu xác thực: Windows Authentication
- Trust Certificate: Enabled
Chuỗi kết nối được cấu hình trong file:
MiniSupermarket.API/appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SonNguyenMiniSupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
Các bảng chính
Bảng	Chức năng
NhomHangs	Lưu thông tin nhóm hàng
SanPhams	Lưu thông tin sản phẩm và tồn kho
KhachHangs	Lưu thông tin khách hàng
DonHangs	Lưu thông tin đơn hàng
ChiTietDonHangs	Lưu chi tiết sản phẩm trong đơn hàng
NguoiDungs	Lưu thông tin tài khoản và vai trò


Quan hệ dữ liệu
NhomHang 1 ───────── N SanPham

KhachHang 1 ──────── N DonHang

NguoiDung 1 ──────── N DonHang

DonHang 1 ────────── N ChiTietDonHang

SanPham 1 ────────── N ChiTietDonHang
Dữ liệu đơn hàng và chi tiết đơn hàng được bảo vệ bằng khóa ngoại để không làm mất lịch sử bán hàng.
🔧 Entity Framework Core và Migration
Dự án sử dụng Entity Framework Core Code First.
Các package chính:
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design
Các migration chính:
20261008122638_CreateGreenLifeSupermarket
20261011025419_AddWarehouseUser
20261015065651_CompleteBuoi4Features
Các migration được sử dụng để:
- Tạo cơ sở dữ liệu.
- Tạo các bảng nghiệp vụ.
- Bổ sung tài khoản kho.
- Bổ sung sản phẩm, khách hàng, đơn hàng.
- Hoàn thiện chức năng Buổi 4.
- Cập nhật cấu trúc dữ liệu khi chương trình thay đổi.
🌱 Dữ liệu mẫu
Dự án có dữ liệu mẫu phục vụ việc kiểm thử:
- 15 nhóm hàng.
- 30 sản phẩm.
- 15 khách hàng.
- 15 tài khoản người dùng.
- 10 đơn hàng.
- 20 chi tiết đơn hàng.
Một số nhóm hàng mẫu:
- Thực phẩm hữu cơ.
- Đồ uống thiên nhiên.
- Sản phẩm chăm sóc cá nhân.
- Đồ dùng gia đình xanh.
- Sản phẩm tái chế.
- Rau củ hữu cơ.
- Trái cây hữu cơ.
- Ngũ cốc.
- Thực phẩm khô.
- Gia vị tự nhiên.
- Đồ dùng nhà bếp.
- Sản phẩm vệ sinh.
Một số mã vạch mẫu:
Mã vạch	Tên sản phẩm
893000000001	Gạo lứt hữu cơ
893000000002	Mật ong nguyên chất
893000000003	Nước ép cam nguyên chất
893000000004	Trà xanh thiên nhiên
893000000005	Xà phòng thiên nhiên
893000000006	Dầu gội thảo mộc


🔐 Tài khoản đăng nhập mẫu
Các tài khoản được tạo sẵn trong cơ sở dữ liệu:
Tài khoản	Mật khẩu	Vai trò
admin01	123456	Admin
admin02	123456	Admin
cashier01	123456	Cashier
cashier02	123456	Cashier
ware01	123456	Warehouse
ware02	123456	Warehouse


Vai trò Admin
Admin có quyền:
- Quản lý nhóm hàng.
- Quản lý sản phẩm.
- Quản lý khách hàng.
- Quản lý tài khoản.
- Xem báo cáo doanh thu.
- Bán hàng tại quầy.
- Xem và quản lý đơn hàng.
Vai trò Cashier
Cashier có quyền:
- Đăng nhập hệ thống.
- Tra cứu sản phẩm bằng mã vạch.
- Tra cứu khách hàng.
- Thực hiện bán hàng.
- Tạo đơn hàng.
- Xem thông tin cần thiết cho việc bán hàng.
Cashier không có quyền:
- Quản lý tài khoản.
- Xem báo cáo doanh thu.
- Thêm, sửa, xóa sản phẩm.
- Thay đổi dữ liệu quản trị.
Vai trò Warehouse
Warehouse có quyền:
- Xem nhóm hàng.
- Quản lý sản phẩm.
- Tìm kiếm sản phẩm.
- Cập nhật số lượng tồn kho.
- Quản lý thông tin sản phẩm.
Warehouse không có quyền:
- Thực hiện thanh toán tại quầy.
- Xem báo cáo doanh thu.
- Quản lý tài khoản người dùng.
🌐 API
API được xây dựng bằng ASP.NET Core Web API.
Địa chỉ API:
https://localhost:7163
Swagger:
https://localhost:7163/swagger/index.html
Auth API
Phương thức	Đường dẫn	Chức năng
POST	/api/auth/login	Đăng nhập và nhận JWT Token


Dữ liệu đăng nhập mẫu:
{
  "username": "admin01",
  "password": "123456"
}
Kết quả trả về bao gồm:
- success
- token
- role
- username
- fullName
📦 Categories API
Đường dẫn gốc:
/api/categories
Phương thức	Đường dẫn	Chức năng
GET	/api/categories	Lấy danh sách nhóm hàng
GET	/api/categories/{id}	Lấy nhóm hàng theo mã
GET	/api/categories/search	Tìm kiếm nhóm hàng
POST	/api/categories	Thêm nhóm hàng
PUT	/api/categories/{id}	Cập nhật nhóm hàng
DELETE	/api/categories/{id}	Xóa nhóm hàng


🛍️ Products API
Đường dẫn gốc:
/api/products
Phương thức	Đường dẫn	Chức năng
GET	/api/products	Lấy danh sách sản phẩm
GET	/api/products/{id}	Lấy sản phẩm theo mã
GET	/api/products/barcode/{barcode}	Tìm sản phẩm theo mã vạch
GET	/api/products/search	Tìm kiếm sản phẩm
POST	/api/products	Thêm sản phẩm
PUT	/api/products/{id}	Cập nhật sản phẩm
DELETE	/api/products/{id}	Xóa sản phẩm


Ví dụ tìm sản phẩm bằng mã vạch:
GET /api/products/barcode/893000000001
👥 Customers API
Đường dẫn gốc:
/api/customers
Phương thức	Đường dẫn	Chức năng
GET	/api/customers	Lấy danh sách khách hàng
GET	/api/customers/{id}	Lấy khách hàng theo mã
GET	/api/customers/search	Tìm kiếm khách hàng
POST	/api/customers	Thêm khách hàng
PUT	/api/customers/{id}	Cập nhật khách hàng
DELETE	/api/customers/{id}	Xóa khách hàng


Số điện thoại khách hàng được thiết lập là duy nhất để phục vụ việc tra cứu khi bán hàng.
🧾 Orders API
Đường dẫn gốc:
/api/orders
Phương thức	Đường dẫn	Chức năng
GET	/api/orders	Lấy danh sách đơn hàng
GET	/api/orders/{id}	Xem chi tiết đơn hàng
POST	/api/orders/checkout	Thanh toán và tạo đơn hàng


Khi thanh toán, hệ thống thực hiện:
- Kiểm tra sản phẩm.
- Kiểm tra số lượng tồn kho.
- Tính tổng tiền.
- Lưu đơn hàng.
- Lưu chi tiết đơn hàng.
- Trừ số lượng tồn kho.
- Gắn đơn hàng với nhân viên thực hiện.
- Gắn đơn hàng với khách hàng nếu có.
👤 Users API
Đường dẫn gốc:
/api/users
Chức năng này chỉ dành cho Admin.
Phương thức	Đường dẫn	Chức năng
GET	/api/users	Lấy danh sách tài khoản
POST	/api/users	Thêm tài khoản
PUT	/api/users/{id}/reset-password	Đặt lại mật khẩu
PUT	/api/users/{id}/toggle-lock	Khóa hoặc mở khóa tài khoản


📊 Reports API
Đường dẫn gốc:
/api/reports
Phương thức	Đường dẫn	Chức năng
GET	/api/reports/quick	Xem báo cáo doanh thu theo ngày


Ví dụ:
GET /api/reports/quick?date=2026-10-01
Báo cáo bao gồm:
- Tổng số đơn hàng.
- Tổng doanh thu.
- Tổng số sản phẩm đã bán.
- Sản phẩm bán chạy nhất.
- Số lượng sản phẩm bán chạy nhất.
🔒 Xác thực và phân quyền JWT
Hệ thống sử dụng JWT Bearer Authentication.
Quy trình xác thực:
1. Người dùng gửi tài khoản và mật khẩu.
2. API kiểm tra thông tin trong bảng NguoiDungs.
3. API tạo JWT Token.
4. Client lưu Token trong SessionManager.
5. Client gửi Token trong Header Authorization.
6. API kiểm tra Token và vai trò người dùng.
7. API cho phép hoặc từ chối truy cập chức năng.
Header xác thực:
Authorization: Bearer {token}
Token có thời hạn sử dụng 2 giờ.
🖥️ Windows Forms Client
Các màn hình chính:
FormLogin
Chức năng:
- Nhập tài khoản.
- Nhập mật khẩu.
- Gọi API đăng nhập.
- Nhận JWT Token.
- Lưu thông tin phiên đăng nhập.
- Mở giao diện chính sau khi đăng nhập.
FormMainShell
Chức năng:
- Hiển thị giao diện chính.
- Hiển thị tên nhân viên.
- Hiển thị vai trò.
- Điều hướng đến các chức năng.
- Ẩn hoặc khóa chức năng theo quyền.
FormPOS
Chức năng:
- Nhập hoặc quét mã vạch.
- Thêm sản phẩm vào giỏ hàng.
- Hiển thị tên sản phẩm.
- Hiển thị đơn giá.
- Thay đổi số lượng.
- Tính thành tiền.
- Xóa sản phẩm khỏi giỏ hàng.
- Tìm khách hàng bằng số điện thoại.
- Nhập tiền khách đưa.
- Tính tiền thừa.
- Thanh toán bằng phím F9.
- Hủy giỏ hàng.
FormCategoryManagement
Chức năng:
- Xem danh sách nhóm hàng.
- Tìm kiếm nhóm hàng.
- Thêm nhóm hàng.
- Cập nhật nhóm hàng.
- Xóa nhóm hàng.
FormProductManagement
Chức năng:
- Xem danh sách sản phẩm.
- Tìm kiếm sản phẩm.
- Thêm sản phẩm.
- Cập nhật sản phẩm.
- Xóa sản phẩm.
- Cập nhật số lượng tồn kho.
FormCustomerManagement
Chức năng:
- Xem danh sách khách hàng.
- Tìm kiếm khách hàng.
- Thêm khách hàng.
- Cập nhật khách hàng.
- Xóa khách hàng theo quyền.
FormUserManagement
Chức năng dành cho Admin:
- Xem danh sách tài khoản.
- Thêm tài khoản.
- Đặt lại mật khẩu.
- Khóa tài khoản.
- Mở khóa tài khoản.
- Tải lại danh sách tài khoản.
FormQuickReport
Chức năng dành cho Admin:
- Chọn ngày báo cáo.
- Xem tổng doanh thu.
- Xem số đơn hàng.
- Xem tổng sản phẩm đã bán.
- Xem sản phẩm bán chạy nhất.
▶️ Hướng dẫn chạy dự án
1. Khởi động SQL Server
Đảm bảo SQL Server Express đang chạy:
.\SQLEXPRESS
Kiểm tra database:
SonNguyenMiniSupermarketDb
Nếu database chưa tồn tại, có thể chạy migration bằng Package Manager Console:
Update-Database
Hoặc dùng .NET CLI:
dotnet ef database update --project .\MiniSupermarket.API
2. Chạy ASP.NET Core Web API
Mở solution trong Visual Studio.
Chọn project:
MiniSupermarket.API
Nhấn:
F5
Hoặc chạy profile HTTPS.
API sẽ chạy tại:
https://localhost:7163
Mở Swagger:
https://localhost:7163/swagger/index.html
3. Kiểm tra đăng nhập bằng Swagger
Trong Swagger, mở endpoint:
POST /api/auth/login
Chọn:
Try it out
Nhập:
{
  "username": "admin01",
  "password": "123456"
}
Chọn:
Execute
Nếu đăng nhập thành công, API trả về JWT Token.
Sao chép giá trị Token.
Chọn nút:
Authorize
Dán Token vào ô Bearer Authentication.
Nếu giao diện Swagger yêu cầu nhập đầy đủ tiền tố thì nhập:
Bearer {token}
Nếu Swagger tự thêm tiền tố Bearer thì chỉ cần dán chuỗi Token.
Sau khi Authorize thành công, có thể kiểm thử các API yêu cầu đăng nhập.
4. Kiểm tra API sản phẩm
Thử endpoint:
GET /api/products
Thử tìm sản phẩm bằng mã vạch:
GET /api/products/barcode/893000000001
Kết quả phải trả về sản phẩm:
Gạo lứt hữu cơ
5. Kiểm tra API khách hàng
Thử endpoint:
GET /api/customers
Thử tìm kiếm khách hàng theo số điện thoại:
GET /api/customers/search
Có thể sử dụng số điện thoại mẫu:
0900000001
6. Kiểm tra API báo cáo
Đăng nhập bằng tài khoản Admin.
Thử endpoint:
GET /api/reports/quick?date=2026-10-01
Kiểm tra các thông tin:
- Số lượng đơn hàng.
- Tổng doanh thu.
- Tổng số sản phẩm đã bán.
- Sản phẩm bán chạy nhất.
7. Kiểm tra phân quyền
Đăng nhập bằng tài khoản:
cashier01 / 123456
Thử truy cập:
GET /api/reports/quick
Kết quả mong đợi:
403 Forbidden
Thử truy cập chức năng quản lý người dùng:
GET /api/users
Kết quả mong đợi:
403 Forbidden
Đăng nhập bằng:
ware01 / 123456
Có thể kiểm tra quyền quản lý sản phẩm.
Warehouse không được phép truy cập báo cáo doanh thu.
8. Chạy Windows Forms
Sau khi API đang chạy, chạy project:
MiniSupermarket.WinForms
Đăng nhập bằng:
Tài khoản: admin01
Mật khẩu: 123456
Sau khi đăng nhập thành công, hệ thống mở giao diện chính.
Từ menu bên trái có thể truy cập:
- Bán hàng POS.
- Quản lý nhóm hàng.
- Quản lý sản phẩm.
- Quản lý khách hàng.
- Báo cáo doanh thu.
- Quản lý tài khoản.
🧪 Quy trình kiểm thử chức năng POS
Bước 1: Mở màn hình POS
Đăng nhập bằng:
admin01 / 123456
Chọn:
Bán hàng (POS)
Bước 2: Thêm sản phẩm
Nhập mã vạch:
893000000001
Nhấn:
Enter
Sản phẩm phải xuất hiện trong giỏ hàng.
Bước 3: Thêm sản phẩm thứ hai
Nhập:
893000000003
Nhấn:
Enter
Kiểm tra:
- Tên sản phẩm.
- Đơn giá.
- Số lượng.
- Thành tiền.
- Tổng tiền cần thanh toán.
Bước 4: Tìm khách hàng
Nhập số điện thoại:
0900000001
Nhấn:
Tìm KH
Tên khách hàng phải được hiển thị.
Bước 5: Nhập tiền khách đưa
Nhập số tiền lớn hơn tổng tiền cần thanh toán.
Ví dụ:
500000
Kiểm tra tiền thừa.
Bước 6: Thanh toán
Nhấn nút:
THANH TOÁN (F9)
Hoặc nhấn phím:
F9
Hệ thống phải:
- Tạo đơn hàng.
- Lưu chi tiết đơn hàng.
- Trừ tồn kho.
- Hiển thị thông báo thanh toán thành công.
- Xóa giỏ hàng sau khi thanh toán.
Bước 7: Kiểm tra dữ liệu sau thanh toán
Mở SQL Server và kiểm tra:
DonHangs
ChiTietDonHangs
SanPhams
Số lượng tồn kho của sản phẩm đã bán phải giảm.
🧪 Kết quả kiểm thử
Các nội dung đã kiểm tra:
- ✅ API chạy thành công.
- ✅ Swagger hiển thị danh sách API.
- ✅ Đăng nhập bằng tài khoản Admin thành công.
- ✅ Đăng nhập bằng tài khoản Cashier thành công.
- ✅ Đăng nhập bằng tài khoản Warehouse thành công.
- ✅ JWT Token được tạo sau khi đăng nhập.
- ✅ API kiểm tra được quyền truy cập theo vai trò.
- ✅ API nhóm hàng kết nối SQL Server.
- ✅ API sản phẩm kết nối SQL Server.
- ✅ Tìm sản phẩm bằng mã vạch.
- ✅ API khách hàng hoạt động.
- ✅ API tạo đơn hàng hoạt động.
- ✅ API báo cáo doanh thu hoạt động.
- ✅ WinForms kết nối được với Web API.
- ✅ POS thêm sản phẩm vào giỏ hàng.
- ✅ POS tính tổng tiền.
- ✅ POS tính tiền thừa.
- ✅ POS thanh toán bằng phím F9.
- ✅ Sau thanh toán, tồn kho được cập nhật.
- ✅ Giao diện POS hiển thị khu vực sản phẩm và thanh toán hợp lý.
- ✅ Giao diện báo cáo doanh thu không bị chồng nút.
- ✅ Giao diện quản lý tài khoản không bị chồng nút.
- ✅ Dữ liệu vẫn còn sau khi dừng và chạy lại ứng dụng.
📁 Cấu trúc chính của dự án
MiniSupermarket.API-upload
│
├── MiniSupermarket.API
│   ├── Controllers
│   │   ├── AuthController.cs
│   │   ├── CategoriesController.cs
│   │   ├── CustomersController.cs
│   │   ├── OrdersController.cs
│   │   ├── ProductsController.cs
│   │   ├── ReportsController.cs
│   │   └── UsersController.cs
│   │
│   ├── Data
│   │   └── SupermarketDbContext.cs
│   │
│   ├── Models
│   │   ├── Category.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderDetail.cs
│   │   ├── Product.cs
│   │   └── User.cs
│   │
│   ├── Migrations
│   │   ├── 20261008122638_CreateGreenLifeSupermarket.cs
│   │   ├── 20261011025419_AddWarehouseUser.cs
│   │   └── 20261015065651_CompleteBuoi4Features.cs
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── MiniSupermarket.API.csproj
│
├── MiniSupermarket.WinForms
│   ├── FormLogin.cs
│   ├── FormMainShell.cs
│   ├── FormPOS.cs
│   ├── FormCategoryManagement.cs
│   ├── FormProductManagement.cs
│   ├── FormCustomerManagement.cs
│   ├── FormUserManagement.cs
│   ├── FormQuickReport.cs
│   ├── SessionManager.cs
│   └── MiniSupermarket.WinForms.csproj
│
└── README.md
🎯 Kết quả đạt được Buổi 4
Hoàn thành các nội dung chính:
- ✅ Hoàn thiện hệ thống đăng nhập.
- ✅ Sử dụng JWT Bearer Authentication.
- ✅ Phân quyền Admin, Cashier và Warehouse.
- ✅ Hoàn thiện API nhóm hàng.
- ✅ Hoàn thiện API sản phẩm.
- ✅ Hoàn thiện API khách hàng.
- ✅ Hoàn thiện API đơn hàng.
- ✅ Hoàn thiện API người dùng.
- ✅ Hoàn thiện API báo cáo doanh thu.
- ✅ Xây dựng giao diện bán hàng POS.
- ✅ Tìm sản phẩm bằng mã vạch.
- ✅ Tìm khách hàng bằng số điện thoại.
- ✅ Tính tổng tiền và tiền thừa.
- ✅ Thanh toán và tạo đơn hàng.
- ✅ Cập nhật tồn kho sau khi bán hàng.
- ✅ Xây dựng giao diện quản lý sản phẩm.
- ✅ Xây dựng giao diện quản lý khách hàng.
- ✅ Xây dựng giao diện quản lý tài khoản.
- ✅ Xây dựng giao diện báo cáo doanh thu.
- ✅ Kết nối toàn bộ Windows Forms với Web API.
- ✅ Kiểm thử API bằng Swagger.
- ✅ Kiểm thử giao diện bằng Windows Forms.
- ✅ Lưu trữ dữ liệu trên SQL Server.
👨‍💻 Tác giả
- Họ tên sinh viên: Nguyễn Vũ Trường Sơn
- Mã sinh viên: 2124110109
- Lớp học phần: CCQ2411D
- Đề tài: Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core - Siêu thị Mini Tổng hợp Đời Sống Xanh
