using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Orcamento.Models
{
    public class Categoria
    {
        public Categoria() { 
            this.Id = Util.NewObjectId();
        }
        
        public Categoria(string id)
        {
            this.Id = id;
            if (this.Id == Util.NewObjectId(Util.GuidSemCategoria))
            {
                this.Icone = "fa-dollar-sign";
                this.Descricao = "Sem Categoria";
            }
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("Id")]
        //public string NId { get; set; }

        public string Descricao { get; set; }
        
        public string Icone { get; set; }
        
        public string Cor { get; set; }

        public TipoConta Tipo { get; set; }

        [BsonIgnore]
        public ICollection<Lancamento> Lancamentos { get; set; }

        [BsonIgnore]
        public ICollection<LancamentoPrevisto> LancamentosPrevistos { get; set; }
    }
}
