🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI — BUỔI 6
📚 1. Thông tin chung
- Môn học: Lập trình Ứng dụng .NET Core — 229162
- Buổi 6: Entity Framework Core nâng cao, phân trang LINQ và tích hợp WinForms Client – Server
- Công nghệ: C#, .NET 8, ASP.NET Core Web API, Entity Framework Core, Windows Forms, SQL Server
- Database: SonNguyenMiniSupermarketDb
- SQL Server trên máy thực hành: .\SQLEXPRESS
Buổi 6 tập trung vào việc truy vấn dữ liệu bằng Entity Framework Core, phân trang tại máy chủ bằng LINQ và tích hợp các chức năng quản lý vào WinForms thông qua Web API.

Luồng hoạt động:
Người dùng thao tác trên WinForms
              │
              ▼
ApiClientService gửi HTTP Request kèm JWT
              │
              ▼
Web API xác thực và kiểm tra quyền
              │
              ▼
Controller xây dựng truy vấn LINQ
              │
              ▼
Entity Framework Core truy vấn SQL Server
              │
              ▼
API trả dữ liệu JSON
              │
              ▼
WinForms cập nhật bảng và thông tin phân trang
✅ 2. Nội dung đã thực hiện
2.1. Phân trang sản phẩm tại máy chủ
- Xây dựng API lấy danh sách sản phẩm theo trang.
- Sắp xếp sản phẩm bằng OrderBy().
- Bỏ qua dữ liệu của các trang trước bằng Skip().
- Giới hạn số dòng của trang hiện tại bằng Take().
- Trả về tổng số sản phẩm và tổng số trang.
- WinForms chỉ nhận dữ liệu của trang đang xem.
2.2. Tìm kiếm và lọc dữ liệu
- Tìm kiếm sản phẩm theo tên hoặc mã vạch.
- Dùng LINQ Where() và Contains().
- Lọc sản phẩm theo nhóm hàng.
- Kết hợp từ khóa, nhóm hàng và phân trang trong cùng yêu cầu.
- Khi thay đổi điều kiện tìm kiếm hoặc nhóm hàng, giao diện quay về trang đầu.
2.3. Nạp dữ liệu liên quan và tối ưu truy vấn đọc
- Dùng Include() để khai báo nạp thông tin nhóm hàng liên quan.
- Trả tên nhóm hàng cùng thông tin sản phẩm.
- Dùng AsNoTracking() cho truy vấn chỉ đọc.
- Dùng Select() để chọn các trường cần trả về DTO.
- Sử dụng các hàm bất đồng bộ như CountAsync() và ToListAsync().
2.4. Tích hợp WinForms với Web API
- Xây dựng ApiClientService dùng chung HttpClient.
- Cấu hình địa chỉ Backend tập trung.
- Tự động gửi JWT Bearer Token.
- Đọc dữ liệu JSON từ API.
- Hiển thị danh sách sản phẩm trên DataGridView.
- Hiển thị trang hiện tại, tổng số trang và tổng số sản phẩm.
- Xử lý thông báo lỗi kết nối bằng tiếng Việt.
2.5. Quản lý sản phẩm qua API
- Xem danh sách sản phẩm có phân trang.
- Tìm kiếm và lọc nhóm hàng.
- Thêm sản phẩm.
- Cập nhật sản phẩm.
- Xóa sản phẩm theo điều kiện nghiệp vụ.
- Tải lại danh sách sau khi thao tác thành công.
- Tự lùi về trang hợp lệ khi xóa dòng cuối của trang cuối.
2.6. Bài tập 6.1 — Phân trang khách hàng
- Xây dựng API phân trang khách hàng.
- Sắp xếp theo điểm tích lũy giảm dần.
- Dùng mã khách hàng làm thứ tự phụ để kết quả ổn định khi bằng điểm.
- Hiển thị dữ liệu và điểm tích lũy trên WinForms.
2.7. Bài tập 6.2 — Lọc bằng ComboBox nhóm hàng
- Tải danh sách nhóm hàng từ API.
- Hiển thị tên nhóm trong ComboBox.
- Gửi mã nhóm đang chọn lên API.
- Cập nhật danh sách sản phẩm theo nhóm.
- Kết hợp lọc nhóm với từ khóa tìm kiếm và phân trang.
🗂️ 3. Cấu trúc mã nguồn liên quan
MiniSupermarket.API-upload/
│
├── MiniSupermarket.API/
│   ├── Controllers/
│   │   ├── ProductsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── CustomersController.cs
│   │   └── AuthController.cs
│   │
│   ├── Data/
│   │   └── SupermarketDbContext.cs
│   │
│   ├── Models/
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Customer.cs
│   │   └── ProductDtos.cs
│   │
│   ├── Migrations/
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── Program.cs
│   └── MiniSupermarket.API.sln
│
└── MiniSupermarket.WinForms/
    ├── Services/
    │   └── ApiClientService.cs
    ├── FormLogin.cs
    ├── FormMainShell.cs
    ├── FormProductManagement.cs
    ├── FormCustomerManagement.cs
    └── Program.cs
