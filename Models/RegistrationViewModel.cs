using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Coloque um email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Entre 5 e 20 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Compare("Password", ErrorMessage = "As palavras-passe não coincidem.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Role { get; set; } = "User";

        public IFormFile? ProfileImage { get; set; }
    }
}