using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public class LancamentoRepo : ILancamentoRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<Lancamento> _context;

        public LancamentoRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<Lancamento>(appSettings.DB_COLLECTION_LANCAMENTO);
        }


        public async Task<List<Lancamento>> All()
        {
            return (await _context.FindAsync(x => true)).ToList();
        }

        public async Task<Lancamento> Single(string id)
        {
            return await _context.Find(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Lancamento>> Search(LancamentoParameters parameters)
        {
            var query = _context.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Id))
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.Id == parameters.Id);
            else if (parameters.Ids != null && parameters.Ids.Any())
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => parameters.Ids.Contains(x.Id));
            }
            if (parameters.Transid.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => parameters.Transid == x.Transid);

            if (parameters.MesFrom.HasValue && parameters.MesTo.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.DataRef >= parameters.MesFrom.Value && x.DataRef <= parameters.MesTo.Value);
            else if (parameters.MesFrom.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.DataRef >= parameters.MesFrom.Value);
            else if (parameters.MesTo.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.DataRef <= parameters.MesTo.Value);

            if (parameters.Data.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.Data == parameters.Data.Value);

            if (!string.IsNullOrEmpty(parameters.BankMemo))
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.BankMemo == parameters.BankMemo);

            if (!string.IsNullOrEmpty(parameters.Memo))
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.Memo == parameters.Memo);

            if (parameters.Tipo.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.Tipo == parameters.Tipo.Value);

            if (parameters.Autocomplete.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.Autocomplete == parameters.Autocomplete.Value);

            if (parameters.SoDebitos.HasValue && parameters.SoDebitos.Value)
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.Valor <= 0);

            if (!string.IsNullOrEmpty(parameters.IdCategoria))
                query = (MongoDB.Driver.Linq.IMongoQueryable<Lancamento>)query.Where(x => x.IdCategoria == parameters.IdCategoria);

            return (await query.ToListAsync())
                .OrderBy(x => x.DataRef)
                .ThenBy(x => x.Data);

        }

        public async Task Add(Lancamento item)
        {
            await _context.InsertOneAsync(item);
        }

        public async Task Update(Lancamento item)
        {
            await _context.ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task Delete(string id)
        {
            await _context.FindOneAndDeleteAsync(x => x.Id == id);
        }
    }
}
