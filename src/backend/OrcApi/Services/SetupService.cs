using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Services
{
    public class SetupService
    {
        private readonly Data.ISetupRepo _repo;
        public SetupService(Data.ISetupRepo repo)
        {
            this._repo = repo;
        }

        public async Task SetupDatabase()
        {
            await _repo.SetupDatabaseFirstUse();
        }
    }
}
