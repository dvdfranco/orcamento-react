using Microsoft.Extensions.Options;
using Orcamento.Models;
using OrcamentoApi.Data;
using OrcamentoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/*cod dvd-*/

var appSettings = new AppSettings();
new ConfigureFromConfigurationOptions<AppSettings>(
    builder.Configuration.GetSection("AppSettings"))
        .Configure(appSettings);

builder.Services.AddSingleton(appSettings); 

builder.Services.AddScoped<ILancamentoRepo, LancamentoRepo>();
builder.Services.AddScoped<ILancamentoPrevistoRepo, LancamentoPrevistoRepo>();
builder.Services.AddScoped<ICategoriaRepo, CategoriaRepo>();
builder.Services.AddScoped<IContaRepo, ContaRepo>();
builder.Services.AddScoped<IParcelaCartaoRepo, ParcelaCartaoRepo>();
builder.Services.AddScoped<ISaldoRepo, SaldoRepo>();
builder.Services.AddScoped<ISaldoAnteriorRepo, SaldoAnteriorRepo>();
builder.Services.AddScoped<IMemoPadraoRepo, MemoPadraoRepo>();
builder.Services.AddScoped<ISetupRepo, SetupRepo>();

builder.Services.AddScoped<ILancamentoService, LancamentoService>();
builder.Services.AddScoped<ILancamentoPrevistoService, LancamentoPrevistoService>();
builder.Services.AddScoped<ISaldoService, SaldoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IParcelaCartaoService, ParcelaCartaoService>();
builder.Services.AddScoped<IMemoPadraoService, MemoPadraoService>();
builder.Services.AddScoped<ISetupService, SetupService>();

//CORS: precisa ter aqui no services e la embaixo no app.usecors:
const string CORS_POLICY = "cors, bitch!";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_POLICY,
                      policy =>
                      {
                          policy.AllowAnyMethod()
                          .AllowAnyOrigin()
                          .AllowAnyHeader();
                      });
});
/*fim dvd/*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//dvd:
app.UseCors(CORS_POLICY);

app.Run();
