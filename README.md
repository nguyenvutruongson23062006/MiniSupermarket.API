🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI — BUỔI 5
📚 1. Thông tin chung
- Môn học: Lập trình Ứng dụng .NET Core — 229162
- Buổi thực hành: Buổi 5 — ADO.NET nâng cao, Stored Procedure và giao dịch POS Checkout
- Đề tài: Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — Siêu thị Mini Tổng hợp Đời Sống Xanh
- Công nghệ: C#, .NET 8, ASP.NET Core Web API, Windows Forms, SQL Server, Entity Framework Core và ADO.NET
Buổi 5 tập trung vào việc truy cập SQL Server bằng ADO.NET, xây dựng báo cáo bằng Stored Procedure và xử lý thanh toán POS an toàn bằng SqlTransaction.

Ứng dụng được tổ chức theo mô hình:
Frontend: Windows Forms
          │
          │ HTTP Request + JWT Token
          ▼
Backend: ASP.NET Core Web API
          │
          │ ADO.NET / EF Core
          ▼
Database: SQL Server
          SonNguyenMiniSupermarketDb
WinForms gọi API để lấy và lưu dữ liệu. Các nghiệp vụ ADO.NET của buổi 5 được xử lý ở Backend.
✅ 2. Nội dung đã thực hiện trong Buổi 5
2.1. Thực hành các thành phần cốt lõi của ADO.NET
- Kết nối SQL Server bằng SqlConnection.
- Tạo và thực thi lệnh bằng SqlCommand.
- Truyền dữ liệu đầu vào bằng SqlParameter.
- Đọc dữ liệu bằng SqlDataReader.
- Tải dữ liệu vào bộ nhớ bằng DataTable và SqlDataAdapter.
- Sử dụng using và await using để giải phóng tài nguyên.
- Phân biệt các phương thức thực thi:
Phương thức	Mục đích
ExecuteScalar()	Lấy một giá trị, chẳng hạn tổng số sản phẩm hoặc mã hóa đơn vừa tạo
ExecuteNonQuery()	Thực hiện INSERT, UPDATE, DELETE và nhận số dòng bị tác động
ExecuteReader()	Đọc kết quả truy vấn gồm một hoặc nhiều dòng


