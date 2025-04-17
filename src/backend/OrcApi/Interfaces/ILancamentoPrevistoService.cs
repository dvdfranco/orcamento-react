using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public interface ILancamentoPrevistoService
    {
        Task<IEnumerable<LancamentoPrevisto>> All();
        Task Add(LancamentoPrevisto item);
        Task<IEnumerable<LancamentoPrevisto>> Search(LancamentoParameters parameters);
    }
}
