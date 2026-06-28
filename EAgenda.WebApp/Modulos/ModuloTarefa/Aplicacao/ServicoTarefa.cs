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

  public List<ListarTarefasDto> Listar(StatusConclusao? status = null, PrioridadeTarefa? prioridade = null)
  {
    List<Tarefa> tarefas = repositorioTarefa.Filtrar(t =>
        (!status.HasValue || t.Status == status.Value) &&
        (!prioridade.HasValue || t.Prioridade == prioridade.Value));

    return tarefas
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

  public Result<ListarItensTarefaDto> ListarItens(Guid tarefaId)
  {
    Tarefa? tarefa = repositorioTarefa.SelecionarPorId(tarefaId);

    if (tarefa == null)
      return Result.Fail("Tarefa não encontrada.");

    return Result.Ok(MapearItensTarefa(tarefa));
  }

  public Result AdicionarItem(AdicionarItemTarefaDto dto)
  {
    Tarefa? tarefa = repositorioTarefa.SelecionarPorId(dto.TarefaId);

    if (tarefa == null)
      return Result.Fail("Tarefa não encontrada.");

    ItemTarefa novoItem = new(dto.Titulo);

    Result resultadoValidacao = ValidarItem(novoItem);

    if (resultadoValidacao.IsFailed)
      return resultadoValidacao;

    tarefa.AdicionarItem(novoItem);
    repositorioTarefa.Salvar();

    return Result.Ok();
  }

  public Result RemoverItem(Guid tarefaId, Guid itemId)
  {
    Tarefa? tarefa = repositorioTarefa.SelecionarPorId(tarefaId);

    if (tarefa == null)
      return Result.Fail("Tarefa não encontrada.");

    bool itemExiste = tarefa.Itens.Any(i => i.Id == itemId);

    if (!itemExiste)
      return Result.Fail("Item não encontrado.");

    tarefa.RemoverItem(itemId);
    repositorioTarefa.Salvar();

    return Result.Ok();
  }

  public Result ConcluirItem(Guid tarefaId, Guid itemId)
  {
    Tarefa? tarefa = repositorioTarefa.SelecionarPorId(tarefaId);

    if (tarefa == null)
      return Result.Fail("Tarefa não encontrada.");

    ItemTarefa? item = tarefa.Itens.FirstOrDefault(i => i.Id == itemId);

    if (item == null)
      return Result.Fail("Item não encontrado.");

    tarefa.ConcluirItem(itemId);
    repositorioTarefa.Salvar();

    return Result.Ok();
  }

  public Result ReabrirItem(Guid tarefaId, Guid itemId)
  {
    Tarefa? tarefa = repositorioTarefa.SelecionarPorId(tarefaId);

    if (tarefa == null)
      return Result.Fail("Tarefa não encontrada.");

    ItemTarefa? item = tarefa.Itens.FirstOrDefault(i => i.Id == itemId);

    if (item == null)
      return Result.Fail("Item não encontrado.");

    tarefa.ReabrirItem(itemId);
    repositorioTarefa.Salvar();

    return Result.Ok();
  }

  private static ListarItensTarefaDto MapearItensTarefa(Tarefa tarefa)
  {
    return new ListarItensTarefaDto(
        tarefa.Id,
        tarefa.Titulo,
        tarefa.Status,
        tarefa.PercentualConcluido,
        tarefa.DataConclusao,
        tarefa.Itens
            .Select(i => new ItemTarefaDto(i.Id, i.Titulo, i.Status))
            .ToList()
    );
  }

  private static Result ValidarEntidade(Tarefa tarefa)
  {
    List<string> erros = tarefa.Validar();

    if (erros.Count == 0)
      return Result.Ok();

    return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
  }

  private static Result ValidarItem(ItemTarefa item)
  {
    List<string> erros = item.Validar();

    if (erros.Count == 0)
      return Result.Ok();

    return Result.Fail(new Error(erros.First()).WithMetadata("Campo", "Titulo"));
  }
}
