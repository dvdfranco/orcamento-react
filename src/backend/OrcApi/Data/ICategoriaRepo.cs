using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Data
{
    public interface ICategoriaRepo
    {
        Task<Categoria> Single(string id);
        //Task<DvdspViewModel> All(string paginationToken = "");
        Task<IEnumerable<Categoria>> All();
        Task<IEnumerable<Categoria>> Search(CategoriaParameters parameters);
        Task Add(Categoria item);
        Task Delete(string id);
        //Task Update(Guid id, Lancamento entity);
    }
}
