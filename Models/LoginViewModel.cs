using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    public class LoginViewModel
    {

        // Campo obrigatório para login — pode ser Username ou Email
        [Required(ErrorMessage = "Username or Email is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        [DisplayName("Username or Email")] // Nome mostrado na interface
        public string UserNameOrEmail { get; set; }

        // Palavra-passe obrigatória com validação de tamanho
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 characters allowed.")]
        [DataType(DataType.Password)] // Renderiza como campo de password (oculta os caracteres)
        public string Password { get; set; }
    }
}