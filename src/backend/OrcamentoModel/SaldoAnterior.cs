using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    [Table("dvd_SaldoAnterior")]
    public class SaldoAnterior
    {
        public SaldoAnterior()
        {
            this.Id = Util.NewObjectId();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Column("vlr_saldo", TypeName = "decimal(7,2)")]
        public decimal Valor { get; set; }

        [Column("data_saldo", TypeName = "datetime")]
        private DateTime _data = DateTime.MinValue;
        public DateTime Data { get { return _data.ToLocalTime(); } set { _data = value.ToUniversalTime(); } }
    }
}
