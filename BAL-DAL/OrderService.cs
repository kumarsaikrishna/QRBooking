using QRCodeBooking.Models.DTOs;
using QRCodeBooking.Models.Entitys;

namespace QRCodeBooking.BAL_DAL
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> PlaceManualOrder(int userId, ManualOrderDto manualOrderDto)
        {
            var order = new Order
            {
                UserId = userId,
                OrderStatus = "Pending",
                DeliveryPersonName = manualOrderDto.DeliveryPersonName,
                DeliveryPersonContact = manualOrderDto.DeliveryPersonContact,
                OTP = manualOrderDto.OTP,
                IsPaymentDone = manualOrderDto.IsPaymentDone,
                PaymentMethod = manualOrderDto.PaymentMethod,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            var orderId = await _orderRepository.CreateOrder(order);

            var manualOrder = new ManualOrder
            {
                OrderId = orderId,
                DeliveryPersonName = manualOrderDto.DeliveryPersonName,
                DeliveryPersonContact = manualOrderDto.DeliveryPersonContact,
                OTP = manualOrderDto.OTP,
                PaymentMethod = manualOrderDto.PaymentMethod,
                CreatedDate = DateTime.Now
            };

            await _orderRepository.AddManualOrder(manualOrder);

            return order;
        }

        public async Task<bool> VerifyOTP(int orderId, string otp)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order != null && order.OTP == otp)
            {
                order.OrderStatus = "Accepted";
                await _orderRepository.UpdateOrder(order);
                return true;
            }
            return false;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }
    }

}
