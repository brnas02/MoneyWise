using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    public class RegistrationViewModel
    {

        // Nome próprio do utilizador (obrigatório, máximo 50 caracteres)
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string FirstName { get; set; }

        // Apelido do utilizador (obrigatório, máximo 50 caracteres)
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string LastName { get; set; }

        // Nome de utilizador (obrigatório, máximo 20 caracteres)
        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string UserName { get; set; }

        // Email do utilizador (obrigatório, máximo 100 caracteres)
        // Inclui expressão regular personalizada para validação
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        //[EmailAddress(ErrorMessage = "Please Enter Valid Email.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        // Palavra-passe (entre 5 e 20 caracteres)
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 characters allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Confirmação da palavra-passe (obrigatório)
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        // Tipo de conta (por padrão "User", mas pode ser "Admin" ou "Empresarial")
        public string Role { get; set; } = "User";

        // Upload de imagem de perfil (opcional)
        public IFormFile? ProfileImage { get; set; }
    }
}