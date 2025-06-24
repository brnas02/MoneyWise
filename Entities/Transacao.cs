using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class Transacao
    {
        // Chave primária da transação
        [Key]
        public int Id { get; set; }

        // Tipo de transação: "Receita" ou "Despesa"
        [Required]
        [MaxLength(20)]
        public string TipoTransacao { get; set; }

        // Categoria da transação (ex: Alimentação, Salário, Transporte)
        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; }

        // Valor da transação com precisão financeira (até 2 casas decimais)
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        // Descrição opcional da transação (ex: "Pagamento da renda")
        [MaxLength(255)]
        public string? Descricao { get; set; }

        // Data em que a transação ocorreu (obrigatória)
        [Required]
        public DateTime Data { get; set; }

        public ICollection<UserTransacao>? UserTransacoes { get; set; }
    }
}