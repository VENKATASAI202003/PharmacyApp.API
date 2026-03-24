using PharmacyOrderingSystemp5.Models;

namespace PharmacyOrderingSystemp5.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUser(int userId);
        Task<Order?> GetOrder(int id);
        Task AddOrder(Order order);
        Task Save();
    }
}