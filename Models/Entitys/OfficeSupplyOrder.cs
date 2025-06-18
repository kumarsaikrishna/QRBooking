namespace QRCodeBooking.Models.Entitys
{
    public class OfficeSupplyOrder
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
        public string RequestedBy { get; set; }
        public string requestedDesk { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsFulfilled { get; set; }
    }

}
