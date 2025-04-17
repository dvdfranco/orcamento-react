using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public interface ISaldoAnteriorRepo
    {
        //Task<Lancamento> Single(Guid id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<IEnumerable<SaldoAnterior>> All();
        Task<IEnumerable<SaldoAnterior>> Search(SaldoParameters parameters);
        Task Add(SaldoAnterior item);
        //Task Remove(Guid id);
        Task Update(SaldoAnterior item);
    }
}
