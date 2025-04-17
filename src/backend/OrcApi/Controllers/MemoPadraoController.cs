using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemoPadraoController : ControllerBase
    {
        private readonly Services.MemoPadraoService _service;

        public MemoPadraoController(Services.MemoPadraoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<MemoPadrao>> Get()
        {
            return (await _service.All());
        }

        [HttpPost]
        public async Task<IActionResult> Post(MemoPadrao item)
        {
            await _service.Add(item);
            return Ok();
        }

    }
}
