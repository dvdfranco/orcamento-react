using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public class ParcelaCartaoService
    {
        private readonly Data.IParcelaCartaoRepo _repo;
        public ParcelaCartaoService(Data.IParcelaCartaoRepo repo)
        {
            this._repo = repo;
        }

        public async Task<IEnumerable<ParcelaCartao>> All()
        {
            return await _repo.All();
        }

        public async Task<IEnumerable<ParcelaCartao>> Search(ParcelaCartaoParameters parameters)
        {
            return await _repo.Search(parameters);
        }

        public async Task Add(ParcelaCartao item)
        {
            await _repo.Add(item);
        }

        internal async Task Update(ParcelaCartao item)
        {
            await _repo.Update(item);
        }
    }
}
