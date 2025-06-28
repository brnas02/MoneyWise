using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [DisplayName("Nome de utilizador ou Email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Entre 5 e 20 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}