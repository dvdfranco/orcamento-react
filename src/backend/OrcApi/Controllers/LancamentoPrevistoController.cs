using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LancamentoPrevistoController : ControllerBase
    {
        private readonly Services.LancamentoPrevistoService _service;

        public LancamentoPrevistoController(Services.LancamentoPrevistoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<LancamentoPrevisto>> Get()
        {
            return await _service.All();
        }

        [HttpPost]
        public async Task<IActionResult> Post(LancamentoPrevisto item)
        {
            await _service.Add(item);
            return Ok();
        }
    }
}
