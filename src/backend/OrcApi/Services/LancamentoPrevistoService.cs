using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public class LancamentoPrevistoService
    {
        private readonly Data.LancamentoPrevistoRepo _repo;
        private readonly Data.CategoriaRepo _catRepo;

        public LancamentoPrevistoService(Data.ILancamentoPrevistoRepo lspRepo, Data.ICategoriaRepo catRepo)
        {
            this._catRepo = (Data.CategoriaRepo)catRepo;
            this._repo = (Data.LancamentoPrevistoRepo)lspRepo;
        }

        public async Task<IEnumerable<LancamentoPrevisto>> All()
        {
            var lctos = await _repo.All();
            var categorias = await _catRepo.All();

            foreach (var lcto in lctos)
            {
                var cat = categorias.Where(x => x.Id == lcto.IdCategoria).SingleOrDefault();
                lcto.Categoria = cat;
            }

            return lctos;
        }

        public async Task Add(LancamentoPrevisto item)
        {
            await _repo.Add(item);
        }

        public async Task<IEnumerable<LancamentoPrevisto>> Search(LancamentoParameters parameters)
        {
            return await _repo.Search(parameters);
        }

    }
}
