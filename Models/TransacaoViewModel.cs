using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Models
{
    public class TransacaoViewModel
    {

        // Objeto que representa a nova transação a ser criada (ligado ao formulário)
        public Transacao NovaTransacao { get; set; }

        // Lista de transações existentes para o mês/ano selecionado (não validada no form)
        [ValidateNever]
        public List<Transacao> Historico { get; set; }

        // Mês atual selecionado (usado na view e filtragem)
        public int Mes { get; set; }

        // Ano atual selecionado (usado na view e filtragem)
        public int Ano { get; set; }
    }
}