using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public interface ILancamentoService
    {
        Task<Lancamento> Get(string Id);
        Task<List<Lancamento>> All();
        Task<IEnumerable<Lancamento>> Search(LancamentoParameters parameters);
        Task<IEnumerable<LancamentoSimples>> SearchCustomMemos(Boolean? autoComplete);
        Task Add(Lancamento item);
        Task Update(Lancamento item);
        Task UpdateAutoComplete(string memo, bool autocomplete);
        Task Delete(string id);
        Task<List<Lancamento>> Dividir(Lancamento[] newItems);
        Task<object> GetEstatisticas(string strMesFrom);
        Task<object> GetCalcSaldos(DateTime mesFrom, DateTime mesTo);
        Task Upload(string ofxContent);
    }
}