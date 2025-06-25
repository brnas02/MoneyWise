using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

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
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de conta.")]
        public int RoleId { get; set; }

        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string? ProfileImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public Role Role { get; set; } = null!;

        public ICollection<UserTransacao> UserTransacoes { get; set; }
    }
}
