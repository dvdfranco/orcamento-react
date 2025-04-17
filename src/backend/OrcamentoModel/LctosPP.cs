using System.Collections.Generic;

namespace Orcamento.Models
{
    public class LctosPP
    {
        public string IdConta { get; set; }
        public Conta Conta { get; set; }
        public string IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
        public decimal Total { get; set; }
    }
}
