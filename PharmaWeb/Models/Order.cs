using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaWeb.Models
{
    [Table("Tb_Pharma_Order")]
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal OrderTotal { get; set; }
        // 1  pedido tem 1 cliente
        public Client Client { get; set; }
        public int ClientId { get; set; }
        // muitos medicines pode ter muitas orders
        public List<OrderMedicine> OrdersMedicines { get; set; } = new List<OrderMedicine>();


    }
}
