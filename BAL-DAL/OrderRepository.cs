using QRCodeBooking.Models.DB;
using QRCodeBooking.Models.Entitys;
using Microsoft.EntityFrameworkCore;


namespace QRCodeBooking.BAL_DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.OrderId;
        }

        public async Task AddManualOrder(ManualOrder manualOrder)
        {
            _context.ManualOrders.Add(manualOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
