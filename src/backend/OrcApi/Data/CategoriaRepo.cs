using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public class CategoriaRepo : ICategoriaRepo
    {
        private readonly MongoClient _client;
        private readonly IMongoCollection<Categoria> _context;
        private readonly AppSettings _appSettings;

        public CategoriaRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _context = _client.GetDatabase(appSettings.DB_NAME).GetCollection<Categoria>(appSettings.DB_COLLECTION_CATEGORIA);
            _appSettings = appSettings;
        }

        public async Task<IEnumerable<Categoria>> Search(CategoriaParameters parameters)
        {
            var query = _context.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Id))
                query = (MongoDB.Driver.Linq.IMongoQueryable<Categoria>)query.Where(x => x.Id == parameters.Id);
            else if (parameters.Ids != null)
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<Categoria>)query.Where(x => parameters.Ids.Contains(x.Id));
            }
            else
                query = (MongoDB.Driver.Linq.IMongoQueryable<Categoria>)query.Where(x => parameters.Tipo == x.Tipo);

            if (parameters.MesFrom.HasValue && parameters.MesTo.HasValue)
                throw new NotImplementedException("Implementar CategoriaRepo.Search (MesFrom/MesTo)");
            //query = query.Where(x => x.Lancamentos.Any(y => y.DataRef >= parameters.MesFrom.Value && y.DataRef <= parameters.MesTo.Value));

            if (parameters.IncludeLancamentos)
                throw new NotImplementedException("Implementar CategoriaRepo.Search (IncludeLancamentos)");

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> All()
        {
            return (await _context.FindAsync(x => true)).ToList();

        }

        public async Task Add(Categoria item)
        {
            await _context.InsertOneAsync(item);

        }

        public async Task<Categoria> Single(string id)
        {
            return await _context.Find(x => x.Id == id).FirstOrDefaultAsync();

        }

        public async Task Delete(string id)
        {
            await _context.FindOneAndDeleteAsync(x => x.Id == id);
        }
    }
}