Các phiên bản bất đồng bộ như ExecuteScalarAsync(), ExecuteNonQueryAsync() và ExecuteReaderAsync() được sử dụng trong API.
2.2. Tra cứu sản phẩm bằng mã vạch
- Xây dựng API tìm sản phẩm theo barcode bằng ADO.NET.
- Dùng SqlConnection để kết nối database.
- Dùng SqlCommand gửi truy vấn tìm sản phẩm.
- Dùng SqlDataReader đọc thông tin sản phẩm.
- Trả dữ liệu cần cho POS: mã sản phẩm, mã vạch, tên, giá bán và tồn kho.
- Trả thông báo khi không tìm thấy mã vạch.
- Dùng tham số SQL để xử lý đầu vào an toàn.
2.3. Thực hành mô hình Connected và Disconnected
- Connected: đọc trực tiếp dữ liệu qua SqlDataReader khi kết nối đang mở.
- Disconnected: nạp dữ liệu vào DataTable, sau đó xử lý dữ liệu trong bộ nhớ.
- Đếm số lượng sản phẩm theo nhóm hàng.
- Lọc sản phẩm theo ngưỡng tồn kho.
2.4. Báo cáo bằng Stored Procedure
Xây dựng và gọi các Stored Procedure:
- Doanh thu theo thu ngân.
- Doanh thu trong khoảng ngày.
- Top 5 sản phẩm bán chạy.
API sử dụng:
CommandType.StoredProcedure
để gọi thủ tục đã lưu trong SQL Server.
2.5. Thanh toán POS bằng SqlTransaction
Một lần thanh toán thực hiện các thao tác:
1. Kiểm tra tài khoản và dữ liệu giỏ hàng.
2. Kiểm tra khách hàng nếu có.
3. Tạo hóa đơn.
4. Lấy giá và kiểm tra tồn kho thực tế từ SQL.
5. Lưu chi tiết hóa đơn.
6. Trừ tồn kho.
7. Tính tổng tiền và tổng số lượng.
8. Cộng điểm thưởng cho khách hàng.
9. Commit() khi toàn bộ thao tác thành công.
Nếu phát sinh lỗi trong giao dịch, hệ thống gọi Rollback() để hoàn tác các thay đổi.
2.6. Hủy hóa đơn
- Kiểm tra hóa đơn và quyền hủy.
- Đọc danh sách sản phẩm trong hóa đơn.
- Hoàn trả số lượng tồn kho.
- Điều chỉnh điểm thưởng đã cộng.
- Xóa chi tiết hóa đơn và hóa đơn.
- Thực hiện toàn bộ trong cùng một Transaction.
2.7. Tích hợp WinForms POS
- Nhập hoặc quét mã vạch để tìm sản phẩm.
- Quét lại cùng sản phẩm để tăng số lượng.
- Hiển thị giỏ hàng và tổng tiền.
- Nhập tiền khách đưa và tính tiền thừa.
- Chặn thanh toán khi tiền khách đưa không đủ.
- Gửi giỏ hàng tới API Checkout.
- Hiển thị kết quả thanh toán và thông báo lỗi bằng tiếng Việt.
🗂️ 3. Cấu trúc mã nguồn liên quan
MiniSupermarket.API-upload/
│
├── MiniSupermarket.API/
│   ├── Controllers/
│   │   ├── AdoProductController.cs
│   │   ├── AdoReportsController.cs
│   │   └── CheckoutController.cs
│   │
│   ├── Services/
│   │   └── AdoCheckoutService.cs
│   │
│   ├── Models/
│   │   └── CheckoutDtos.cs
│   │
│   ├── Migrations/
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── Program.cs
│   └── MiniSupermarket.API.sln
│
├── MiniSupermarket.WinForms/
│   ├── Services/
│   │   └── ApiClientService.cs
│   ├── FormLogin.cs
│   ├── FormPOS.cs
│   └── Program.cs
│
├── ThucHanhAdoNet_Tiet1/
│   └── Program.cs
│
└── ThucHanhTransaction_Tiet4/
    └── Program.cs
Thành phần	Vai trò
AdoProductController.cs	API tra cứu mã vạch và thực hành đọc dữ liệu ADO.NET
AdoReportsController.cs	Gọi Stored Procedure báo cáo
CheckoutController.cs	Nhận yêu cầu thanh toán, hủy hóa đơn và trả kết quả
AdoCheckoutService.cs	Xử lý nghiệp vụ trong SqlTransaction
CheckoutDtos.cs	Khai báo dữ liệu giỏ hàng và kết quả thanh toán
FormPOS.cs	Giao diện quầy bán hàng
ApiClientService.cs	HttpClient dùng chung, địa chỉ Backend và Bearer Token
ThucHanhAdoNet_Tiet1	Console thực hành Scalar, NonQuery, Reader và DataTable
ThucHanhTransaction_Tiet4	Console minh họa Transaction và Rollback


Solution ứng dụng chính gồm Web API và WinForms. Hai project Console được chạy riêng để minh họa nội dung thực hành tương ứng.
⚙️ 4. Yêu cầu môi trường
Chuẩn bị các thành phần sau:
- Windows để chạy WinForms.
- .NET 8 SDK.
- SQL Server / SQL Server Express.
- SQL Server Management Studio — SSMS để kiểm tra database.
- Visual Studio có các workload:
  - ASP.NET and web development.
  - .NET desktop development.
- Git nếu tải mã nguồn bằng lệnh clone.
Kiểm tra .NET SDK:
dotnet --list-sdks
Danh sách cần có SDK phù hợp để build dự án .NET 8.
🗄️ 5. Cấu hình SQL Server
5.1. Database sử dụng
Ứng dụng sử dụng chung database:
SonNguyenMiniSupermarketDb
SQL Server trên máy thực hành:
.\SQLEXPRESS
5.2. Chuỗi kết nối Backend
Mở file:
MiniSupermarket.API/appsettings.json
Cấu hình phần ConnectionStrings:
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SonNguyenMiniSupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
Đây là một phần trong file JSON hiện có. Giữ lại các cấu hình khác của dự án khi chỉnh sửa.

