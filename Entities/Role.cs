using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{

    // Índices únicos para garantir que não haja roles duplicadas
    [Index(nameof(Type), IsUnique = true)]
    public class Role
    {

        // Identificador único da role (chave primária)
        [Key]
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        public string Type { get; set; }
    }
}