Vai trò các thành phần
Thành phần	Vai trò
ProductsController.cs	Phân trang, tìm kiếm, lọc và CRUD sản phẩm
CategoriesController.cs	Cung cấp dữ liệu nhóm hàng
CustomersController.cs	Quản lý và phân trang khách hàng
SupermarketDbContext.cs	Cấu hình Entity Framework Core và các DbSet
ProductDtos.cs	Dữ liệu nhận khi thêm/sửa và dữ liệu sản phẩm trả về
ApiClientService.cs	HttpClient dùng chung, BaseAddress và Bearer Token
FormProductManagement.cs	Giao diện quản lý sản phẩm
FormCustomerManagement.cs	Giao diện quản lý khách hàng
FormMainShell.cs	Điều hướng và hiển thị menu theo quyền
FormLogin.cs	Đăng nhập và lưu thông tin phiên


⚙️ 4. Yêu cầu môi trường
Chuẩn bị:
- Windows để chạy WinForms.
- .NET 8 SDK.
- SQL Server / SQL Server Express.
- SQL Server Management Studio — SSMS.
- Visual Studio với:
  - ASP.NET and web development.
  - .NET desktop development.
- Git nếu tải mã nguồn bằng lệnh clone.
Kiểm tra SDK:
dotnet --list-sdks
Mã nguồn và database cần cùng phiên bản cấu trúc để tránh lỗi thiếu bảng hoặc cột.
🗄️ 5. Cấu hình database
5.1. Database sử dụng
Dự án sử dụng duy nhất:
SonNguyenMiniSupermarketDb
Server trên máy thực hành:
.\SQLEXPRESS
5.2. Chuỗi kết nối
Mở:
MiniSupermarket.API/appsettings.json
Cấu hình phần kết nối:
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SonNguyenMiniSupermarketDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
Giữ nguyên các phần cấu hình khác trong file khi chỉnh sửa.
Thành phần	Ý nghĩa
Server	SQL Server cần kết nối
Database	Database của ứng dụng
Trusted_Connection=True	Xác thực bằng tài khoản Windows
TrustServerCertificate=True	Tin cậy chứng chỉ SQL Server trong môi trường thực hành


Nếu dùng SQL Server khác, sửa Server cho đúng tên máy hoặc instance.
5.3. Áp dụng Migration
Trong Visual Studio:
1. Mở Solution.
2. Chọn API làm Startup Project.
3. Mở Tools → NuGet Package Manager → Package Manager Console.
4. Chọn Default project: MiniSupermarket.API.
5. Chạy:
Update-Database
Lệnh này áp dụng các Migration còn thiếu vào database đã cấu hình.
Dữ liệu được bổ sung trực tiếp trên máy thực hành không tự đi theo Git. Khi chạy trên máy khác, tổng số sản phẩm, khách hàng và số trang có thể khác.

▶️ 6. Chạy Backend — Web API
Cách 1: Visual Studio
1. Mở:
   MiniSupermarket.API/MiniSupermarket.API.sln
2. Nhấn chuột phải project MiniSupermarket.API.
3. Chọn Set as Startup Project.
4. Chọn profile https.
5. Nhấn F5 hoặc Ctrl + F5.
Địa chỉ Backend:
https://localhost:7163
http://localhost:5167
Swagger:
https://localhost:7163/swagger/index.html
Cách 2: Terminal
Mở terminal tại thư mục gốc repository:
dotnet restore MiniSupermarket.API/MiniSupermarket.API.csproj
Chạy:
dotnet run --project MiniSupermarket.API --launch-profile https
Backend chạy thành công khi xuất hiện thông báo tương tự:
Now listening on: https://localhost:7163
Now listening on: http://localhost:5167
Chứng chỉ HTTPS
Nếu máy chưa tin cậy chứng chỉ HTTPS của .NET:
dotnet dev-certs https --trust
Xác nhận yêu cầu tin cậy chứng chỉ khi hệ điều hành hiển thị.
Tránh chạy trùng API
Nếu API đã chạy trong Visual Studio thì không khởi chạy thêm một phiên khác trên cùng cổng.

