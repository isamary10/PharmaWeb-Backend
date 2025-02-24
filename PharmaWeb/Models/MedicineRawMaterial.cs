using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaWeb.Models
{
    [Table("Tb_Pharma_MedicineRawMaterial")]
    public class MedicineRawMaterial
    {
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int RawMaterialId { get; set; }
        public RawMaterial RawMaterial { get; set; }
    }
}
