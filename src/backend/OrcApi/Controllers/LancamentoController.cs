using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LancamentoController : ControllerBase
    {
        private readonly Services.LancamentoService _service;

        public LancamentoController(Services.LancamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<Lancamento> Get(string id)
        {
            return await _service.Get(id);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IEnumerable<Lancamento>> All()
        {
            return await _service.All();
        }

        [HttpPost]
        [Route("Search")]
        public async Task<List<Lancamento>> Search(LancamentoParameters parameters)
        {
            var ls = await _service.Search(parameters);

            return ls.OrderBy(x => x.Data).ToList();
        }

        [HttpGet]
        [Route("SearchCustomMemos")]
        public async Task<List<LancamentoSimples>> SearchCustomMemos(Boolean? autoComplete)
        {
            var ls = await _service.SearchCustomMemos(autoComplete);
            return ls.OrderBy(x => x.Memo).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Lancamento item)
        {
            await _service.Add(item);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Lancamento item)
        {
            await _service.Update(item);
            return Ok();
        }

        [HttpPut("PutAutocomplete")]
        public async Task<IActionResult> PutAutocomplete(Lancamento item)
        {
            await _service.UpdateAutoComplete(item.Memo, item.Autocomplete);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            await _service.Delete(Id);
            return Ok();
        }

        [HttpPut("Dividir")]
        public async Task<IActionResult> Dividir([FromBody] Lancamento[] newItems)
        {
            var lctosDivididos = await _service.Dividir(newItems);
            return Ok(lctosDivididos);
        }

        [HttpGet("CalcSaldos/{strMesFrom}")]
        public async Task<object> GetCalcSaldos(string strMesFrom)
        {
            var mesFrom = DateTime.ParseExact(strMesFrom + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            var mesTo = mesFrom.AddMonths(1).AddDays(-1);

            return await _service.GetCalcSaldos(mesFrom, mesTo);
        }

        public class Uploadf {
            public byte[] file { get; set; }
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file) {
            
            var ofxContent = new StreamReader(file.OpenReadStream()).ReadToEnd();


            bool sucesso = false;
            string msgerro = null;

            try
            {
                await _service.Upload(ofxContent);
                sucesso = true;
            }
            catch (Exception e)
            {
                sucesso = false;
                msgerro = e.Message + (e.InnerException != null ? e.InnerException.Message : "");
            }

            //}

            //this.TempData["ErroUpload"] = msgerro;
            //this.TempData["SucessoUpload"] = sucesso;

            return Ok(new
            {
                sucesso,
                msgerro
            });
        }

        [HttpGet("GetEstatisticas/{strMesFrom}")]
        public async Task<IActionResult> GetEstatisticas(string strMesFrom)
        {
            return Ok(await _service.GetEstatisticas(strMesFrom));
        }
    }
}
