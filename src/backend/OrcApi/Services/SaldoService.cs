using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public class SaldoService
    {
        private readonly Data.ISaldoRepo _repo;
        private readonly Data.ISaldoAnteriorRepo _repoSaldoAnterior;
        public SaldoService(Data.ISaldoRepo repo, Data.ISaldoAnteriorRepo repoSaldoAnterior)
        {
            this._repo = repo;
            this._repoSaldoAnterior = repoSaldoAnterior;
        }

        public async Task<IEnumerable<Saldo>> Search(SaldoParameters parameters)
        {
            return await _repo.Search(parameters);
        }

        public async Task Add(Saldo item)
        {
            await _repo.Add(item);
        }

        public async Task Update(Saldo item)
        {
            await _repo.Update(item);
        }

        internal async Task<SaldoAnterior> SearchSaldoAnterior(SaldoParameters parameters)
        {
            return (await _repoSaldoAnterior.Search(parameters)).SingleOrDefault();
        }

        public async Task AddSaldoAnterior(SaldoAnterior item)
        {
            await _repoSaldoAnterior.Add(item);
        }

        public async Task UpdateSaldoAnterior(SaldoAnterior item)
        {
            await _repoSaldoAnterior.Update(item);
        }
    }
}