Lỗi:
Failed to bind to address ...
address already in use
thường xuất hiện khi cổng đã được sử dụng.
🖥️ 7. Chạy Frontend — WinForms
7.1. Kiểm tra địa chỉ API
Mở:
MiniSupermarket.WinForms/Services/ApiClientService.cs
Địa chỉ mặc định:
BaseAddress = new Uri("https://localhost:7163/api/");
Địa chỉ này cần khớp với Backend đang chạy.
7.2. Chạy bằng Visual Studio
Sau khi API chạy:
1. Nhấn chuột phải project MiniSupermarket.WinForms.
2. Chọn Debug → Start New Instance.
3. Đăng nhập trên cửa sổ WinForms.
Có thể cấu hình Multiple Startup Projects để chạy đồng thời API và WinForms.
7.3. Chạy bằng terminal
Mở terminal thứ hai tại thư mục gốc repository:
dotnet restore MiniSupermarket.WinForms/MiniSupermarket.WinForms.csproj
Chạy:
dotnet run --project MiniSupermarket.WinForms
Terminal	Chương trình
Terminal 1	Web API
Terminal 2	WinForms


Giữ API chạy trong suốt quá trình sử dụng WinForms.
7.4. Đăng nhập và quyền sử dụng
Tài khoản mẫu:
Tài khoản	Mật khẩu	Mục đích
admin01	123456	Quản lý sản phẩm, khách hàng và các chức năng quản trị
cashier01	123456	Bán hàng và quản lý khách hàng theo quyền hiện tại


Tài khoản cần tồn tại và đang hoạt động trong database.

Phân quyền hiện tại:
Vai trò	Quản lý sản phẩm	Quản lý khách hàng	Báo cáo doanh thu
Admin	Có	Có	Có
Warehouse	Có	Không	Không
Cashier	Không	Có, không được xóa	Không


Sử dụng Admin hoặc tài khoản Warehouse hiện có để kiểm thử màn hình sản phẩm.

Cashier chỉ thấy các menu được cấp quyền; các menu đang hiển thị được xếp liền nhau trên thanh bên trái.
🌐 8. API phân trang sản phẩm
Endpoint
GET /api/products/paged
Tham số
Tham số	Kiểu	Ý nghĩa
keyword	string	Tìm theo tên hoặc mã vạch
categoryId	int	Mã nhóm hàng
pageNumber	int	Trang cần lấy
pageSize	int	Số sản phẩm mỗi trang


Giá trị mặc định:
pageNumber = 1
pageSize = 5
Quy tắc xử lý hiện tại:
- pageNumber nhỏ hơn 1 được đưa về 1.
- pageSize ngoài khoảng 1–50 được đưa về 5.
- Không có từ khóa thì không áp dụng điều kiện tìm kiếm.
- Mã nhóm bằng 0 hoặc không truyền thì không lọc theo nhóm.
Ví dụ gọi
Trang đầu:
/api/products/paged?pageNumber=1&pageSize=5
Trang thứ hai:
/api/products/paged?pageNumber=2&pageSize=5
Tìm tên:
/api/products/paged?keyword=Gạo&pageNumber=1&pageSize=5
Lọc nhóm:
/api/products/paged?categoryId=1&pageNumber=1&pageSize=5
Kết hợp:
/api/products/paged?keyword=Gạo&categoryId=1&pageNumber=1&pageSize=5
Khi WinForms tạo URL, từ khóa được mã hóa để truyền an toàn qua query string.
📦 9. Cấu trúc kết quả phân trang
API trả kết quả theo cấu trúc:
{
  "currentPage": 1,
  "pageSize": 5,
  "totalItems": 1,
  "totalPages": 1,
  "items": [
    {
      "productId": 1,
      "barcode": "893000000001",
      "productName": "Gạo lứt hữu cơ",
      "price": 85000,
      "stockQuantity": 46,
      "categoryId": 1,
      "categoryName": "Thực phẩm hữu cơ"
    }
  ]
}
Đây là ví dụ cấu trúc phản hồi. Dữ liệu, giá, tồn kho và số lượng thực tế phụ thuộc database.
Trường	Ý nghĩa
currentPage	Trang được trả về
pageSize	Kích thước trang
totalItems	Tổng số sản phẩm sau khi tìm kiếm và lọc
totalPages	Tổng số trang của kết quả
items	Danh sách sản phẩm trong trang


