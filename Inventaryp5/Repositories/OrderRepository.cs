using Microsoft.EntityFrameworkCore;
using PharmacyOrderingSystemp5.Data;
using PharmacyOrderingSystemp5.Models;
using PharmacyOrderingSystemp5.Repositories.Interfaces;

namespace PharmacyOrderingSystemp5.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUser(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<Order?> GetOrder(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}