using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public interface ISaldoService
    {
        Task<IEnumerable<Saldo>> Search(SaldoParameters parameters);
        Task Add(Saldo item);
        Task Update(Saldo item);
        Task AddSaldoAnterior(SaldoAnterior item);
        Task UpdateSaldoAnterior(SaldoAnterior item);
        Task<SaldoAnterior> SearchSaldoAnterior(SaldoParameters parameters);
    }
}
