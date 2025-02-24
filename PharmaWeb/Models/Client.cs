using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaWeb.Models
{
    [Table("Tb_Pharma_Client")]
    public class Client
    {
        public int ClientId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(11)]
        public string Cpf { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        [StringLength(11)]
        public string Cellphone { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        // 1 cliente tem varios pedidos
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
