using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public class LancamentoPrevistoRepo : ILancamentoPrevistoRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<LancamentoPrevisto> _context;

        public LancamentoPrevistoRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<LancamentoPrevisto>(appSettings.DB_COLLECTION_LANCAMENTO_PREVISTO);
        }


        public async Task<List<LancamentoPrevisto>> All()
        {
            return (await _context.FindAsync<LancamentoPrevisto>(x => true)).ToList();

        }

        public async Task Add(LancamentoPrevisto item)
        {
            await _context.InsertOneAsync(item);
        }

        public async Task Update(LancamentoPrevisto item)
        {
            await _context.ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task<IEnumerable<LancamentoPrevisto>> Search(LancamentoParameters parameters)
        {
            var query = _context.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Id))
                query = (MongoDB.Driver.Linq.IMongoQueryable<LancamentoPrevisto>)query.Where(x => x.Id == parameters.Id);

            if (parameters.MesFrom.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<LancamentoPrevisto>)query.Where(x => x.Data <= parameters.MesFrom.Value);

            if (parameters.MesTo.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<LancamentoPrevisto>)query.Where(x => x.DataLimite >= parameters.MesTo.Value);

            return query.OrderByDescending(x => x.Data).ToList();

        }
    }
}
