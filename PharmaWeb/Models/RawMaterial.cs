using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaWeb.Models
{
    [Table("Tb_Pharma_RawMaterial")]
    public class RawMaterial
    {
        public int RawMaterialId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [StringLength(200)]
        public string Supplier { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

        // relação com RawMaterial e Medicine (N:N)
        public List<MedicineRawMaterial> Composition { get; set; } = new List<MedicineRawMaterial>();

    }
}
