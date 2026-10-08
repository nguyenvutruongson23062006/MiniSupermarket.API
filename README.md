# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI

## 📚 Thông tin

- **Môn học:** Lập trình Ứng dụng .NET Core
- **Mã môn:** 229162
- **Buổi thực hành:** Buổi 3 - Kết nối SQL Server và Entity Framework Core
- **Ngôn ngữ:** C#
- **Framework:** .NET 8
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Client:** Windows Forms

---

## 🏪 Giới thiệu

Trong Buổi 3, dự án được nâng cấp từ việc lưu trữ dữ liệu trên RAM sang lưu trữ dữ liệu trên Microsoft SQL Server bằng Entity Framework Core - Code First.

### Đề tài

**Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core - Siêu thị Mini Tổng hợp Đời Sống Xanh**

---

## 🏗️ Mô hình Kiến trúc Hệ thống

Hệ thống sử dụng mô hình **Client - Server**:

```text
┌─────────────────────────┐
│     WinForms Client     │
│                         │
│  FormLogin              │
│  FormCategoryManagement │
└────────────┬────────────┘
             │ HTTP/HTTPS
             ▼
┌─────────────────────────┐
│    ASP.NET Core API     │
│                         │
│ Controllers             │
│ JWT Authentication      │
│ Entity Framework Core   │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│       SQL Server        │
│                         │
│ SonNguyenMiniSupermarket│
│ Db                       │
└─────────────────────────┘
```

WinForms đóng vai trò **Client**, gửi yêu cầu đến ASP.NET Core Web API.

Web API xử lý yêu cầu và sử dụng **Entity Framework Core** để thao tác với SQL Server.

---

## 💻 Công nghệ sử dụng

- C#
- .NET 8
- ASP.NET Core Web API
- Windows Forms
- Entity Framework Core
- SQL Server 2022 Express
- JWT Bearer
- Swagger

---

## 🗄️ Cơ sở dữ liệu

Đã cài đặt và kết nối SQL Server Express.

- **Server:** `.\SQLEXPRESS`
- **Database:** `SonNguyenMiniSupermarketDb`

Các bảng chính:

- `Categories`
- `Products`

Dữ liệu được lưu trữ trực tiếp trên SQL Server thay vì chỉ lưu tạm trên RAM.

---

## 🔧 Entity Framework Core

Đã cài đặt các package:

- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Design`

Đã xây dựng:

- `Category.cs`
- `Product.cs`
- `SupermarketDbContext.cs`

Thiết lập quan hệ:

**Category 1 - N Product**

Sử dụng **Code First** và **Migration** để tạo và cập nhật Database.

Các Migration đã thực hiện:

- `InitialCreateDatabase`
- `UpdateGreenCategories`

---

## 🌱 Dữ liệu nhóm hàng

Đã thêm dữ liệu mẫu phù hợp với đề tài **Siêu thị Mini Tổng hợp Đời Sống Xanh**:

- Thực phẩm hữu cơ
- Đồ uống thiên nhiên
- Sản phẩm chăm sóc cá nhân
- Đồ dùng gia đình xanh
- Sản phẩm tái chế

---

## 🌐 Categories API

Đã cập nhật `CategoriesController` để làm việc với SQL Server thông qua Entity Framework Core.

Các chức năng:

| Phương thức | Chức năng |
|---|---|
| GET | Lấy danh sách nhóm hàng |
| GET/{id} | Xem nhóm hàng theo ID |
| GET/search | Tìm kiếm nhóm hàng |
| POST | Thêm nhóm hàng |
| PUT | Cập nhật nhóm hàng |
| DELETE | Xóa nhóm hàng |

### Phân quyền

- **Admin:** Xem, thêm, sửa, xóa
- **Cashier:** Xem và tìm kiếm

---

## 🖥️ WinForms Client

Đã kết nối WinForms với Web API.

Các Form chính:

- `FormLogin`
- `FormCategoryManagement`

WinForms có thể lấy và hiển thị dữ liệu nhóm hàng từ SQL Server thông qua API.

API sử dụng:

`https://localhost:7163/api/`

