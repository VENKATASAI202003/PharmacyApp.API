using Microsoft.EntityFrameworkCore;
using PharmacyOrderingSystemp5.Data;
using PharmacyOrderingSystemp5.Enums;
using PharmacyOrderingSystemp5.Models;
using PharmacyOrderingSystemp5.Repositories.Interfaces;
using PharmacyOrderingSystemp5.Services.Interfaces;

namespace PharmacyOrderingSystemp5.Services.Implementations
{
    // Service handles business logic for orders
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IOrderRepository _orderRepo;

        public OrderService(AppDbContext context, IOrderRepository orderRepo)
        {
            _context = context;
            _orderRepo = orderRepo;
        }

        // FUNCTION 1: Reduce medicine stock when prescription is approved
        public async Task<bool> ReduceStock(int prescriptionId)
        {
            // Step 1: Find the prescription by ID
            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(p => p.Id == prescriptionId);

            if (prescription == null)
                return false; // Prescription not found

            // Step 2: Find the order linked to this prescription
            var order = await _context.Orders
                .Include(o => o.Items) // Include cart items
                .FirstOrDefaultAsync(o =>
                    o.UserId == prescription.UserId &&
                    o.Status == OrderStatus.PendingApproval);

            if (order == null)
                return false; // Order not found

            // Step 3: For each medicine in the order, reduce stock
            foreach (var item in order.Items)
            {
                var medicine = await _context.Medicines
                    .FirstOrDefaultAsync(m => m.Id == item.MedicineId);

                if (medicine == null)
                    continue; // Skip if medicine not found

                // Check if we have enough stock
                if (medicine.Stock < item.Quantity)
                    return false; // Not enough stock

                // Reduce the stock
                medicine.Stock -= item.Quantity;
            }

            // Step 4: Update order status to Approved
            order.Status = OrderStatus.Approved;

            // Step 5: Save all changes to database
            await _context.SaveChangesAsync();
            return true; // Success
        }

        // FUNCTION 2: Get all orders for a specific user
        public async Task<List<Order>> GetOrderHistory(int userId)
        {
            // Find all orders for this user and include their medicines
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items) // Include medicines in order
                .OrderByDescending(o => o.Id) // Show newest first
                .ToListAsync();

            return orders;
        }

        // FUNCTION 3: Create a new order by copying items from an old order
        public async Task<Order> Reorder(int oldOrderId, int userId)
        {
            // Step 1: Get the old order with its items
            var oldOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == oldOrderId && o.UserId == userId);

            if (oldOrder == null)
                return null; // Old order not found

            // Step 2: Create a new order
            var newOrder = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalAmount = oldOrder.TotalAmount,
                Items = new List<CartItem>()
            };

            // Step 3: Copy all items from old order to new order
            foreach (var oldItem in oldOrder.Items)
            {
                var newItem = new CartItem
                {
                    MedicineId = oldItem.MedicineId,
                    Quantity = oldItem.Quantity
                };
                newOrder.Items.Add(newItem);
            }

            // Step 4: Save the new order to database
            await _orderRepo.AddOrder(newOrder);
            await _orderRepo.Save();

            return newOrder; // Return the newly created order
        }
    }
}