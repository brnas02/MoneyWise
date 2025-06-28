using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class UserSeguranca
    {
        [Key, ForeignKey("UserAccount")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string Password { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public UserAccount UserAccount { get; set; }
    }
}
