
using EAgenda.WebApp.Compartilhado.Infra.Arquivos;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Infra;

public class RepositorioCompromissoEmArquivo :
    RepositorioBaseEmArquivo<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromissoEmArquivo(ContextoJson contexto) : base(contexto)
    {
    }

    protected override List<Compromisso> CarregarRegistros()
    {
        return contexto.Compromissos;
    }
}
