using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public interface ICategoriaService
    {
        Task<Categoria> Get(string id);
        Task<IEnumerable<Categoria>> All();
        Task<IEnumerable<Categoria>> Search(CategoriaParameters parameters);
        Task Add(Categoria item);
        Task Delete(string id);
    }
}
