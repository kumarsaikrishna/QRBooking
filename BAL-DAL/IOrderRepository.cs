using QRCodeBooking.Models.Entitys;

namespace QRCodeBooking.BAL_DAL
{
    public interface IOrderRepository
    {
        Task<int> CreateOrder(Order order);
        Task AddManualOrder(ManualOrder manualOrder);
        Task<Order> GetOrderById(int orderId);
        Task UpdateOrder(Order order);
        Task<List<Order>> GetAllOrders();
    }
}