Điểm cần phân biệt:
- totalItems không phải lúc nào cũng là toàn bộ sản phẩm trong database.
- Khi có điều kiện tìm kiếm hoặc nhóm hàng, nó là tổng số sản phẩm thỏa điều kiện.
- items chỉ chứa dữ liệu của trang đang lấy.
🔍 10. Tìm kiếm và lọc bằng LINQ
10.1. Tìm kiếm
API dùng điều kiện tương tự:
query = query.Where(p =>
    p.TenSanPham.ToLower().Contains(search)
    || p.MaVach.Contains(search));
Ví dụ nhập:
gạo
có thể tìm được sản phẩm có tên chứa từ khóa đó.

“Gần đúng” trong chức năng này là tên có chứa từ khóa, không phải tự sửa lỗi chính tả.
10.2. Lọc nhóm hàng
query = query.Where(p => p.MaNhomHang == categoryId);
Chỉ lấy sản phẩm thuộc nhóm đã chọn.
10.3. Thứ tự xử lý
Danh sách sản phẩm
        ↓
Lọc từ khóa
        ↓
Lọc nhóm hàng
        ↓
Đếm tổng số kết quả
        ↓
Sắp xếp
        ↓
Skip / Take
        ↓
Trả dữ liệu của trang
Phân trang được áp dụng sau các điều kiện lọc, giúp số trang tương ứng đúng với kết quả tìm kiếm.
📄 11. Phân trang bằng OrderBy, Skip và Take
Các lệnh chính:
.OrderBy(p => p.MaSanPham)
.Skip((int)offset)
.Take(pageSize)
Cách tính số dòng cần bỏ qua:
long offset = (long)(pageNumber - 1) * pageSize;
Ví dụ mỗi trang 5 sản phẩm:
Trang	Số dòng bỏ qua	Số dòng lấy
1	0	Tối đa 5
2	5	Tối đa 5
3	10	Tối đa 5


Vai trò từng lệnh
- OrderBy(): xác định thứ tự trước khi chia trang.
- Skip(): bỏ qua các dòng của trang trước.
- Take(): giới hạn số dòng của trang hiện tại.
Tổng số trang:
int totalPages =
    (int)Math.Ceiling((double)totalItems / pageSize);
Ví dụ:
100 sản phẩm / 5 sản phẩm mỗi trang = 20 trang.
Đây là minh họa công thức; số trang thực tế được tính theo dữ liệu và điều kiện lọc.
🔗 12. Include, AsNoTracking và DTO
12.1. Include
Trong truy vấn sản phẩm:
.Include(p => p.NhomHang)
khai báo lấy dữ liệu nhóm hàng liên quan.

Thông tin tên nhóm được đưa vào DTO:
CategoryName = p.NhomHang != null
    ? p.NhomHang.TenNhomHang
    : string.Empty
Nhờ đó, WinForms hiển thị tên nhóm thay vì chỉ hiển thị mã nhóm.

Với truy vấn có Select() trực tiếp sang DTO, EF Core còn có thể tạo phép nối cần thiết từ việc truy cập thuộc tính NhomHang.TenNhomHang trong phép chiếu.
12.2. AsNoTracking
.AsNoTracking()
được dùng cho truy vấn chỉ đọc.

Mục đích là tránh chi phí theo dõi thay đổi Entity khi không cần cập nhật đối tượng vừa đọc.

Trong truy vấn chỉ chiếu các giá trị sang DTO, EF Core cũng không theo dõi Entity nếu kết quả không chứa Entity.
12.3. DTO
API dùng DTO để quy định dữ liệu truyền qua mạng:
- Dữ liệu nhận khi thêm sản phẩm.
- Dữ liệu nhận khi sửa sản phẩm.
- Dữ liệu sản phẩm trả về.
- Dữ liệu kết quả phân trang.
WinForms đọc JSON và chuyển thành các đối tượng C# để hiển thị.
🔐 13. ApiClientService và JWT
File:
MiniSupermarket.WinForms/Services/ApiClientService.cs
Class này cung cấp:
- HttpClient dùng chung.
- Địa chỉ Backend.
- Hàm tạo request.
- Gắn Bearer Token.
- Thông báo lỗi kết nối thân thiện.
Header xác thực:
Authorization: Bearer <JWT_TOKEN>
Luồng sử dụng:
Đăng nhập thành công
        ↓