Thuộc tính	Ý nghĩa
Server	Tên SQL Server cần kết nối
Database	Tên database
Trusted_Connection=True	Sử dụng tài khoản Windows
TrustServerCertificate=True	Tin cậy chứng chỉ SQL Server trong môi trường thực hành


Nếu tên SQL Server trên máy khác với .\SQLEXPRESS, thay phần Server cho phù hợp.
5.3. Cập nhật cấu trúc database bằng Migration
Trong Visual Studio:
1. Mở Solution.
2. Chọn API làm Startup Project.
3. Mở Tools → NuGet Package Manager → Package Manager Console.
4. Chọn Default project là MiniSupermarket.API.
5. Chạy:
Update-Database
EF Core sẽ áp dụng các Migration còn thiếu vào database đã cấu hình.

Migration liên quan trực tiếp đến phần thanh toán buổi 5:
20261022103235_CompleteBuoi5AdoCheckout
Nếu chạy phiên bản mã nguồn đã có nội dung các buổi sau, cần áp dụng đầy đủ Migration của phiên bản đó để code và database đồng bộ.
5.4. Kiểm tra lịch sử Migration
Trong SSMS, mở:
SonNguyenMiniSupermarketDb
└── Tables
    └── dbo.__EFMigrationsHistory
Bảng này do EF Core quản lý để ghi nhận những Migration đã được áp dụng.
5.5. Kiểm tra Stored Procedure
Trong SSMS, mở:
SonNguyenMiniSupermarketDb
└── Programmability
    └── Stored Procedures
Các thủ tục báo cáo buổi 5 gồm:
dbo.sp_GetRevenueByCashier
dbo.sp_GetRevenueByDateRange
dbo.sp_GetTop5SellingProducts
Dữ liệu nhập bổ sung trực tiếp trong database máy thực hành không tự được chuyển theo Git. Khi chạy trên máy khác, số lượng sản phẩm, khách hàng và hóa đơn có thể khác.

▶️ 6. Hướng dẫn chạy Backend — Web API
Cách 1: Chạy bằng Visual Studio
1. Mở file Solution:
   MiniSupermarket.API/MiniSupermarket.API.sln
2. Nhấn chuột phải project MiniSupermarket.API.
3. Chọn Set as Startup Project.
4. Chọn launch profile https.
5. Nhấn F5 hoặc Ctrl + F5.
Backend sử dụng:
HTTPS: https://localhost:7163
HTTP:  http://localhost:5167
Trang Swagger:
https://localhost:7163/swagger/index.html
Cách 2: Chạy bằng terminal
Mở terminal tại thư mục gốc repository — nơi chứa cả thư mục API và WinForms.

Khôi phục thư viện:
dotnet restore MiniSupermarket.API/MiniSupermarket.API.csproj
Chạy Backend:
dotnet run --project MiniSupermarket.API --launch-profile https
Sau khi Backend chạy, mở:
https://localhost:7163/swagger/index.html
Chứng chỉ HTTPS cho môi trường phát triển
Nếu máy chưa tin cậy chứng chỉ HTTPS của .NET, chạy:
dotnet dev-certs https --trust
Xác nhận tin cậy chứng chỉ khi hệ điều hành yêu cầu.
Dấu hiệu Backend đã chạy
Terminal hiển thị thông báo tương tự:
Now listening on: https://localhost:7163
Now listening on: http://localhost:5167
Swagger mở được và hiển thị danh sách endpoint.

Giữ Backend chạy trong suốt quá trình sử dụng WinForms.
🖥️ 7. Hướng dẫn chạy Frontend — WinForms
7.1. Kiểm tra địa chỉ Backend
Mở file:
MiniSupermarket.WinForms/Services/ApiClientService.cs
Địa chỉ mặc định:
BaseAddress = new Uri("https://localhost:7163/api/");
Địa chỉ này cần khớp với Backend đang chạy.

