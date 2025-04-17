using Orcamento.Models;

namespace OrcamentoApi.Data
{
    public interface IMemoPadraoRepo
    {
        //Task<Lancamento> Single(Guid id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<List<MemoPadrao>> All();
        //Task<IEnumerable<Lancamento>> Search(LancamentoParameters parameters);
        Task Add(MemoPadrao item);
        //Task Remove(Guid id);
        Task Update(MemoPadrao item);
    }
}
