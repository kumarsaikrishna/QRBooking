namespace QRCodeBooking.Models.Entitys
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryPersonName { get; set; }
        public string DeliveryPersonContact { get; set; }
        public string OTP { get; set; }
        public bool IsPaymentDone { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
