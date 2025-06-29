using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjetoFinal.Entities
{
    public class UserSeguranca
    {
        // Chave primária e chave estrangeira que referencia UserAccount
        [Key, ForeignKey("UserAccount")]
        public int UserId { get; set; }

        // Password encriptada do utilizador 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string Password { get; set; }

        // Data de criação da conta de segurança (valor predefinido: data atual)
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Propriedade de navegação para a conta do utilizador associada
        public UserAccount UserAccount { get; set; }
    }
}
