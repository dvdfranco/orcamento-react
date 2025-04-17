using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public class SaldoRepo : ISaldoRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<Saldo> _context;

        public SaldoRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<Saldo>(appSettings.DB_COLLECTION_SALDO);
        }

        public async Task<IEnumerable<Saldo>> Search(SaldoParameters parameters)
        {
            var query = _context.AsQueryable<Saldo>();

            if (parameters.Mes.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Saldo>)query.Where(x => x.Data == parameters.Mes.Value);
            else if (parameters.MesTo.HasValue && parameters.MesFrom.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Saldo>)query.Where(x => x.Data >= parameters.MesFrom.Value && x.Data <= parameters.MesTo.Value);
            else if (parameters.MesTo.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Saldo>)query.Where(x => x.Data <= parameters.MesTo.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Saldo>> All()
        {
            return (await _context.FindAsync(x => true)).ToList();
        }

        public async Task Add(Saldo item)
        {
            await _context.InsertOneAsync(item);
        }

        public async Task Update(Saldo item)
        {
            await _context.ReplaceOneAsync(x => x.Id == item.Id, item);
        }
    }
}
