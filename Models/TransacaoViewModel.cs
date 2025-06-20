using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    public class TransacaoViewModel
    {
        public Transacao NovaTransacao { get; set; }

        [ValidateNever]
        public List<Transacao> Historico { get; set; }
    }
}