Nếu đổi cổng Backend trong launchSettings.json, cần cập nhật địa chỉ tương ứng tại đây.
7.2. Chạy bằng Visual Studio
Sau khi API đã chạy:
1. Trong Solution Explorer, nhấn chuột phải project MiniSupermarket.WinForms.
2. Chọn Debug → Start New Instance.
3. Cửa sổ đăng nhập WinForms xuất hiện.
Có thể cấu hình Multiple Startup Projects để khởi chạy cả API và WinForms từ cùng Solution.
7.3. Chạy bằng terminal
Mở terminal thứ hai tại thư mục gốc repository.

Khôi phục thư viện:
dotnet restore MiniSupermarket.WinForms/MiniSupermarket.WinForms.csproj
Chạy WinForms:
dotnet run --project MiniSupermarket.WinForms
Hai terminal có nhiệm vụ riêng:
Terminal	Chương trình
Terminal 1	Backend Web API
Terminal 2	Frontend WinForms


7.4. Đăng nhập
Tài khoản mẫu trong môi trường thực hành:
Tài khoản	Mật khẩu	Vai trò
admin01	123456	Quản trị
cashier01	123456	Thu ngân


Tài khoản cần tồn tại và đang hoạt động trong database.

Sau khi đăng nhập thành công:
- WinForms lưu JWT Token.
- ApiClientService gửi Token khi gọi API.
- Người dùng được truy cập các chức năng theo quyền.
7.5. Thử quầy POS
1. Mở Bán hàng POS.
2. Nhập mã vạch của sản phẩm đang có.
3. Nhấn Enter.
4. Kiểm tra sản phẩm được thêm vào giỏ.
5. Nhập lại cùng mã để tăng số lượng.
6. Chọn khách hàng nếu cần tích điểm.
7. Nhập tiền khách đưa.
8. Kiểm tra tổng tiền và tiền thừa.
9. Bấm Thanh toán.
10. Kiểm tra thông báo và đối chiếu dữ liệu SQL.
Ví dụ:
Đơn giá:        85.000đ
Số lượng:            2
Tổng tiền:     170.000đ
Khách đưa:     200.000đ
Tiền thừa:      30.000đ
Giá thực tế lấy theo dữ liệu sản phẩm trong SQL.
🧩 8. Chạy các bài Console
Tiết 1 — Thành phần ADO.NET
Tại thư mục gốc repository:
dotnet run --project ThucHanhAdoNet_Tiet1
Chương trình cung cấp các bài thực hành:
- Đếm sản phẩm bằng ExecuteScalar().
- Cập nhật tồn kho bằng ExecuteNonQuery() và tham số.
- Đọc sản phẩm bằng SqlDataReader.
- Đếm sản phẩm theo nhóm bằng DataTable.
- Ôn tập thành phần ADO.NET và ACID.
Bài cập nhật tồn kho có thể thay đổi dữ liệu thật. Khi thử nên chọn sản phẩm thực hành và ghi lại tồn kho trước khi cập nhật.
Tiết 4 — Transaction và Rollback
dotnet run --project ThucHanhTransaction_Tiet4
Chương trình minh họa:
- Bắt đầu Transaction.
- Thực hiện thao tác cập nhật.
- Giả lập lỗi.
- Rollback.
- Đối chiếu dữ liệu trước và sau.
Hai chương trình Console dùng cấu hình kết nối chung với API theo cách được khai báo trong project.
🌐 9. Các API của Buổi 5
Phương thức	Endpoint	Chức năng
GET	/api/ado-products/barcode/{barcode}	Tìm sản phẩm theo mã vạch
GET	/api/ado-products/filter-stock?minStock=10	Lọc sản phẩm có tồn kho nhỏ hơn hoặc bằng ngưỡng
GET	/api/ado-products/disconnected-cache	Minh họa tải dữ liệu bằng DataTable
GET	/api/ado-products/count-by-category	Đếm sản phẩm theo nhóm
GET	/api/reports/revenue-by-cashier	Báo cáo doanh thu theo thu ngân
GET	/api/reports/revenue-by-date-range	Báo cáo doanh thu theo khoảng ngày
GET	/api/reports/top-selling-products	Top 5 sản phẩm bán chạy bằng Stored Procedure
POST	/api/checkout	Thanh toán POS
DELETE	/api/checkout/cancel/{orderId}	Hủy hóa đơn, hoàn kho và điều chỉnh điểm


