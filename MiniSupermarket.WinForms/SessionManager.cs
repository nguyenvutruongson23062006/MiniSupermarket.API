global using MiniSupermarket.WinForms.Services;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSupermarket.WinForms
{
    // Quản lý thông tin phiên đăng nhập của người dùng
    public static class SessionManager
    {
        // Lưu JWT Token sau khi đăng nhập
        private static string _jwtToken = string.Empty;
        public static string JwtToken
        {
            get => _jwtToken;
            set
            {
                _jwtToken = value ?? string.Empty;
                ApiClientService.AttachToken(_jwtToken);
            }
        }

        // Lưu vai trò: Admin, Cashier hoặc Warehouse
        public static string CurrentRole { get; set; } = string.Empty;

        // Lưu tên đăng nhập của người dùng
        public static string CurrentUsername { get; set; } = string.Empty;
    }

}
