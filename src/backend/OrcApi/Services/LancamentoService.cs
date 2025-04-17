using OfxSharpLib;
using Orcamento.Models;
using Orcamento.Models.Parameters;
using OrcamentoApi.Data;

namespace OrcamentoApi.Services
{
    public class LancamentoService
    {
        private readonly ILancamentoRepo _repo;
        private readonly LancamentoPrevistoService _lspService;
        private readonly ICategoriaRepo _catRepo;
        private readonly IContaRepo _contaRepo;
        private readonly IMemoPadraoRepo _memoPadraoRepo;
        private readonly AppSettings _appSettings;
        private readonly SaldoService _saldoService;

        public LancamentoService(AppSettings appSettings, Data.ILancamentoRepo repo,
            Data.ICategoriaRepo catRepo, 
            Data.IContaRepo contaRepo, 
            Data.IMemoPadraoRepo memoRepo, 
            Services.LancamentoPrevistoService lspService, 
            SaldoService saldoService)
        {
            this._repo = repo;
            this._catRepo = catRepo;
            this._lspService = lspService;
            this._contaRepo = contaRepo;
            this._memoPadraoRepo = memoRepo;
            _appSettings = appSettings;
            _saldoService = saldoService;
        }

        public async Task<Lancamento> Get(string Id)
        {
            var lcto = await _repo.Single(Id);
            if (lcto != null)
            {
                var cat = await _catRepo.Single(lcto.IdCategoria);
                lcto.Categoria = cat;
            }

            return lcto;
        }

        public async Task<List<Lancamento>> All()
        {
            return await _repo.All();
        }

        public async Task<IEnumerable<Lancamento>> Search(LancamentoParameters parameters)
        {
            var ls = (await _repo.Search(parameters)).ToList();

            //TODO: seria otimo isso conseguir entrar na query do _repo:
            if (parameters.MemoPersonalizado.HasValue && parameters.MemoPersonalizado.Value)
                ls = ls.Where(x => !x.Memo.Equals(x.BankMemo)).ToList();

            var categorias = await _catRepo.All();
            var contas = await _contaRepo.All();

            if (parameters.IncludePrevistos)
            {
                var lsp = (await _lspService.Search(parameters)).ToList();

                lsp.ForEach(x =>
                {
                    var lcto = ls.Where(y => y.Memo.Trim().Equals(x.Memo.Trim())).FirstOrDefault();
                    if (lcto != null)
                    {
                        lcto.IsPrevisto = true;
                        lcto.IdLancamentoPrevisto = x.Id;
                    }
                    else
                        ls.Insert(0, new Lancamento(x, parameters.MesFrom.Value));
                });
            }


            ls.ForEach(x =>
            {
                x.Categoria = categorias.Where(y => y.Id == x.IdCategoria).SingleOrDefault();
                x.Conta = contas.Where(y => y.Id == x.IdConta).SingleOrDefault();
            });

            return ls;
        }
        
        public async Task<IEnumerable<LancamentoSimples>> SearchCustomMemos(Boolean? autoComplete)
        {
            var ls = await this.Search(new LancamentoParameters() {
                MemoPersonalizado = true,
                Autocomplete = autoComplete,
                MesFrom = DateTime.Today.AddMonths(-3)
            });

            var categorias = await _catRepo.All();

            var grupo = ls.Select(x => $"{x.Memo}||{x.IdCategoria}||{x.IsPrevisto}||{x.Autocomplete}").Distinct(); ;

            return grupo
                .Select(x => x.Split("||"))
                .Select(x => new LancamentoSimples
                {
                    Memo = x[0],
                    IdCategoria = x[1],
                    Categoria = categorias.FirstOrDefault(y => y.Id == x[1]),
                    IsPrevisto = bool.Parse(x[2]),
                    Autocomplete = bool.Parse(x[3])
                });
        }


        public async Task Add(Lancamento item)
        {
            await _repo.Add(item);
        }

        public async Task Update(Lancamento item)
        {
            await _repo.Update(item);
        }

