using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{
    public class UserTransacao
    {
        // Identificador do utilizador (parte da chave composta)
        public int UserAccountId { get; set; }

        // Propriedade de navegação para o utilizador
        public UserAccount UserAccount { get; set; }

        // Identificador da transação (parte da chave composta)
        public int TransacaoId { get; set; }

        // Propriedade de navegação para a transação
        public Transacao Transacao { get; set; }
    }
}