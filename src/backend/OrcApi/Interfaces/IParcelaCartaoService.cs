using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public interface IParcelaCartaoService
    {
        Task<IEnumerable<ParcelaCartao>> All();
        Task<IEnumerable<ParcelaCartao>> Search(ParcelaCartaoParameters parameters);
        Task Add(ParcelaCartao item);
        Task Update(ParcelaCartao item);
    }
}
