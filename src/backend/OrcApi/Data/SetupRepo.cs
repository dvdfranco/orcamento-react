using MongoDB.Driver;
using Orcamento.Models;
using Orcamento.Models.Parameters;
using System.Drawing;
using System.Runtime.Intrinsics.Arm;

namespace OrcamentoApi.Data
{
    public class SetupRepo : ISetupRepo
    {
        private readonly MongoClient _client;
        private readonly AppSettings _appSettings;
        private IMongoDatabase? database;
        public List<string>? collectionNames { get; set; } = new();

        private string guidConta = string.Empty;
        private string guidSalario = string.Empty;
        private string guidContas = string.Empty;
        private string guidMoradia = string.Empty;

        public SetupRepo(AppSettings appSettings)
        {
            _client = new MongoClient(appSettings.MongoUrl);
            _appSettings = appSettings;
        }

        public async Task SetupDatabaseFirstUse()
        {
            await CreateDB();

            collectionNames = (await database.ListCollectionNamesAsync()).ToList();
            
            guidSalario = Util.NewObjectId();
            guidConta = Util.NewObjectId();
            guidContas = Util.NewObjectId();
            guidMoradia = Util.NewObjectId();

            await CreateConta();
            await CreateCategoria();
            await CreateMemoPadrao();
            await CreateSaldo();
            await CreateSaldoAnterior();
            await CreateLancamento();
            await CreateLancamentoPrevisto();
            await CreateParcelaCartao();
        }

        private async Task CreateDB()
        {
            // Check if the database exists
            var databaseNames = (await _client.ListDatabaseNamesAsync()).ToList();
            if (!databaseNames.Contains(_appSettings.DB_NAME))
            {
                // Create the database by creating a collection
                database = _client.GetDatabase(_appSettings.DB_NAME);
                await database.CreateCollectionAsync("sample");
            }
            else
                database = _client.GetDatabase(_appSettings.DB_NAME);
        }

