using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orcamento.Models
{
    [Serializable]
    public class Lancamento
    {
        public Lancamento()
        {
            this.Id = Util.NewObjectId();
            this.DataInclusao = DateTime.Now;
        }
        public Lancamento (DateTime dataRef)
        {
            if (dataRef > DateTime.MinValue)
            {
                this.DataRef = dataRef;
                this.Data = new DateTime(dataRef.Year, dataRef.Month, 1);
            }
            //this.Categoria = new Categoria(1);
            this.Id = Util.NewObjectId();
            this.IdCategoria = Util.NewObjectId(Util.GuidSemCategoria);
            this.Autocomplete = true;
            this.Tipo = TipoConta.Corrente;
            this.DataInclusao = DateTime.Now;
        }

        public Lancamento(LancamentoPrevisto x, DateTime dataRef)
        {
            this.Id = Util.NewObjectId();
            this.IsPrevisto = true;
            this.IdLancamentoPrevisto = x.Id;
            this.BankMemo = this.Memo = x.Memo;
            this.IdCategoria = x.IdCategoria;
            this.Categoria = x.Categoria;
            this.Valor = x.Valor;
            this.ValorOriginal = x.Valor;
            this.Autocomplete = true;
            //this.Transid = 1; //temp pra usar na view Exportar

            this.Data = new DateTime(dataRef.Year, dataRef.Month, x.Data.Day);
            this.DataRef = dataRef;
            this.DataInclusao = DateTime.Now;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("Id")]
        //public string NId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdConta { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLancamentoRef { get; set; }

        public Int64 Transid { get; set; }
        
        [Column(TypeName = "decimal(7,2)")]
        public decimal Valor { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public decimal ValorOriginal { get; set; }
        
        [Column(TypeName = "datetime")]
        private DateTime _data = DateTime.MinValue;
        public DateTime Data { get { return _data.ToLocalTime(); } set { _data = value.ToUniversalTime(); } }

        [Column(TypeName = "datetime")]
        private DateTime _dataRef = DateTime.MinValue;
        public DateTime DataRef { get { return _dataRef.ToLocalTime(); } set { _dataRef = value.ToUniversalTime(); } }

        public bool Autocomplete { get; set; }

        [Column("tipo_lancamento")]
        public TipoConta Tipo { get; set; }

        [Required]
        public string Memo { get; set; }
        
        [Required]
        public string BankMemo { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdCategoria { get; set; }

        //[BsonIgnore]
        //public int NIdCategoria { get; set; }


        [BsonIgnore]
        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }

        [BsonIgnore]
        [ForeignKey("IdConta")]
        public Conta Conta { get; set; }

        [NotMapped]
        [BsonIgnore]
        public bool IsPrevisto { get; set; }

        [NotMapped]
        [BsonIgnore]
        public string IdLancamentoPrevisto { get; set; }

        //[NotMapped]
        //[BsonIgnore]
        //public int? NIdLancamentoPrevisto { get; set; }

        private DateTime _dataInclusao = DateTime.MinValue;
        public DateTime DataInclusao { get { return _dataInclusao.ToLocalTime(); } set { _dataInclusao = value.ToUniversalTime(); } }
    }

    public enum TipoConta
    {
        Corrente = 0,
        Poupanca = 1
    }
}
