using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaWeb.Models
{
    [Table("Tb_Pharma_Medicine")]
    public class Medicine
    {
        public int MedicineId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        // relação com raw materials (N:N)
        public List<MedicineRawMaterial> Composition { get; set; } = new List<MedicineRawMaterial>();
        public List<OrderMedicine> OrdersMedicines { get; set; } = new List<OrderMedicine>();

    }
}