Lưu Token vào SessionManager
        ↓
Form tạo yêu cầu qua ApiClientService
        ↓
Token được gửi kèm
        ↓
API xác thực và kiểm tra quyền
Người dùng đăng nhập một lần để sử dụng các màn hình được cấp quyền trong thời gian Token còn hợp lệ.
🖱️ 14. Luồng hoạt động của màn hình sản phẩm
14.1. Mở màn hình
Sự kiện Load gọi:
await LoadCategoriesAsync();
await LoadProductsAsync(1);
- Tải nhóm hàng cho ComboBox.
- Tải trang sản phẩm đầu tiên.
14.2. Tải danh sách
Form gửi yêu cầu:
GET /api/products/paged
kèm từ khóa, nhóm hàng, trang và kích thước trang.

Sau khi nhận kết quả:
1. Đọc JSON.
2. Lấy items.
3. Chuyển dữ liệu thành nguồn cho bảng.
4. Cập nhật nhãn phân trang.
5. Cập nhật trạng thái nút Trang trước / Trang sau.
14.3. Tìm kiếm
Bấm Tìm kiếm hoặc Enter tại ô từ khóa:
await LoadProductsAsync(1);
Kết quả quay về trang đầu của danh sách mới.

Màn hình hiện tại tìm kiếm thông qua endpoint products/paged có tham số keyword. Endpoint products/search vẫn là một API tìm kiếm riêng.
14.4. Đổi nhóm hàng
Khi thay đổi ComboBox lọc:
- Đọc mã nhóm.
- Giữ từ khóa hiện tại.
- Gửi yêu cầu mới.
- Quay về trang 1.
14.5. Chuyển trang
await LoadProductsAsync(_currentPage - 1);
hoặc:
await LoadProductsAsync(_currentPage + 1);
Các điều kiện tìm kiếm và lọc nhóm tiếp tục được gửi kèm.
✏️ 15. Thêm, sửa và xóa sản phẩm
Thêm sản phẩm
Nhập thông tin
    ↓
Form kiểm tra đầu vào cơ bản
    ↓
POST /api/products
    ↓
API nhận CreateProductDto
    ↓
Kiểm tra dữ liệu và nhóm hàng
    ↓
EF Core lưu SQL
    ↓
Trả 201
    ↓
WinForms thông báo và tải lại trang
Sửa sản phẩm
Chọn một dòng
    ↓
Thông tin được đưa lên ô nhập
    ↓
Sửa dữ liệu
    ↓
PUT /api/products/{id}
    ↓
API tìm Entity và cập nhật thuộc tính
    ↓
SaveChangesAsync
    ↓
Trả 200 và tải lại danh sách
Xóa sản phẩm
Chọn sản phẩm
    ↓
Xác nhận xóa
    ↓
DELETE /api/products/{id}
    ↓
API kiểm tra điều kiện xóa
    ↓
EF Core xóa nếu được phép
    ↓
WinForms tải lại danh sách
Sản phẩm có dữ liệu liên quan có thể bị từ chối xóa theo điều kiện nghiệp vụ.

Nếu xóa dòng cuối cùng của trang cuối, giao diện tải lại trang cuối còn dữ liệu.
👥 16. Bài 6.1 — Phân trang khách hàng
Endpoint
GET /api/customers/paged
Ví dụ:
/api/customers/paged?pageNumber=1&pageSize=5
Thứ tự hiển thị
- Điểm tích lũy giảm dần.
- Mã khách hàng tăng dần khi bằng điểm.
Ý nghĩa:
- Khách có nhiều điểm được đưa lên trước.
- Thứ tự ổn định khi chuyển trang.
- Có thể theo dõi danh sách khách hàng theo mức tích lũy.
WinForms nhận kết quả phân trang và hiển thị các trường thông tin khách hàng, gồm điểm tích lũy.
🗃️ 17. Bài 6.2 — Lọc bằng ComboBox nhóm hàng
Luồng thực hiện:
Form tải danh sách nhóm từ API
              ↓
ComboBox hiển thị tên nhóm
              ↓