        //Ligar/desligar o autocomplete dos customMemos: de todos os que tem o mesmo memo
        public async Task UpdateAutoComplete(string memo, bool autocomplete)
        {
            var ls = this.Search(new LancamentoParameters()
            {
                MesFrom = DateTime.Today.AddMonths(-3),
                Memo = memo
            });

            foreach (var item in ls.Result)
            {
                item.Autocomplete = autocomplete;
                await _repo.Update(item);
            }
        }

        public async Task Delete(string id)
        {
            await _repo.Delete(id);
        }

        // seed = qtd a adicionar, por seguranca para nao confundir com algum transid q o banco gera
        internal async Task<long> NewTransId(System.DateTime dataRef, int seed = 1000)
        {
            var dataRefIni = new System.DateTime(dataRef.Year, dataRef.Month, 01);
            var dataRefFim = dataRefIni.AddMonths(1).AddDays(-1);
            var lctos = await this.Search(new LancamentoParameters() { MesFrom = dataRefIni, MesTo = dataRefFim });
            if (lctos.Count() > 0)
                return lctos.Max(x => x.Transid + seed);
            else
                return long.Parse(dataRef.ToString("yyyyMMdd") + seed.ToString());
        }

        public async Task<List<Lancamento>> Dividir (Lancamento[] newItems)
        {
            List<string> newIds = new List<string>();
            var lcto = newItems[0];
            var newTransid = await this.NewTransId(lcto.DataRef);

            var valorOriginal = lcto.Valor;
            lcto.Valor = newItems[0].Valor;
            lcto.IdLancamentoRef = lcto.Id;

            newIds.Add(lcto.Id);

            if (newItems.Length > 1)
                for (var i = 1; i < newItems.Length; i++)
                {
                    var lcto2 = new Lancamento()
                    {
                        Autocomplete = lcto.Autocomplete,
                        BankMemo = lcto.BankMemo + " " + i.ToString(),
                        Memo = newItems[i].Memo,
                        IdCategoria = lcto.IdCategoria,
                        Data = lcto.Data,
                        DataRef = lcto.DataRef,
                        IsPrevisto = lcto.IsPrevisto,
                        Valor = newItems[i].Valor,
                        ValorOriginal = valorOriginal,
                        Transid = newTransid + i,
                        IdLancamentoRef = lcto.Id,
                        Tipo = TipoConta.Corrente
                    };
                    newIds.Add(lcto2.Id);
                    await this.Add(lcto2);
                }

            await this.Update(lcto);

            //Retornar o objeto completo dos itens divididos
            return (await this.Search(new LancamentoParameters() { IncludePrevistos = false, Ids = newIds.ToArray() })).ToList();
        }

        internal async Task<object> GetCalcSaldos(DateTime mesFrom, DateTime mesTo)
        {
            var ultimoSaldo = (await _saldoService.Search(new SaldoParameters() { MesTo = mesTo })).OrderByDescending(x => x.Data).FirstOrDefault();

            var ls = await this.Search(new LancamentoParameters() { MesFrom = mesFrom, MesTo = mesTo, Tipo = TipoConta.Corrente, IncludePrevistos = true });
            //var lsDesdeUltimoSaldo = await this.Search(new LancamentoParameters() { MesFrom = ultimoSaldo.Data, MesTo = mesTo, Tipo = TipoConta.Corrente, IncludePrevistos = true });
            //var lsDesteMes = lsDesdeUltimoSaldo.Where(x => x.DataRef >= mesFrom);
            //var lsReaisDesdeUltimoSaldo = lsDesdeUltimoSaldo.Where(x => !(x.IsPrevisto && x.Transid == 0));

            decimal saldo = 0;
            if (ultimoSaldo != null)
                if (ultimoSaldo.Data == DateTime.Today)
                    saldo = ultimoSaldo.Valor;
                else
                    saldo = ultimoSaldo.Valor + (await this.Search(new LancamentoParameters() { 
                        MesFrom = ultimoSaldo.Data, MesTo = mesTo, Tipo = TipoConta.Corrente 
                    })).Sum(x => x.Valor);


            
            var saldoAnterior = (await _saldoService.SearchSaldoAnterior(new SaldoParameters() { Mes = mesFrom }))?.Valor ?? 0;
            var soma = ls.Where(x => x.Transid != 0).Sum(x => x.Valor);

            return new
            {
                SaldoHoje = saldo,
                DataSaldoHoje = ultimoSaldo != null ? new DateTime(ultimoSaldo.Data.Year, ultimoSaldo.Data.Month, ultimoSaldo.Data.Day) : new DateTime(1985, 01, 20),
                SaldoAnterior = saldoAnterior,
                Soma = soma,
                SaldoCalculado = saldoAnterior + soma,
                SaldoPrevisto = saldoAnterior + ls.Sum(x => x.Valor)
            };
        }



