namespace MiniSupermarket.API.Models;

public record CashierRevenueDto(string CashierUsername, int TotalOrders,
    decimal TotalRevenue, int TotalPointsAwarded);
