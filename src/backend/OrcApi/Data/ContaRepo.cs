using MongoDB.Driver;
using Orcamento.Models;

namespace OrcamentoApi.Data
{
    public class ContaRepo : IContaRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<Conta> _context;

        public ContaRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<Conta>(appSettings.DB_COLLECTION_CONTA);
        }

        public async Task<IEnumerable<Conta>> All()
        {return (await _context.FindAsync(x => true)).ToList();
        }

        public async Task Add(Conta item)
        {
            await _context.InsertOneAsync(item);
        }

        public async Task Update(Conta item)
        {
            await _context.ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task<Conta> Single(string id)
        {
            return await _context.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Delete(string id)
        {
            await _context.FindOneAndDeleteAsync(x => x.Id == id);
        }
    }
}
