using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Orcamento.Models
{
    public class MemoPadrao
    {
        public MemoPadrao()
        {
            this.Id = Util.NewObjectId();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("Id")]
        //public string NId { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        public string Tipo { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdCategoria { get; set; }

        //[BsonIgnore]
        //public int NIdCategoria { get; set; }
    }
}
