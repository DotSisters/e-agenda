using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;

public class ServicoTarefa
{
    private readonly IRepositorioTarefa repositorioTarefa;

    public ServicoTarefa(IRepositorioTarefa repositorioTarefa)
    {
        this.repositorioTarefa = repositorioTarefa;

    }

    public List<ListarTarefasDto> SelecionarTodos()
    {
        return repositorioTarefa
            .SelecionarTodos()
            .Select(t => new ListarTarefasDto(
                t.Id,
                t.Titulo,
                t.Prioridade,
                t.DataCriacao,
                t.DataConclusao,
                t.Status,
                t.PercentualConcluido
            ))
            .ToList();
    }
}
