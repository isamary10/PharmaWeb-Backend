using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaWeb.Models
{
    [Table("Tb_Pharma_OrderMedicine")]
    public class OrderMedicine
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
    }
}