Lưu ý: endpoint filter-stock hiện dùng điều kiện tồn kho <= ngưỡng truyền vào, dù tham số có tên minStock.
🔐 10. Xác thực khi kiểm thử Swagger
Trước khi gọi API được bảo vệ:
1. Mở:
   POST /api/auth/login
2. Nhập:
   {
     "username": "admin01",
     "password": "123456"
   }
3. Bấm Try it out → Execute.
4. Sao chép Token trong phản hồi.
5. Bấm Authorize.
6. Nhập Token theo hướng dẫn của hộp thoại Swagger.
7. Xác nhận rồi gọi API.
Trạng thái	Ý nghĩa
200	Yêu cầu thành công
400	Dữ liệu hoặc điều kiện nghiệp vụ không hợp lệ
401	Thiếu Token, Token sai hoặc hết hạn
403	Đã đăng nhập nhưng không có quyền
404	Không tìm thấy dữ liệu


Các API báo cáo ADO.NET yêu cầu Admin. API thanh toán yêu cầu Admin hoặc Cashier.
🧾 11. Luồng thanh toán và quy tắc dữ liệu
11.1. Dữ liệu giỏ hàng
Ví dụ thanh toán cơ bản:
{
  "customerId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2
    }
  ]
}
Trước khi chạy, cần bảo đảm khách hàng và sản phẩm tồn tại, sản phẩm đủ tồn kho.

Có thể bỏ customerId khi thanh toán cho khách vãng lai:
{
  "items": [
    {
      "productId": 1,
      "quantity": 1
    }
  ]
}
11.2. Giá bán
- Giá dùng tính tiền được lấy từ SQL Server.
- Client không được tùy ý quyết định giá thanh toán.
- Nếu gửi unitPrice, API dùng giá đó để kiểm tra giá phía client có bị cũ hay không.
11.3. Thu ngân
- Thu ngân được xác định từ tài khoản trong JWT Token.
- Nếu gửi cashierUsername, thông tin này phải khớp tài khoản đang đăng nhập.
11.4. Tích điểm
Trong luồng thanh toán cơ bản của buổi 5:
Mỗi 10.000đ thanh toán được cộng 1 điểm.
Chỉ lấy phần nguyên.
Ví dụ:
Tổng tiền	Điểm cộng
25.000đ	2
85.000đ	8
170.000đ	17


Khách vãng lai không có tài khoản khách hàng để nhận điểm.
11.5. Các bảng được cập nhật
Bảng	Thao tác
DonHang	Thêm hóa đơn
ChiTietDonHang	Thêm từng dòng sản phẩm
SanPham	Trừ tồn kho
KhachHang	Cộng điểm nếu có khách hàng


🔄 12. Transaction và ACID
Luồng xử lý
BeginTransaction
        │
        ▼
Tạo hóa đơn
        │
        ▼
Lưu chi tiết và trừ kho
        │
        ▼
Cập nhật tổng tiền và điểm
        │
        ├── Thành công → Commit
        │
        └── Có lỗi     → Rollback
Ý nghĩa ACID
Đặc tính	Áp dụng trong hệ thống
Atomicity	Hóa đơn, chi tiết, kho và điểm cùng thành công hoặc cùng hoàn tác
Consistency	Bảo đảm dữ liệu tuân thủ các ràng buộc và điều kiện nghiệp vụ
Isolation	Kiểm soát giao dịch đồng thời để hạn chế xung đột tồn kho
Durability	Sau Commit thành công, dữ liệu được xác nhận lưu trong SQL Server


