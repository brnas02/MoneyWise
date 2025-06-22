using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class Orcamento
    {
        // Chave primária da tabela
        [Key]
        public int Id { get; set; }

        // ID do utilizador a que pertence este orçamento
        [Required]
        public int UserId { get; set; }

        // Categoria à qual o orçamento se aplica (ex: Alimentação, Transporte)
        [Required, MaxLength(50)]
        public string Categoria { get; set; }

        // Valor máximo permitido para gastar nessa categoria no mês
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal LimiteMensal { get; set; }

        // Total já gasto até o momento nesta categoria (inicia a 0)
        [Column(TypeName = "decimal(10,2)")]
        public decimal GastoAtual { get; set; } = 0;

        // Mês a que o orçamento se refere (1 a 12)
        [Required]
        public int Mes { get; set; }

        // Ano correspondente ao orçamento
        [Required]
        public int Ano { get; set; }
    }
}