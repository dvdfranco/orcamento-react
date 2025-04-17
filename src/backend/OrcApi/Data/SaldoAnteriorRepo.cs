using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public class SaldoAnteriorRepo : ISaldoAnteriorRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<SaldoAnterior> _context;

        public SaldoAnteriorRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<SaldoAnterior>(appSettings.DB_COLLECTION_SALDO_ANTERIOR);
        }


        public async Task<IEnumerable<SaldoAnterior>> Search(SaldoParameters parameters)
        {
            var query = _context.AsQueryable<SaldoAnterior>();

            if (parameters.Mes.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<SaldoAnterior>)query.Where(x => x.Data == parameters.Mes.Value);
            else if (parameters.MesTo.HasValue && parameters.MesFrom.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<SaldoAnterior>)query.Where(x => x.Data >= parameters.MesFrom.Value && x.Data <= parameters.MesTo.Value);
            else if (parameters.MesTo.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<SaldoAnterior>)query.Where(x => x.Data <= parameters.MesTo.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<SaldoAnterior>> All()
        {
            return (await _context.FindAsync(x => true)).ToList();
        }

        public async Task Add(SaldoAnterior item)
        {
            await _context.InsertOneAsync(item);
        }

        public async Task Update(SaldoAnterior item)
        {
            await _context.ReplaceOneAsync(x => x.Id == item.Id, item);
        }
    }
}
