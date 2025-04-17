using System;

namespace Orcamento.Models.Parameters
{
    public class SaldoParameters
    {
        public SaldoParameters()
        {
        }

        public DateTime? MesFrom { get; set; }
        public DateTime? MesTo { get; set; }
        public DateTime? Mes { get; set; }
    }
}
