using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    // Modelo para o formulário de registo de novos utilizadores
    public class RegistrationViewModel
    {
        // Primeiro nome do utilizador 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string FirstName { get; set; }

        // Último nome do utilizador 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string LastName { get; set; }

        // Nome de utilizador para autenticação 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        public string UserName { get; set; }

        // Email do utilizador com validação de formato 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Coloque um email válido.")]
        public string Email { get; set; }

        // Palavra-passe com requisitos de comprimento
        [Required(ErrorMessage = "Campo obrigatório.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Entre 5 e 20 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Confirmação da palavra-passe
        [Required(ErrorMessage = "Campo obrigatório.")]
        [Compare("Password", ErrorMessage = "As palavras-passe não coincidem.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        // Tipo de conta de utilizador (Pessoal ou Empresarial)
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Role { get; set; } = "User";

        // Imagem de perfil do utilizador (opcional)
        public IFormFile? ProfileImage { get; set; }
    }
}