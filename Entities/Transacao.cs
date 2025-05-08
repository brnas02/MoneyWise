using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities {
    public class Transacao {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string TipoTransacao { get; set; }

        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [MaxLength(255)]
        public string? Descricao { get; set; }

        [Required]
        public DateTime Data { get; set; }
    }
}
