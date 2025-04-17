using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public interface ISaldoRepo
    {
        //Task<Lancamento> Single(Guid id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<List<Saldo>> All();
        //Task<IEnumerable<Lancamento>> Search(LancamentoParameters parameters);
        Task Add(Saldo item);
        //Task Remove(Guid id);
        Task Update(Saldo entity);
        Task<IEnumerable<Saldo>> Search(SaldoParameters parameters);
    }
}
