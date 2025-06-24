using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{

    // Índices únicos para garantir que não haja emails ou usernames duplicados
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {

        // Identificador único do utilizador (chave primária)
        [Key]
        public int Id { get; set; }

        // Nome próprio do utilizador (obrigatório, até 50 caracteres)
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string FirstName { get; set; }

        // Apelido do utilizador (obrigatório, até 50 caracteres)
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string LastName { get; set; }

        // Nome de utilizador (obrigatório, único, até 20 caracteres)
        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string UserName { get; set; }

        // Email do utilizador (obrigatório, único, até 100 caracteres)
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        public string Email { get; set; }

        // Palavra-passe (obrigatória, até 20 caracteres)
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string Password { get; set; }

        // Função do utilizador (ex: Admin, Empresarial, Utilizador normal)
        [Required(ErrorMessage = "Selecione o tipo de conta.")]
        public int RoleId { get; set; }

        // Caminho para imagem de perfil (opcional, até 255 caracteres)
        [MaxLength(255, ErrorMessage = "Max 255 characters allowed.")]
        public string? ProfileImagePath { get; set; }

        // Indica se a conta está ativa ou desativada (por padrão, ativa)
        public bool IsActive { get; set; } = true;

        public Role Role { get; set; } = null!;

        public ICollection<UserTransacao> UserTransacoes { get; set; }
    }
}