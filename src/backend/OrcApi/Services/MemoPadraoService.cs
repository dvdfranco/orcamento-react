using Orcamento.Models;

namespace OrcamentoApi.Services
{
    public class MemoPadraoService: IMemoPadraoService
    {
        private readonly Data.IMemoPadraoRepo _repo;
        public MemoPadraoService(Data.IMemoPadraoRepo repo)
        {
            this._repo = repo;
        }

        public async Task<IEnumerable<MemoPadrao>> All()
        {
            return await _repo.All();
        }

        public async Task Add(MemoPadrao item)
        {
            await _repo.Add(item);
        }
    }
}
