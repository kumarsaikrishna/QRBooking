namespace QRCodeBooking.Models.DTOs
{
    public class VerifyOTPDto
    {
        public int OrderId { get; set; }
        public string OTP { get; set; }
    }
}
