using EAgenda.WebApp.Compartilhado.Infra.Arquivos;
using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using EAgenda.WebApp.Modulos.ModuloCategoria.Infra;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;
using EAgenda.WebApp.Modulos.ModuloContato.Infra;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Infra;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;
using EAgenda.WebApp.Modulos.ModuloDespesa.Infra;
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;
using EAgenda.WebApp.Compartilhado.Infra.Sql;
using EAgenda.WebApp.Modulos.ModuloTarefa.Infra;

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

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();



        services.AddScoped<IRepositorioContato, RepositorioContatoEmSql>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoEmSql>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmSql>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesaEmSql>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefaEmSql>();
    }
}
