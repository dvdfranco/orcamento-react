namespace Orcamento.Models
{
    public class AppSettings
    {
        public string ApiUri { get; set; }
        public string SemCategoria { get; set; }
        public string CategoriaAlimentacao { get; set; }
        public string CategoriaGordices { get; set; }
        public string CategoriaRoupas { get; set; }
        public string CategoriaCarro { get; set; }
        public string MongoUrl { get; set; }

        public string DB_NAME { get; set; }
        public string DB_COLLECTION_CATEGORIA { get; set; }
        public string DB_COLLECTION_LANCAMENTO { get; set; }
        public string DB_COLLECTION_LANCAMENTO_PREVISTO { get; set; }
        public string DB_COLLECTION_CONTA { get; set; }
        public string DB_COLLECTION_MEMO_PADRAO { get; set; }
        public string DB_COLLECTION_SALDO { get; set; }
        public string DB_COLLECTION_SALDO_ANTERIOR { get; set; }
        public string DB_COLLECTION_PARCELA_CARTAO { get; set; }
    }
}