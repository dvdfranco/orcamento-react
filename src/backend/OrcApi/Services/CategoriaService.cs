using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public class CategoriaService: ICategoriaService
    {
        private readonly Data.CategoriaRepo _repo;
        public CategoriaService(Data.ICategoriaRepo repo)
        {
            this._repo = (Data.CategoriaRepo)repo;
        }

        public async Task<Categoria> Get(string id)
        {
            return await _repo.Single(id);
        }

        public async Task<IEnumerable<Categoria>> All()
        {
            return await _repo.All();
        }

        public async Task<IEnumerable<Categoria>> Search(CategoriaParameters parameters)
        {
            return await _repo.Search(parameters);
        }

        public async Task Add(Categoria item)
        {
            await _repo.Add(item);
        }

        public async Task Delete(string id)
        {
            await _repo.Delete(id);
        }
    }
}
