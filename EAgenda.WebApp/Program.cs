using EAgenda.WebApp.Compartilhado.Aplicacao;
using EAgenda.WebApp.Compartilhado.Apresentacao;
using EAgenda.WebApp.Compartilhado.Infra;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Dependências (Dependency Injection)
builder.Services.AddInfraRepositories();

builder.Services.AddApplicationServices();

builder.Services.AddPresentationConfig();

var app = builder.Build();

// Configuração de Middlewares
app.UseStaticFiles();

app.UseRouting();
app.MapDefaultControllerRoute();

// Execução do Servidor
app.Run();
