using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;
using FluentResults;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;

public class ServicoTarefa
{
    private readonly IRepositorioTarefa repositorioTarefa;

    public ServicoTarefa(IRepositorioTarefa repositorioTarefa)
    {
        this.repositorioTarefa = repositorioTarefa;

    }

    public Result Cadastrar(CadastrarTarefaDto dto)
    {
        Tarefa novaTarefa = new(
            dto.Titulo,
            dto.Prioridade
        );
        // {
        //     Itens = dto.Itens
        //         .Where(i => !string.IsNullOrWhiteSpace(i))
        //         .Select(i => new ItemTarefa(i))
        //         .ToList()
        // };

        // tarefa.AtualizarPercentual();

        Result resultadoValidacao = ValidarEntidade(novaTarefa);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioTarefa.Cadastrar(novaTarefa);

        return Result.Ok();
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

    private static Result ValidarEntidade(Tarefa tarefa)
    {
        List<string> erros = tarefa.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}
