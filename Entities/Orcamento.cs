using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class Orcamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string Categoria { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal LimiteMensal { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal GastoAtual { get; set; } = 0;

        [Required]
        public int Mes { get; set; }

        [Required]
        public int Ano { get; set; }
    }
}