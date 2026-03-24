using PharmacyOrderingSystemp5.Models;

namespace PharmacyOrderingSystemp5.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> ReduceStock(int prescriptionId);
        Task<List<Order>> GetOrderHistory(int userId);
        Task<Order> Reorder(int oldOrderId, int userId);
    }
}