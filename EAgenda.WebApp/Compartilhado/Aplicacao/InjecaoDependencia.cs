using EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloContato.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloDespesa.Aplicacao;

namespace EAgenda.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ServicoContato>();
        // services.AddScoped<ServicoCompromisso>();
        services.AddScoped<ServicoCategoria>();
        services.AddScoped<ServicoDespesa>();
        // services.AddScoped<ServicoTarefa>();
    }
}