---

## ▶️ Hướng dẫn Chạy và Kiểm thử Dự án

### 1. Khởi động SQL Server

Đảm bảo dịch vụ SQL Server Express đang chạy:

```text
.\SQLEXPRESS
```

Database sử dụng:

```text
SonNguyenMiniSupermarketDb
```

### 2. Chạy ASP.NET Core Web API

Mở project `MiniSupermarket.API` trong Visual Studio.

Nhấn **F5** để chạy API.

Swagger:

```text
https://localhost:7163/swagger/index.html
```

### 3. Kiểm tra API

Trong Swagger:

- Thực hiện đăng nhập để lấy JWT.
- Sử dụng **Authorize** để nhập token.
- Kiểm tra API `GET /api/categories`.
- Kiểm tra API `POST /api/categories`.
- Kiểm tra các chức năng `PUT` và `DELETE` với tài khoản Admin.

### 4. Chạy WinForms

Chạy project:

```text
MiniSupermarket.WinForms
```

Đăng nhập bằng tài khoản đã cấu hình ở Buổi 2.

Sau khi đăng nhập, hệ thống mở:

```text
FormCategoryManagement
```

Tại đây có thể xem và quản lý danh sách nhóm hàng thông qua API.

### 5. Kiểm tra dữ liệu trên SQL Server

Mở **SQL Server Object Explorer** trong Visual Studio.

Truy cập:

```text
.\SQLEXPRESS
└── Databases
    └── SonNguyenMiniSupermarketDb
        └── Tables
            └── dbo.Categories
```

Kiểm tra dữ liệu sau khi thêm hoặc thay đổi.

### 6. Kiểm tra tính lưu trữ

Dừng Web API và WinForms.

Sau đó chạy lại cả hai chương trình.

Nếu dữ liệu nhóm hàng vẫn còn thì chứng minh dữ liệu đã được lưu trên SQL Server và không còn phụ thuộc vào bộ nhớ RAM.

---

## 🧪 Kết quả Kiểm thử

Đã kiểm tra:

- ✅ API lấy dữ liệu trực tiếp từ SQL Server.
- ✅ Thêm nhóm hàng bằng Swagger.
- ✅ Dữ liệu được lưu vào bảng `Categories`.
- ✅ WinForms hiển thị dữ liệu từ Database.
- ✅ Dừng và chạy lại API/WinForms, dữ liệu vẫn còn.
- ✅ Dữ liệu được lưu trữ bền vững trên SQL Server.

---

## 📁 Cấu trúc chính

```text
MiniSupermarket
├── MiniSupermarket.API
│   ├── Controllers
│   ├── Data
│   ├── Models
│   ├── Migrations
│   ├── Program.cs
│   └── appsettings.json
│
├── MiniSupermarket.WinForms
│   ├── FormLogin.cs
│   ├── FormCategoryManagement.cs
│   ├── ApiClientService.cs
│   └── SessionManager.cs
│
└── README.md
```

---

## 🎯 Kết quả

Hoàn thành các nội dung chính của Buổi 3:

- ✅ Kết nối ASP.NET Core Web API với SQL Server.
- ✅ Cài đặt và sử dụng Entity Framework Core.
- ✅ Xây dựng Entity và DbContext.
- ✅ Sử dụng Code First và Migration.
- ✅ Tạo và cập nhật Database.
- ✅ Quản lý nhóm hàng bằng SQL Server.
- ✅ Kết nối Categories API với Database.
- ✅ Kết nối WinForms với API.
- ✅ Kiểm tra dữ liệu được lưu trữ bền vững.

---

## 👨‍💻 Tác giả

- **Họ tên sinh viên:** Nguyễn Vũ Trường Sơn
- **Mã sinh viên:** 2124110109
- **Lớp học phần:** CCQ2411D
- **Đề tài:** Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core - Siêu thị Mini Tổng hợp Đời Sống Xanh
