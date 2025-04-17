using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    public class LancamentoPrevisto
    {
        public LancamentoPrevisto()
        {
            this.Id = Util.NewObjectId();
        }
        public LancamentoPrevisto (DateTime dataRef)
        {
            this.Id = Util.NewObjectId();
            if (dataRef > DateTime.MinValue)
            {
                this.Data = new DateTime(dataRef.Year, dataRef.Month, 1);
            }
            this.Categoria = new Categoria(Util.NewObjectId(Util.GuidSemCategoria));
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

        [Required]
        public string Memo { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdCategoria { get; set; }

        //[BsonIgnore]
        //public int NIdCategoria { get; set; }

        [BsonIgnore]
        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }

        [Column(TypeName = "datetime")]
        private DateTime _dataLimite = DateTime.MinValue;
        public DateTime DataLimite { get { return _dataLimite.ToLocalTime(); } set { _dataLimite = value.ToUniversalTime(); } }

        [NotMapped]
        [BsonIgnore]
        public int Parcelas { get; set; }
    }
}
