using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orcamento.Models.Parameters
{
    public class ParcelaCartaoParameters
    {
        public ParcelaCartaoParameters()
        {
        }

        public string Id { get; set; }
        public DateTime? Mes { get; set; }
        public DateTime? MesFrom { get; set; }
    }
}
