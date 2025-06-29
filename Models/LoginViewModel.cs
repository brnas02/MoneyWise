using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    // Modelo para o formulário de login do utilizador
    public class LoginViewModel
    {
        // Campo para introdução do nome de utilizador ou email 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [DisplayName("Nome de utilizador ou Email")]
        public string UserNameOrEmail { get; set; }

        // Campo para a palavra-passe com validação de comprimento
        [Required(ErrorMessage = "Campo obrigatório.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Entre 5 e 20 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}