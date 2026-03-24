using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingSystemp5.Services.Interfaces;

namespace PharmacyOrderingSystemp5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ENDPOINT 1: Get all orders for the logged-in user
        // GET /api/orders/user 
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            try
            {
                var orders = await _orderService.GetOrderHistory(userId);
                return Ok(new
                {
                    success = true,
                    data = orders,
                    message = "Orders retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // ENDPOINT 2: Reorder - create new order from old order
        // POST /api/orders/reorder/{oldOrderId}
        [HttpPost("reorder/{oldOrderId}")]
        public async Task<IActionResult> Reorder(int oldOrderId, int userId)
        {
            try
            {
                var newOrder = await _orderService.Reorder(oldOrderId, userId);
                if (newOrder == null)
                    return NotFound(new
                    {
                        success = false,
                        message = "Old order not found"
                    });

                return Ok(new
                {
                    success = true,
                    data = newOrder,
                    message = "Order reordered successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // ENDPOINT 3: Reduce stock when prescription is approved
        // PUT /api/orders/reduce-stock/{prescriptionId}
        [HttpPut("reduce-stock/{prescriptionId}")]
        public async Task<IActionResult> ReduceStock(int prescriptionId)
        {
            try
            {
                bool success = await _orderService.ReduceStock(prescriptionId);
                if (!success)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Stock reduction failed"
                    });

                return Ok(new
                {
                    success = true,
                    message = "Stock reduced successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}