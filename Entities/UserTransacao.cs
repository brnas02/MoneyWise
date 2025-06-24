using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities
{
    public class UserTransacao
    {
        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        public int TransacaoId { get; set; }
        public Transacao Transacao { get; set; }
    }
}