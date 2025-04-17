using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orcamento.Models.Parameters
{
    public class LancamentoParameters
    {
        public LancamentoParameters()
        {
        }

        public string Id { get; set; }
        public string[] Ids { get; set; }

        public string BankMemo { get; set; }
        public string Memo { get; set; }

        public DateTime? Data { get; set; }
        public DateTime? MesFrom { get; set; }
        public DateTime? MesTo { get; set; }

        public TipoConta? Tipo { get; set; }

        public bool IncludePrevistos { get; set; }
        public bool? Autocomplete { get; set; }
        public bool? MemoPersonalizado { get; set; }

        public bool? SoDebitos { get; set; }
        public string IdCategoria { get; set; }
        public long? Transid { get; set; }
    }
}
