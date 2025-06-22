using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class Meta
    {
        // Chave primária da tabela
        [Key]
        public int Id { get; set; }

        // Relaciona a meta ao utilizador
        [Required]
        public int UserId { get; set; }

        // Nome da meta (obrigatório e com limite de 100 caracteres)
        [Required, MaxLength(100)]
        public string Nome { get; set; }

        // Valor que o utilizador pretende alcançar
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal ValorAlvo { get; set; }

        // Valor atual acumulado da meta (começa com 0 por defeito)
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorAtual { get; set; } = 0;

        // Data limite para atingir a meta (obrigatória)
        [Required]
        public DateTime DataLimite { get; set; }

        // Descrição opcional da meta (até 255 caracteres)
        [MaxLength(255)]
        public string? Descricao { get; set; }
    }
}