using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public interface IParcelaCartaoRepo
    {
        //Task<Lancamento> Single(Guid id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<List<ParcelaCartao>> All();
        Task<IEnumerable<ParcelaCartao>> Search(ParcelaCartaoParameters parameters);
        Task Add(ParcelaCartao item);
        //Task Remove(Guid id);
        Task Update(ParcelaCartao item);
    }
}
