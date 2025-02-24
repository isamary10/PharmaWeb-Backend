using System.ComponentModel.DataAnnotations;

namespace PharmaWeb.Dto
{
    public class OrderCreateDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public int ClientId { get; set; }
        public List<OrderMedicineDto> OrdersMedicines { get; set; }
    }
}