        private async Task CreateCategoria()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_CATEGORIA))
                return;

            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_CATEGORIA);
            var collection = database.GetCollection<Categoria>(_appSettings.DB_COLLECTION_CATEGORIA);

            await collection.InsertManyAsync([
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108df"), Descricao =  "Sem Categoria",
                    Icone = "fa-question-circle", Tipo = TipoConta.Corrente
                    }
                ,
                new() {
                    Id = guidSalario, Descricao =  "Salario",
                    Icone = "fa-dollar-sign", Tipo= TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108d1"), Descricao =  "Recebimentos",
                    Icone = "fa-dollar-sign", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108d2"), Descricao = "Saúde",
                    Icone = "fa-briefcase-medical", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108d4"), Descricao = "Shopping/Lazer",
                    Icone = "fa-shopping-bag", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108d5"), Descricao = "Cosméticos",
                    Icone = "fa-paint-brush", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108d6"), Descricao = "Educação",
                    Icone = "fa-dollar-sign", Tipo= TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108da"), Descricao = "Roupas",
                    Icone = "fa-tshirt", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108dc"), Descricao = "Farmácia",
                    Icone = "fa-prescription-bottle-alt", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108e1"), Descricao = "Mercado",
                    Icone = "fa-shopping-cart", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108ea"), Descricao = "Compras",
                    Icone = "fa-shopping-basket", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId(_appSettings.CategoriaAlimentacao), Descricao = "Alimentacao",
                    Icone = "fa-apple-alt", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = guidContas, Descricao = "Contas",
                    Icone = "fa-file-invoice-dollar", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId(_appSettings.CategoriaCarro), Descricao = "Carro",
                    Icone = "fa-car", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108f1"), Descricao = "Poupança",
                    Icone = "fa-piggy-bank", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = guidMoradia, Descricao = "Moradia",
                    Icone = "fa-home", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108f4"), Descricao = "Gordices",
                    Icone = "fa-candy-cane", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108f6"), Descricao = "Gastos Domésticos",
                    Icone = "fa-house-damage", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108f7"), Descricao = "Pets",
                    Icone = "fa-fish", Tipo = TipoConta.Corrente
                },
                new ()
                {
                    Id = Util.NewObjectId("617a073c70752ef5b7e108fa"), Descricao = "Presentes",
                    Icone = "fa-gift", Tipo = TipoConta.Corrente
                }
            ]);
        }

        private async Task CreateMemoPadrao()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_MEMO_PADRAO))
                return;

            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_MEMO_PADRAO);

            var collection = database.GetCollection<MemoPadrao>(_appSettings.DB_COLLECTION_MEMO_PADRAO);
            await collection.InsertManyAsync([
                new MemoPadrao() { From= "IFD.", To = "IFOOD", Tipo = "contains", IdCategoria = Util.NewObjectId(_appSettings.CategoriaAlimentacao) },
                new MemoPadrao() { From= "Posto", To = "Combustivel", Tipo = "contains", IdCategoria = Util.NewObjectId(_appSettings.CategoriaCarro) },
            ]);
        }

        private async Task CreateSaldo()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_SALDO))
                return;

            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_SALDO);

            var colleciton = database.GetCollection<Saldo>(_appSettings.DB_COLLECTION_SALDO);
            await colleciton.InsertManyAsync([
                new Saldo() { Id = Util.NewObjectId(), Valor = 0, Data = DateTime.Today.AddMonths(-2) },
                new Saldo() { Id = Util.NewObjectId(), Valor = 0, Data = DateTime.Today.AddMonths(-1) },
                new Saldo() { Id = Util.NewObjectId(), Valor = 0, Data = DateTime.Today}
            ]);
        }

        private async Task CreateSaldoAnterior()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_SALDO_ANTERIOR))
                return;
            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_SALDO_ANTERIOR);

            var collection = database.GetCollection<SaldoAnterior>(_appSettings.DB_COLLECTION_SALDO_ANTERIOR);
            await collection.InsertManyAsync([
                new SaldoAnterior() { Id = Util.NewObjectId(), Valor = 0, Data = DateTime.Today.AddMonths(-2) },
                new SaldoAnterior() { Id = Util.NewObjectId(), Valor = 0, Data = DateTime.Today.AddMonths(-1) },
                new SaldoAnterior() { Id = Util.NewObjectId(), Valor = 0, Data = DateTime.Today }
            ]);
        }

        private async Task CreateConta()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_CONTA))
                return;
            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_CONTA);

            var collection1 = database.GetCollection<Conta>(_appSettings.DB_COLLECTION_CONTA);
            await collection1.InsertOneAsync(new Conta() { Id = guidConta, Nome = "Conta Corrente Padrao", Ordem = 1 });
        }

        private async Task CreateLancamento()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_LANCAMENTO))
                return;
            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_LANCAMENTO);
            
            var collection = database.GetCollection<Lancamento>(_appSettings.DB_COLLECTION_LANCAMENTO);

            await collection.InsertManyAsync([
                new Lancamento() {
                    Id = Util.NewObjectId(),
                    IdConta = Util.NewObjectId(guidConta),
                    Transid = 156806,
                    Valor = -50,
                    ValorOriginal =-50,
                    Data = DateTime.Today,
                    DataRef = DateTime.Today,
                    Autocomplete = true,
                    Tipo = TipoConta.Corrente,
                    Memo = "Almoco",
                    BankMemo = "COMPRA CARTAO MAESTRO              04/04 COCOTINHA",
                    IdCategoria = Util.NewObjectId(_appSettings.CategoriaAlimentacao),
                    DataInclusao = DateTime.Now
                },
                new Lancamento() {
                    Id = Util.NewObjectId(),
                    IdConta = Util.NewObjectId(guidConta),
                    Transid = 156807,
                    Valor = 15000,
                    ValorOriginal =15000,
                    Data = DateTime.Today,
                    DataRef = DateTime.Today,
                    Autocomplete = true,
                    Tipo = TipoConta.Corrente,
                    Memo = "Salario",
                    BankMemo = "DEPOSITO CONTA CORRENTE",
                    IdCategoria = Util.NewObjectId(guidSalario),
                    DataInclusao = DateTime.Now
                },
            ]);
        }

        private async Task CreateLancamentoPrevisto()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_LANCAMENTO_PREVISTO))
                return;
            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_LANCAMENTO_PREVISTO);
            var collection = database.GetCollection<LancamentoPrevisto>(_appSettings.DB_COLLECTION_LANCAMENTO_PREVISTO);

            await collection.InsertManyAsync([
                new LancamentoPrevisto() { 
                    Id = Util.NewObjectId(),
                    Valor = 10000,
                    Data = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1),
                    Memo = "Salario",
                    IdCategoria = Util.NewObjectId(guidSalario),
                    DataLimite = DateTime.Today.AddYears(1),
                },
                new LancamentoPrevisto() { 
                    Id = Util.NewObjectId(),
                    Valor = -700,
                    Data = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1),
                    Memo = "Cartão de Crédito",
                    IdCategoria = Util.NewObjectId(guidContas),
                    DataLimite = DateTime.Today.AddYears(1),
                },
                new LancamentoPrevisto() { 
                    Id = Util.NewObjectId(),
                    Valor = -1200,
                    Data = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1),
                    Memo = "Aluguel",
                    IdCategoria = Util.NewObjectId(guidMoradia),
                    DataLimite = DateTime.Today.AddYears(1),
                },
            ]);
        }

        private async Task CreateParcelaCartao()
        {
            if (collectionNames.Contains(_appSettings.DB_COLLECTION_PARCELA_CARTAO))
                return;
            await database.CreateCollectionAsync(_appSettings.DB_COLLECTION_PARCELA_CARTAO);

            var collection = database.GetCollection<ParcelaCartao>(_appSettings.DB_COLLECTION_PARCELA_CARTAO);

            await collection.InsertManyAsync([
                new ParcelaCartao() {
                    Id = Util.NewObjectId(),
                    Valor = -700,
                    DataCompra = DateTime.Today.AddMonths(-1),
                    DataInicio = DateTime.Today.AddMonths(-1),
                    DataTermino = DateTime.Today.AddMonths(4),
                    Descricao = "Baú da felicidade",
                    DescricaoCartao = "COMRA**BAU*FELIC 02/05",
                    IdCartao = 1,
                    ParcelaAtual=2,
                    QtdParcelas = 5
                },
            ]);
        }
    }
}
