using Microsoft.AspNetCore.Mvc;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveController : ControllerBase
    {
        private readonly Services.ICategoriaService _s;
        private readonly Services.ISetupService _setup;
        public LiveController(Services.ICategoriaService s, Services.ISetupService setup)
        {
            _s = s;
            _setup = setup;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await _s.Add(new Categoria() { Id = new ObjectId(), GuidId = new Guid(), Descricao = "ABC" });

            return Ok(await _s.Search(new Orcamento.Models.Parameters.CategoriaParameters()));
            //return Ok("CORS BITCH! 1");
        }

        [HttpPost("SetupDatabase")]
        public async Task<IActionResult> SetupDatabaseFirstUse()
        {
            await _setup.SetupDatabase();

            return Ok("Database setup OK");
        }

    }
}
