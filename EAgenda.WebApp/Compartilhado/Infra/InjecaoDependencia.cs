using EAgenda.WebApp.Compartilhado.Infra.Arquivos;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;
using EAgenda.WebApp.Modulos.ModuloContato.Infra;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Infra;
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            ContextoJson contextoJson = new ContextoJson();

            contextoJson.Carregar();

            return contextoJson;
        });

        services.AddScoped<IRepositorioContato, RepositorioContatoEmArquivo>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoEmArquivo>();
        // services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmArquivo>();
        // services.AddScoped<IRepositorioDespesa, RepositorioDespesaEmArquivo>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefaEmArquivo>();
    }
}