Người dùng chọn nhóm
              ↓
Form lấy mã nhóm đang chọn
              ↓
Gửi categoryId lên API phân trang
              ↓
API Where theo MaNhomHang
              ↓
Hiển thị sản phẩm thuộc nhóm
Ví dụ:
Chọn nhóm: Thực phẩm hữu cơ
Mã nhóm: 1
Yêu cầu gửi lên:
/api/products/paged?categoryId=1&pageNumber=1&pageSize=5
Có thể nhập thêm từ khóa để lọc đồng thời:
/api/products/paged?keyword=Gạo&categoryId=1&pageNumber=1&pageSize=5
🧪 18. Kiểm thử trên Swagger
18.1. Đăng nhập
Gọi:
POST /api/auth/login
Ví dụ:
{
  "username": "admin01",
  "password": "123456"
}
Lấy Token và gắn vào nút Authorize theo hướng dẫn của Swagger.
18.2. Trang đầu
Gọi:
/api/products/paged?pageNumber=1&pageSize=5
Mong đợi:
- 200.
- currentPage là 1.
- items có tối đa 5 sản phẩm.
- Có tổng số sản phẩm và tổng số trang.
18.3. Trang tiếp theo
Gọi:
/api/products/paged?pageNumber=2&pageSize=5
Mong đợi:
- Dữ liệu tương ứng trang 2.
- Thứ tự theo mã sản phẩm.
- Không lặp lại các dòng trang trước khi dữ liệu không thay đổi giữa hai lượt gọi.
18.4. Tìm kiếm
Gọi:
/api/products/paged?keyword=Gạo&pageNumber=1&pageSize=5
Mong đợi:
- Tên hoặc mã vạch phù hợp điều kiện.
- Tổng số kết quả và số trang được tính lại.
18.5. Lọc nhóm
Gọi:
/api/products/paged?categoryId=1&pageNumber=1&pageSize=5
Mong đợi:
- Các sản phẩm trả về có categoryId bằng 1.
- Có categoryName tương ứng.
18.6. Không có kết quả
Dùng từ khóa không tồn tại:
/api/products/paged?keyword=KHONG-TON-TAI-BUOI6&pageNumber=1&pageSize=5
Mong đợi:
- 200.
- items rỗng.
- totalItems bằng 0.
- Không phát sinh lỗi xử lý.
18.7. Kích thước trang không hợp lệ
Gọi:
/api/products/paged?pageNumber=1&pageSize=1000
Theo cách xử lý hiện tại, API đưa pageSize về 5.

Không phải mọi đầu vào ngoài giới hạn đều trả 400; cần đánh giá theo quy tắc chuẩn hóa đã viết trong code.
18.8. Kiểm tra phân quyền
- Bỏ Token và gọi endpoint được bảo vệ → 401.
- Dùng tài khoản không có quyền quản lý sản phẩm → 403.
- Admin hoặc Warehouse hợp lệ → được phép sử dụng API quản lý sản phẩm.
🧪 19. Kiểm thử trực tiếp trên WinForms
Danh sách và phân trang
- [ ] Mở Quản lý Sản phẩm.
- [ ] Trang đầu tải được dữ liệu.
- [ ] Nhãn trang hiển thị đúng.
- [ ] Trang sau lấy dữ liệu trang tiếp theo.
- [ ] Trang trước quay lại đúng.
- [ ] Nút chuyển trang được bật/tắt phù hợp.
Tìm kiếm và lọc
- [ ] Nhập từ khóa rồi bấm Tìm kiếm.
- [ ] Nhấn Enter tại ô từ khóa.
- [ ] Chọn một nhóm hàng.
- [ ] Kết hợp từ khóa và nhóm.
- [ ] Chuyển trang trong kết quả đã lọc.
- [ ] Xóa điều kiện để tải lại danh sách.
- [ ] Từ khóa không có kết quả hiển thị bảng rỗng hợp lệ.
CRUD
- [ ] Thêm sản phẩm thử.
- [ ] Kiểm tra thông báo thành công.
- [ ] Tìm sản phẩm vừa thêm.
- [ ] Sửa tên, giá hoặc tồn kho.
- [ ] Tải lại và xác nhận thay đổi.
- [ ] Xóa sản phẩm thử.
- [ ] Xác nhận sản phẩm không còn trong danh sách.
Khách hàng
- [ ] Danh sách khách hàng tải được.
- [ ] Chuyển trang đúng.
- [ ] Điểm tích lũy hiển thị đúng.
- [ ] Thứ tự điểm giảm dần.
- [ ] Cashier truy cập được khách hàng nhưng không được xóa.
Lỗi kết nối
- [ ] Khi không kết nối được API, hiện thông báo tiếng Việt.
- [ ] Không làm ứng dụng bị văng.
- [ ] Sau khi Backend hoạt động lại, có thể tải lại dữ liệu.
🔎 20. Đối chiếu SQL
Sản phẩm
SELECT
    MaSanPham,
    MaVach,
    TenSanPham,
    GiaBan,
    SoLuongTon,
    MaNhomHang
