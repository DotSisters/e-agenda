
using EAgenda.WebApp.Compartilhado.Infra.Arquivos;
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;
public class RepositorioTarefaEmArquivo :
    RepositorioBaseEmArquivo<Tarefa>, IRepositorioTarefa
{
    public RepositorioTarefaEmArquivo(ContextoJson contexto) : base(contexto)
    {
    }

    protected override List<Tarefa> CarregarRegistros()
    {
        return contexto.Tarefas;
    }
}