Vị trí code
MiniSupermarket.API/Services/AdoCheckoutService.cs
Các lệnh chính:
connection.BeginTransaction(...)
transaction.Commit()
transaction.Rollback()
Các SqlCommand trong nghiệp vụ được gắn vào cùng Connection và Transaction.
🧪 13. Kịch bản kiểm thử
13.1. Tra cứu barcode hợp lệ
Thao tác:
- Gọi GET /api/ado-products/barcode/{barcode}.
- Nhập mã vạch đang tồn tại.
Mong đợi:
- Trả 200.
- Đúng tên sản phẩm, giá và tồn kho.
13.2. Barcode không tồn tại
Thao tác:
KHONG-TON-TAI-BUOI5
Mong đợi:
- Trả 404.
- Thông báo không tìm thấy.
- Không thay đổi dữ liệu.
13.3. Đầu vào đặc biệt
Thao tác:

Nhập chuỗi:
' OR 1=1--
vào tham số barcode.

Mong đợi:
- Không trả toàn bộ sản phẩm.
- Không phát sinh lỗi cú pháp SQL.
- Đầu vào được xử lý như một giá trị mã vạch.
Bằng chứng chính về cách xử lý an toàn là code dùng @Barcode và SqlParameter.
13.4. Báo cáo doanh thu
Thao tác:
- Đăng nhập Admin.
- Gọi GET /api/reports/revenue-by-cashier.
Mong đợi:
- Trả 200.
- Có dữ liệu doanh thu theo thu ngân.
- Số liệu đối chiếu được với hóa đơn trong database.
13.5. Thanh toán thành công — Commit
Chuẩn bị:
- Chọn sản phẩm đủ tồn kho.
- Ghi lại tồn kho.
- Chọn khách hàng và ghi lại điểm.
Thao tác:
- Thanh toán giỏ hàng hợp lệ qua Swagger hoặc WinForms.
Mong đợi:
- Có hóa đơn mới.
- Có chi tiết đúng sản phẩm và số lượng.
- Tồn kho giảm đúng.
- Tổng tiền đúng.
- Điểm tăng đúng nếu có khách hàng.
13.6. Thanh toán vượt tồn kho — Rollback
Chuẩn bị:
- Chọn hai sản phẩm.
- Một sản phẩm mua hợp lệ.
- Một sản phẩm mua vượt tồn kho.
- Ghi lại dữ liệu trước khi thử.
Thao tác:
- Gửi yêu cầu thanh toán.
Mong đợi:
- API trả 400.
- Không giữ lại hóa đơn hoặc chi tiết dở dang.
- Tồn kho không bị thay đổi bởi giao dịch thất bại.
- Điểm khách hàng không tăng.
Để minh họa lỗi xảy ra sau khi một mặt hàng đã được xử lý, cần chú ý service hiện xử lý sản phẩm theo thứ tự mã sản phẩm.
Giá trị Identity có thể bị nhảy sau Rollback. Điều này không đồng nghĩa có hóa đơn rác; cần kiểm tra các dòng thực tế trong bảng.