        internal async Task Upload(string ofxContent)
        {
            var ofx = this.ReadOfx(ofxContent);

            var qtd = 0;
            var lctosAdicionados = new List<Lancamento>();
            var memosPadrao = await _memoPadraoRepo.All();
            //var allLancamentos = await _api.Get<IEnumerable<Lancamento>>("Lancamento/All");
            var allLancamentos = await this.Search(new LancamentoParameters() { MesFrom = DateTime.Today.AddMonths(-2), Tipo = TipoConta.Corrente, IncludePrevistos = false });
            var dataInclusao = DateTime.Now;

            foreach (var itemLcto in ofx.Transactions)
            {
                var lancamentoIgual = allLancamentos.Where(x => x.Data == itemLcto.Date && x.BankMemo == itemLcto.Memo.Trim());
                if (!lancamentoIgual.Where(x => x.ValorOriginal == itemLcto.Amount).Any())
                {
                    //Evitar cadastrar o mesmo transid de outro lcto, se o itau tiver alterado o transid
                    var transid = Int64.Parse(itemLcto.TransactionId);
                    if (transid == 0 || lctosAdicionados.Union(allLancamentos).Where(x => x.Transid == transid && x.Tipo == TipoConta.Corrente).Any()) transid = await this.NewTransId(itemLcto.Date) + qtd;

                    //íf (transid)
                    var memoPadrao = memosPadrao.Where(x => itemLcto.Memo.Trim().Contains(x.From)).FirstOrDefault();
                    var lcto = new Lancamento()
                    {
                        Data = itemLcto.Date,
                        DataRef = itemLcto.Date,
                        Memo = (memoPadrao != null ? memoPadrao.To : itemLcto.Memo.Trim()),
                        BankMemo = itemLcto.Memo.Trim(),
                        Transid = transid,
                        IdCategoria = (memoPadrao != null ? memoPadrao.IdCategoria : _appSettings.SemCategoria),
                        Valor = itemLcto.Amount,
                        ValorOriginal = itemLcto.Amount,
                        Autocomplete = true,
                        Tipo = TipoConta.Corrente,
                        DataInclusao = dataInclusao
                    };

                    await this.Add(lcto);
                    lctosAdicionados.Add(lcto);
                }
                qtd++;
            }

            var ultimoSaldo = (await _saldoService.Search(new() { Mes = ofx.Balance.LedgerBalanceDate })).FirstOrDefault();
            if (ultimoSaldo == null)
            {
                var saldo = new Saldo();
                saldo.Data = ofx.Balance.LedgerBalanceDate;
                saldo.Valor = ofx.Balance.LedgerBalance;

                await _saldoService.Add(saldo);
            }
            else if (ultimoSaldo.Valor != ofx.Balance.LedgerBalance)
            {
                ultimoSaldo.Valor = ofx.Balance.LedgerBalance;
                await _saldoService.Update(ultimoSaldo);
            }
        }

        private OfxDocument ReadOfx(string ofxContent)
        {
            //OFXParser.Entities.
            //OFXParser.Parser.GenerateExtract(caminhoDoArquivoOFX);


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //string ofxContent = System.IO.File.ReadAllText(lastOfx.FullName, System.Text.Encoding.GetEncoding(1252));


            //Substituir erro do Itau: Trocar 1.000,00 por 1000.00:
            //var m = Regex.Match(ofxContent, @"([0-9]+)(\.)([0-9]{3})(\,)([0-9]{1,3})$", RegexOptions.Multiline);
            //if (m.Success)
            //    ofxContent = Regex.Replace(ofxContent, @"([0-9]+)(\.)([0-9]{3})(\,)([0-9]{1,3})$", @"$1$3.$5", RegexOptions.Multiline);

            //Substituir erro do santander: <ACCTTYPE errado
            ofxContent = ofxContent.Replace("<ACCTTYPE>000000[-3:GMT]", "<ACCTTYPE>CHECKING");

            return new OfxDocumentParser().Import(ofxContent);

        }


