using Orcamento.Models;

namespace OrcamentoApi.Data
{
    public interface ILancamentoPrevistoRepo
    {
        //Task<Lancamento> Single(Guid id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<List<LancamentoPrevisto>> All();
        //Task<IEnumerable<Lancamento>> Search(LancamentoPrevistoParameters parameters);
        Task Add(LancamentoPrevisto item);
        //Task Remove(Guid id);
        Task Update(LancamentoPrevisto item);
    }
}
