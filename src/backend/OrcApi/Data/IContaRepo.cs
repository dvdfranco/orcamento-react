using Orcamento.Models;

namespace OrcamentoApi.Data
{
    public interface IContaRepo
    {
        Task<Conta> Single(string id);
        Task<IEnumerable<Conta>> All();
        //Task<IEnumerable<Conta>> Search(ContaParameters parameters);
        Task Add(Conta item);
        Task Delete(string id);
        //Task Update(Guid id, Lancamento entity);
    }
}
