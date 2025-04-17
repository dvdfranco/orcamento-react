using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    public class Conta
    {
        public Conta() {
            this.Id = Util.NewObjectId();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("Id")]
        //public string NId { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }
        
        [BsonElement("Ordem")]
        public int Ordem { get; set; }

        [BsonIgnore]
        public ICollection<Lancamento> Lancamentos { get; set; }
    }
}