13.7. Hủy hóa đơn
Thao tác:
- Tạo một hóa đơn thử hợp lệ.
- Gọi DELETE /api/checkout/cancel/{orderId} với mã hóa đơn đó.
Mong đợi:
- Hoàn lại tồn kho.
- Điều chỉnh điểm đã cộng.
- Xóa chi tiết và hóa đơn.
- Gọi lại mã đã hủy thì không tìm thấy.
13.8. Thiếu tiền trên WinForms
Thao tác:
- Giỏ hàng có tổng tiền 170.000đ.
- Nhập khách đưa 100.000đ.
- Bấm thanh toán.
Mong đợi:
- Hiện “Chưa đủ tiền!”.
- Thông báo tiền khách đưa chưa đủ.
- Chặn thanh toán.
Nhập lại 200.000đ thì tiền thừa hiển thị 30.000đ.
🔎 14. Đối chiếu dữ liệu SQL
Các truy vấn dưới đây phục vụ kiểm tra dữ liệu trước và sau khi thử.
Kiểm tra sản phẩm và tồn kho
SELECT MaSanPham, TenSanPham, GiaBan, SoLuongTon
FROM SanPham
ORDER BY MaSanPham;
Kiểm tra hóa đơn mới nhất
SELECT TOP (10) *
FROM DonHang
ORDER BY MaDonHang DESC;
Kiểm tra chi tiết một hóa đơn
Thay 123 bằng mã hóa đơn nhận được:
SELECT *
FROM ChiTietDonHang
WHERE MaDonHang = 123;
Kiểm tra điểm khách hàng
SELECT MaKhachHang, HoTen, DiemTichLuy
FROM KhachHang
ORDER BY MaKhachHang;
Khi kiểm chứng Rollback, cần so sánh đúng sản phẩm và khách hàng đã dùng trong yêu cầu thanh toán.
🛠️ 15. Một số lỗi thường gặp
Hiện tượng	Nguyên nhân thường gặp	Cách xử lý
API báo address already in use	Đã có API chạy trên cùng cổng	Kiểm tra phiên đang chạy, tránh khởi chạy trùng
WinForms không kết nối được	API chưa chạy hoặc sai BaseAddress	Chạy Backend và kiểm tra ApiClientService.cs
Lỗi kết nối SQL	Sai tên server, SQL chưa chạy hoặc thiếu quyền	Kiểm tra SQL Server và chuỗi kết nối
Báo thiếu bảng hoặc cột	Database chưa đồng bộ Migration	Chạy Update-Database cho đúng project
Không tìm thấy Stored Procedure	Database chưa có thủ tục tương ứng	Kiểm tra Migration và mục Stored Procedures
API trả 401	Chưa gắn Token hoặc Token hết hạn	Đăng nhập và gắn Token lại
API trả 403	Tài khoản không có quyền	Dùng đúng vai trò cho chức năng
Barcode trả 404	Mã không có trong dữ liệu hiện tại	Kiểm tra mã trong bảng SanPham
Checkout trả 400	Giỏ sai, thiếu tồn kho, khách không tồn tại hoặc giá đã đổi	Đọc thông báo và kiểm tra dữ liệu
Build báo file bị khóa	API hoặc WinForms đang chạy	Stop ứng dụng trước khi build lại


📋 16. Phạm vi kết quả và kiểm chứng
Các phần cần kiểm tra khi báo cáo buổi 5:
- [ ] Chạy được Backend và mở Swagger.
- [ ] Chạy được WinForms và đăng nhập.
- [ ] Thực hành được Scalar, NonQuery và Reader.
- [ ] Tra cứu đúng sản phẩm bằng API ADO.NET.
- [ ] Minh họa được DataTable và Disconnected Mode.
- [ ] Gọi được các Stored Procedure báo cáo.
- [ ] Thanh toán thành công và đối chiếu SQL.
- [ ] Thử vượt tồn kho và xác nhận Rollback.
- [ ] Hủy hóa đơn và kiểm tra hoàn kho, điểm thưởng.
- [ ] Giải thích được Connection, Command, Parameter, Reader và ACID.
Thời gian phản hồi barcode phụ thuộc môi trường, dữ liệu và trạng thái máy. Yêu cầu dưới 100 ms trong bài giảng cần được đo trong phiên kiểm thử thực tế.

Các con số về sản phẩm, thu ngân, hóa đơn và doanh thu phụ thuộc database đang chạy. README không cố định kết quả báo cáo theo dữ liệu mẫu.
👨‍💻 17. Sinh viên thực hiện
- Họ tên: Nguyễn Vũ Trường Sơn
- MSSV: 2124110109
- Lớp học phần: CCQ2411D
- Môn học: Lập trình Ứng dụng .NET Core — 229162
- Đề tài: Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — Siêu thị Mini Tổng hợp Đời Sống Xanh

    
17:37
