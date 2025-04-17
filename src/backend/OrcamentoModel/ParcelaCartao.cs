using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    public class ParcelaCartao
    {
        public ParcelaCartao()
        {
            this.Id = Util.NewObjectId();
            this.IdCartao = 2705;
            this.DataInicio = DateTime.Today;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("Id")]
        //public string NId { get; set; }

        [Required]
        [Column("id_cartao")]
        public Int16 IdCartao { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        private DateTime _dataCompra = DateTime.MinValue;
        public DateTime DataCompra { get { return _dataCompra.ToLocalTime(); } set { _dataCompra = value.ToUniversalTime(); } }

        [Required]
        [Column(TypeName = "datetime")]
        private DateTime _dataInicio = DateTime.MinValue;
        public DateTime DataInicio { get { return _dataInicio.ToLocalTime(); } set { _dataInicio = value.ToUniversalTime(); } }

        [Required]
        [Column(TypeName = "datetime")]
        private DateTime _dataTermino = DateTime.MinValue;
        public DateTime DataTermino { get { return _dataTermino.ToLocalTime(); } set { _dataTermino = value.ToUniversalTime(); } }

        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Valor { get; set; }

        [Required]
        public string Descricao { get; set; }

        public string DescricaoCartao { get; set; }

        [NotMapped]
        [BsonIgnore]
        public int QtdParcelas { get; set; }

        [NotMapped]
        [BsonIgnore]
        public int ParcelaAtual { get; set; }

        [NotMapped]
        [BsonIgnore]
        public string DescParcela { get { return string.Format("{0}/{1}", ParcelaAtual, QtdParcelas); } }
    }
}
