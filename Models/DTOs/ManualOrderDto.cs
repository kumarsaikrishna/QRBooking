namespace QRCodeBooking.Models.DTOs
{
    public class ManualOrderDto
    {
        public string DeliveryPersonName { get; set; }
        public string DeliveryPersonContact { get; set; }
        public string OTP { get; set; }
        public bool IsPaymentDone { get; set; }
        public string PaymentMethod { get; set; }
    }
}