        public async Task<object> GetEstatisticas(string strMesFrom)
        {
            var mesFrom = DateTime.ParseExact(strMesFrom + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            var mesTo = mesFrom.AddMonths(1).AddDays(-1);

            var query = await this.Search(new LancamentoParameters() { MesFrom = mesFrom, MesTo = mesTo, Tipo = TipoConta.Corrente, SoDebitos = true });

            var itens = query
                .Where(x => !(new string[] { //remover algumas categorias que nao quero nesse grafico
                    //TODO: constantes! appsettings!
                    "2262ddcc-d0dc-4a58-8eee-f3e1a62f8bf5", "617a073c70752ef5b7e108ed", //Contas
                    "91cbe9b3-e7dc-4423-9edf-d30b3aca1c1f", "617a073c70752ef5b7e108ce", //Dizimo
                    "94862c0d-13d4-4358-b546-cf6a6b41fa0e", "617a073c70752ef5b7e108f2"  //Moradia
                    }.Contains(x.IdCategoria.ToString()))
                )
                .GroupBy(x => x.Categoria.Descricao)
                .Select(x => new { Name = x.Key, Valor = x.Sum(y => y.Valor) })
                .OrderByDescending(x => x.Valor)
                .ToList();

            var dadosHistorico = await this.GetEstatisticasHistorico(mesFrom);

            return new
            {
                PieData = itens.Select(x => new Lancamento() { Memo = x.Name, Valor = x.Valor }),
                LineData = dadosHistorico
            };
        }

        internal async Task<List<EstatisticaGraficoLinha>> GetEstatisticasHistorico(DateTime mesFrom, string[] categorias = null)
        {
            List<EstatisticaGraficoLinha> retorno = new List<EstatisticaGraficoLinha>();
            mesFrom = mesFrom.AddMonths(-6);

            List<string> periodos = new List<string>();
            List<string> labels = new List<string>();
            for (var i = 0; i <= 6; i++)
            {
                periodos.Add(mesFrom.AddMonths(i).ToString("yyyyMM"));
                labels.Add(mesFrom.AddMonths(i).ToString("MMMM"));
            }

            if (categorias == null)
                categorias = new string[3] { Util.NewObjectId(_appSettings.CategoriaAlimentacao), Util.NewObjectId(_appSettings.CategoriaRoupas), Util.NewObjectId(_appSettings.CategoriaGordices) }; //alimentacao, roupas, gordices

            foreach (var itemCat in categorias)
            {
                var ls = await this.Search(new LancamentoParameters() { MesFrom = mesFrom, Tipo = TipoConta.Corrente, IdCategoria = itemCat });
                var somaValores = ls
                    .OrderBy(x => x.DataRef)
                    .GroupBy(x => x.DataRef.ToString("yyyyMM")).Select(x => new
                    {
                        NomeCategoria = x.FirstOrDefault()?.Categoria?.Descricao,
                        Periodo = x.Key,
                        Valor = x.Sum(y => Math.Abs(y.Valor))
                    })
                    .ToArray();

                //Caso a query acima nao tenha valor para um periodo (ex: jan, fev, ???, abr), fazer esse left join aqui pra trazer valores para todos os periodos (1, 2, 0, 3)
                var periodoValores =
                    from p in periodos
                    join s in somaValores on p equals s.Periodo into LJ
                    from leftjoin in LJ.DefaultIfEmpty()
                    select new
                    {
                        NomeCategoria = leftjoin != null ? leftjoin.NomeCategoria : ls.FirstOrDefault()?.Categoria?.Descricao,
                        Periodo = p,
                        Valor = leftjoin != null ? leftjoin.Valor : 0
                    };

                if (ls.Any())
                    retorno.Add(new EstatisticaGraficoLinha()
                    {
                        NomeCategoria = periodoValores.FirstOrDefault()?.NomeCategoria,
                        Valores = periodoValores.OrderBy(x => x.Periodo).Select(x => x.Valor).ToArray(),
                        Labels = labels.ToArray()
                    });

            }

            return retorno;
        }

    }
}
