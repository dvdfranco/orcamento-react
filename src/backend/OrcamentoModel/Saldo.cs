using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    public class Saldo
    {
        public Saldo()
        {
            this.Id = Util.NewObjectId();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("Id")]
        //public string NId { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public decimal Valor { get; set; }

        [Column(TypeName = "datetime")]
        private DateTime _data = DateTime.MinValue;
        public DateTime Data { get { return _data.ToLocalTime(); } set { _data = value.ToUniversalTime(); } }
    }
}
