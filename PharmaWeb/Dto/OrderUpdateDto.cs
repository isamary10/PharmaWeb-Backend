namespace PharmaWeb.Dto
{
    public class OrderUpdateDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public int ClientId { get; set; }
        public List<OrderMedicineDto> OrdersMedicines { get; set; }
    }
}
