using System.Text.Json.Serialization;
using EAgenda.WebApp.Compartilhado.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

public class Tarefa : EntidadeBase<Tarefa>
{
  public string Titulo { get; set; } = string.Empty;

  public PrioridadeTarefa Prioridade { get; set; }

  public DateOnly DataCriacao { get; set; }

  public DateOnly? DataConclusao { get; set; }

  public StatusConclusao Status { get; set; }

  public decimal PercentualConcluido { get; set; }

  [JsonInclude]
  private List<ItemTarefa> itens = [];

  [JsonIgnore]
  public IReadOnlyList<ItemTarefa> Itens => itens;

  public Tarefa() { }
  public Tarefa(string titulo, PrioridadeTarefa prioridade)
  {
    Titulo = titulo;
    Prioridade = prioridade;
    DataCriacao = DateOnly.FromDateTime(DateTime.Now);
    Status = StatusConclusao.Pendente;
    PercentualConcluido = 0;
  }

  public void AdicionarItem(ItemTarefa item)
  {
    itens.Add(item);
    AtualizarPercentual();
  }

  public void RemoverItem(Guid itemId)
  {
    ItemTarefa? item = itens.FirstOrDefault(i => i.Id == itemId);

    if (item == null)
      return;

    itens.Remove(item);
    AtualizarPercentual();
  }

  public void ConcluirItem(Guid itemId)
  {
    ItemTarefa? item = itens.FirstOrDefault(i => i.Id == itemId);

    if (item == null)
      return;

    item.Status = StatusConclusao.Concluida;
    AtualizarPercentual();
  }

  public void ReabrirItem(Guid itemId)
  {
    ItemTarefa? item = itens.FirstOrDefault(i => i.Id == itemId);

    if (item == null)
      return;

    item.Status = StatusConclusao.Pendente;
    AtualizarPercentual();
  }

  public void AtualizarPercentual()
  {
    if (itens.Count == 0)
    {
      PercentualConcluido = 0;
      Status = StatusConclusao.Pendente;
      DataConclusao = null;
      return;
    }

    int total = itens.Count;
    int concluidos = itens.Count(i => i.Status == StatusConclusao.Concluida);

    PercentualConcluido = (decimal)concluidos / total * 100;
    AtualizarStatusPorPercentual();
  }

  public void ConcluirManualmente()
  {
    if (itens.Count > 0)
      return;

    Status = StatusConclusao.Concluida;
    PercentualConcluido = 100;
    DataConclusao = DateOnly.FromDateTime(DateTime.Now);
  }

  public void ReabrirManualmente()
  {
    if (itens.Count > 0)
      return;

    Status = StatusConclusao.Pendente;
    PercentualConcluido = 0;
    DataConclusao = null;
  }

  public override List<string> Validar()
  {
    List<string> erros = [];

    if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
      erros.Add("O campo \"Título\" deve conter entre 2 e 100 caracteres.");

    if (!Enum.IsDefined(typeof(PrioridadeTarefa), Prioridade))
      erros.Add("O campo \"Prioridade\" deve ser Baixa, Normal ou Alta.");

    return erros;
  }

  public override void Atualizar(Tarefa entidadeAtualizada)
  {
    Titulo = entidadeAtualizada.Titulo;
    Prioridade = entidadeAtualizada.Prioridade;
  }

  private void AtualizarStatusPorPercentual()
  {
    if (itens.Count == 0)
      return;

    if (PercentualConcluido == 100)
    {
      Status = StatusConclusao.Concluida;
      DataConclusao = DateOnly.FromDateTime(DateTime.Now);
    }
    else
    {
      Status = StatusConclusao.Pendente;
      DataConclusao = null;
    }
  }
}
