using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orcamento.Models.Parameters
{
    public class CategoriaParameters
    {
        public CategoriaParameters()
        {
            this.Tipo = TipoConta.Corrente;
        }

        public string Id { get; set; }
        public string[] Ids { get; set; }
        public bool IncludeLancamentos { get; set; }
        public DateTime? MesFrom { get; set; }
        public DateTime? MesTo { get; set; }
        public TipoConta Tipo { get; set; }
    }
}
