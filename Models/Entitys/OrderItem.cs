namespace QRCodeBooking.Models.Entitys
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

}
