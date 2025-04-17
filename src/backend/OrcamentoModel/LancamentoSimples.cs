using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    [Serializable]
    public class LancamentoSimples
    {
        public bool Autocomplete { get; set; }
        public string Memo { get; set; }
        public string IdCategoria { get; set; }
        public Categoria Categoria { get; set; }
        public bool IsPrevisto { get; set; }
    }
}
