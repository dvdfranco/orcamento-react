using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly Services.ICategoriaService _service;

        public CategoriaController(Services.ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IEnumerable<Categoria>> All()
        {
            return (await _service.All()).OrderBy(x => x.Descricao == "Sem Categoria" ? -1 : 0).ThenBy(x => x.Descricao);
        }

        [HttpGet]
        public async Task<Categoria> Get(string id)
        {
            return await _service.Get(id);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IEnumerable<Categoria>> Search(CategoriaParameters parameters)
        {
            return await _service.Search(parameters);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Categoria item)
        {
            await _service.Add(item);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            await _service.Delete(Id);
            return Ok();
        }

    }
}
