using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    // DbContext đại diện cho phiên làm việc với cơ sở dữ liệu SQL Server
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext(
            DbContextOptions<SupermarketDbContext> options)
            : base(options)
        {
        }

        // Khai báo các bảng dữ liệu ánh xạ từ Model
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        // Cấu hình dữ liệu mồi ban đầu (Data Seeding)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nạp sẵn 5 danh mục ban đầu vào SQL Server
            // Nạp sẵn 5 danh mục phù hợp với đề tài Siêu thị Mini Tổng hợp Đời Sống Xanh
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Thực phẩm hữu cơ",
                    Description = "Các sản phẩm thực phẩm hữu cơ, tự nhiên"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Đồ uống thiên nhiên",
                    Description = "Nước uống, trà và các sản phẩm có nguồn gốc tự nhiên"
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Sản phẩm chăm sóc cá nhân",
                    Description = "Các sản phẩm chăm sóc cá nhân thân thiện với môi trường"
                },
                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Đồ dùng gia đình xanh",
                    Description = "Đồ dùng gia đình có thể tái sử dụng và thân thiện với môi trường"
                },
                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Sản phẩm tái chế",
                    Description = "Các sản phẩm được làm từ vật liệu tái chế"
                }
            );
        }
    }
}