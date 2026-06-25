using EAgenda.WebApp.Compartilhado.Infra.Arquivos;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloContato.Infra;

public class RepositorioContatoEmArquivo : RepositorioBaseEmArquivo<Contato>, IRepositorioContato
{
    public RepositorioContatoEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Contato> CarregarRegistros()
    {
        return contexto.Contatos;
    }
}