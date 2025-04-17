using MongoDB.Driver;
using Orcamento.Models;

namespace OrcamentoApi.Data
{
    public class MemoPadraoRepo : IMemoPadraoRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<MemoPadrao> _context;

        public MemoPadraoRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<MemoPadrao>(appSettings.DB_COLLECTION_MEMO_PADRAO);
        }

        public async Task Update(MemoPadrao item)
        {
            await _context.ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public async Task<List<MemoPadrao>> All()
        {
            return (await _context.FindAsync(x => true)).ToList();
        }

        public async Task Add(MemoPadrao item)
        {
            await _context.InsertOneAsync(item);
        }
    }
}