FROM SanPham
ORDER BY MaSanPham;
Sản phẩm kèm tên nhóm
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    sp.GiaBan,
    sp.SoLuongTon,
    nh.TenNhomHang
FROM SanPham AS sp
INNER JOIN NhomHang AS nh
    ON sp.MaNhomHang = nh.MaNhomHang
ORDER BY sp.MaSanPham;
Khách hàng theo điểm tích lũy
SELECT
    MaKhachHang,
    HoTen,
    SoDienThoai,
    DiemTichLuy
FROM KhachHang
ORDER BY DiemTichLuy DESC, MaKhachHang;
Dùng các truy vấn này để đối chiếu với dữ liệu API và WinForms sau thao tác thêm, sửa, xóa.
🛠️ 21. Một số lỗi thường gặp
Hiện tượng	Nguyên nhân thường gặp	Cách xử lý
API không chạy do trùng cổng	Có phiên API khác đang chạy	Kiểm tra và chỉ dùng một phiên phù hợp
WinForms không tải dữ liệu	Backend chưa chạy hoặc sai BaseAddress	Chạy API, kiểm tra địa chỉ và cổng
Lỗi chứng chỉ HTTPS	Máy chưa tin cậy chứng chỉ phát triển	Chạy dotnet dev-certs https --trust
Lỗi kết nối SQL	Sai server, SQL chưa chạy hoặc thiếu quyền	Kiểm tra chuỗi kết nối và SQL Server
Thiếu bảng hoặc cột	Migration chưa được áp dụng đầy đủ	Chạy Update-Database
API trả 401	Token thiếu hoặc hết hạn	Đăng nhập lại
API trả 403	Sai quyền	Dùng tài khoản được cấp quyền
Tìm kiếm không có dữ liệu	Không có sản phẩm thỏa điều kiện	Kiểm tra từ khóa và nhóm đang chọn
Xóa bị từ chối	Sản phẩm có dữ liệu liên quan	Đọc thông báo và kiểm tra điều kiện nghiệp vụ
Build báo file bị khóa	API hoặc WinForms vẫn chạy	Stop chương trình trước khi build lại


📋 22. Kết quả và phạm vi kiểm thử
Các lần kiểm tra trước đã bao gồm:
- Build API và WinForms.
- Kiểm tra API phân trang, tìm kiếm và lọc nhóm.
- Kiểm tra đầu vào và phân quyền.
- Thao tác chuyển trang, tìm kiếm, lọc và CRUD trên WinForms.
- Đối chiếu dữ liệu được lưu trong SQL.
- Dọn sản phẩm thử sau các lượt kiểm tra tương ứng.
Sau khi thay đổi code, cấu hình, phân quyền hoặc dữ liệu, cần kiểm tra lại các chức năng bị ảnh hưởng.

Phạm vi nội dung buổi 6 trong README này là:
- EF Core nâng cao.
- Tìm kiếm và lọc bằng LINQ.
- Phân trang tại máy chủ.
- Tích hợp API với WinForms.
- Phân trang khách hàng.
- ComboBox lọc nhóm hàng.
Các chức năng bổ sung như Dashboard, biểu đồ, đổi điểm trừ tiền và xuất báo cáo không được tính vào phần thực hành buổi 6 được mô tả ở đây.
👨‍💻 23. Sinh viên thực hiện
- Họ tên: Nguyễn Vũ Trường Sơn
- MSSV: 2124110109
- Lớp học phần: CCQ2411D
- Môn học: Lập trình Ứng dụng .NET Core — 229162
- Đề tài: Xây dựng Hệ thống Quản lý Bán hàng kết hợp Cơ sở dữ liệu Entity Framework Core — Siêu thị Mini Tổng hợp Đời Sống Xanh
