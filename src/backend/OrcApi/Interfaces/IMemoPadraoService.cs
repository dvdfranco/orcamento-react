using Orcamento.Models;

namespace OrcamentoApi.Services
{
    public interface IMemoPadraoService
    {
        Task<IEnumerable<MemoPadrao>> All();
        Task Add(MemoPadrao item);
    }
}
