using QRCodeBooking.Models.DTOs;
using QRCodeBooking.Models.Entitys;

namespace QRCodeBooking.BAL_DAL
{
    public interface IOrderService
    {
        Task<Order> PlaceManualOrder(int userId, ManualOrderDto manualOrderDto);
        Task<bool> VerifyOTP(int orderId, string otp);
        Task<List<Order>> GetAllOrders();
    }
}
