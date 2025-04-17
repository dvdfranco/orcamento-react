using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public class ParcelaCartaoRepo : IParcelaCartaoRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<ParcelaCartao> _context;

        public ParcelaCartaoRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<ParcelaCartao>(appSettings.DB_COLLECTION_PARCELA_CARTAO);
        }


        public async Task<List<ParcelaCartao>> All()
        {
            return (await _context.FindAsync(x => true)).ToList();
        }

        public async Task<ParcelaCartao> Get(string id)
        {
            return (await _context.FindAsync(x => x.Id == id)).FirstOrDefault();
        }


        public async Task<IEnumerable<ParcelaCartao>> Search(ParcelaCartaoParameters parameters)
        {
            var query = _context.AsQueryable<ParcelaCartao>();

            if (!string.IsNullOrEmpty(parameters.Id))
                query = (MongoDB.Driver.Linq.IMongoQueryable<ParcelaCartao>)query.Where(x => x.Id == parameters.Id);

            if (parameters.Mes.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<ParcelaCartao>)query.Where(x => x.DataInicio <= parameters.Mes.Value && x.DataTermino >= parameters.Mes.Value);

            if (parameters.MesFrom.HasValue)
                query = (MongoDB.Driver.Linq.IMongoQueryable<ParcelaCartao>)query.Where(x => x.DataInicio >= parameters.MesFrom.Value);

            return await query.ToListAsync();
        }


        public async Task Add(ParcelaCartao item)
        {
            await _context.InsertOneAsync(item);
        }

        public async Task Update(ParcelaCartao item)
        {
            await _context.FindOneAndReplaceAsync(x => x.Id == item.Id, item);
        }
    }
}
