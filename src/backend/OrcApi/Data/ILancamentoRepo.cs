using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public interface ILancamentoRepo
    {
        Task<Lancamento> Single(string id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<List<Lancamento>> All();
        Task<IEnumerable<Lancamento>> Search(LancamentoParameters parameters);
        Task Add(Lancamento item);
        Task Delete(string id);
        Task Update(Lancamento item);
    }
}
