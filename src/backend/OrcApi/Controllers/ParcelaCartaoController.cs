using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcelaCartaoController : ControllerBase
    {
        private readonly Services.IParcelaCartaoService _service;

        public ParcelaCartaoController(Services.IParcelaCartaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<ParcelaCartao>> Get()
        {
            return (await _service.All());
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IEnumerable<ParcelaCartao>> Search(ParcelaCartaoParameters parameters)
        {
            return await _service.Search(parameters);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ParcelaCartao item)
        {
            await _service.Add(item);
            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Put(ParcelaCartao item)
        {
            await _service.Update(item);
            return Ok();
        }
    }
}
