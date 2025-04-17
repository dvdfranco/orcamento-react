using Microsoft.AspNetCore.Mvc;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveController : ControllerBase
    {
        private readonly Services.CategoriaService _s;
        private readonly Services.SetupService _setup;
        public LiveController(Services.CategoriaService s, Services.SetupService setup)
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

        [HttpPost]
        public async Task<IActionResult> SetupDatabaseFirstUse()
        {
            await _setup.SetupDatabase();

            return Ok("Database setup OK");
        }

    }
}
