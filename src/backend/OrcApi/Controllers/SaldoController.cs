using Microsoft.AspNetCore.Mvc;
using Orcamento.Models;
using Orcamento.Models.Parameters;

namespace OrcamentoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly Services.SaldoService _service;

        public SaldoController(Services.SaldoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<Saldo> Get(string strMes)
        {
            DateTime mes = DateTime.ParseExact(strMes, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return (await _service.Search(new SaldoParameters() { Mes = mes })).SingleOrDefault();
        }

        [HttpGet]
        [Route("GetUltimo")]
        public async Task<Saldo> GetUltimo(string strMesTo)
        {
            DateTime mesTo = DateTime.ParseExact(strMesTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return (await _service.Search(new SaldoParameters() { MesTo = mesTo })).OrderByDescending(x => x.Data).FirstOrDefault();
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IEnumerable<Saldo>> Search(SaldoParameters parameters)
        {
            var ls = await _service.Search(parameters);

            return ls;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Saldo item)
        {
            await _service.Add(item);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Saldo item)
        {
            await _service.Update(item);
            return Ok();
        }

        [HttpGet]
        [Route("GetSaldoAnterior")]
        public async Task<SaldoAnterior> GetSaldoAnterior(string strMes)
        {
            DateTime mes = DateTime.ParseExact(strMes, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return await _service.SearchSaldoAnterior(new SaldoParameters() { Mes = mes });
        }

        [HttpPost]
        [Route("AddSaldoAnterior")]
        public async Task<IActionResult> AddSaldoAnterior(SaldoAnterior item)
        {
            await _service.AddSaldoAnterior(item);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateSaldoAnterior")]
        public async Task<IActionResult> UpdateSaldoAnterior(SaldoAnterior item)
        {
            var saldo = await this.GetSaldoAnterior(item.Data.ToString("yyyy-MM-dd"));
            if (saldo != null)
            {
                item.Id = saldo.Id;
                await _service.UpdateSaldoAnterior(item);
            }
            else
                await _service.AddSaldoAnterior(item);

            return Ok();
        }
    }
}
