using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class Meta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal ValorAlvo { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorAtual { get; set; } = 0;

        [Required]
        public DateTime DataLimite { get; set; }

        [MaxLength(255)]
        public string? Descricao { get; set; }
    }
}