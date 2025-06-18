namespace QRCodeBooking.Models.Entitys
{
    public class ManualOrder
    {
        public int ManualOrderId { get; set; }
        public int OrderId { get; set; }
        public string DeliveryPersonName { get; set; }
        public string DeliveryPersonContact { get; set; }
        public string OTP { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
