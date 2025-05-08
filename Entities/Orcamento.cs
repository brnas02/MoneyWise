using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{
    public class Orcamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Limite { get; set; }

        [Required]
        public DateTime MesReferencia { get; set; }
    }
}
