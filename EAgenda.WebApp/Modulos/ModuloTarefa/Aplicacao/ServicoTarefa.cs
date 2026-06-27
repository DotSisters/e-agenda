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

    public Result Editar(EditarTarefaDto dto)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(dto.Id);

        if (tarefa == null)
            return Result.Fail("Tarefa não encontrada.");

        Tarefa tarefaAtualizada = new Tarefa(
            dto.Titulo,
            dto.Prioridade
        );

        Result resultadoValidacao = ValidarEntidade(tarefaAtualizada);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioTarefa.Editar(dto.Id, tarefaAtualizada);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Tarefa? compromisso = repositorioTarefa.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Tarefa não encontrado.");

        repositorioTarefa.Excluir(id);

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

    public Result<DetalhesTarefaDto> SelecionarPorId(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);

        if (tarefa == null)
            return Result.Fail("Tarefa nao encontrada.");

        return Result.Ok(new DetalhesTarefaDto(
            tarefa.Id,
            tarefa.Titulo,
            tarefa.Prioridade,
            tarefa.Status,
            tarefa.PercentualConcluido
        ));
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
