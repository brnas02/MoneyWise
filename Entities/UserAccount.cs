using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{
    // Índices únicos para garantir que não existam emails ou nomes de utilizador duplicados
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {
        // Chave primária da conta de utilizador
        [Key]
        public int Id { get; set; }

        // Primeiro nome do utilizador 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string FirstName { get; set; }

        // Último nome ou apelido do utilizador 
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string LastName { get; set; }

        // Nome de utilizador para autenticação (obrigatório e único)
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        public string UserName { get; set; }

        // Endereço de email do utilizador (obrigatório e único)
        [Required(ErrorMessage = "Campo obrigatório.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Email { get; set; }

        // Referência para o tipo de conta (Pessoal, Empresarial, Admin)
        [Required(ErrorMessage = "Selecione o tipo de conta.")]
        public int RoleId { get; set; }

        // Caminho para a imagem de perfil do utilizador (opcional)
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string? ProfileImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        // Propriedades de navegação
        public Role Role { get; set; } = null!;

        public ICollection<UserTransacao> UserTransacoes { get; set; }

        public UserSeguranca UserSeguranca { get; set; }
    }
}
