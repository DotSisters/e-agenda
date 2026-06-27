using EAgenda.WebApp.Compartilhado.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

public class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; private set; } = string.Empty;

    public PrioridadeTarefa Prioridade { get; private set; }

    public DateOnly DataCriacao { get; private set; }

    public DateOnly? DataConclusao { get; private set; }

    public StatusConclusao Status { get; private set; }

    public decimal PercentualConcluido { get; private set; }

    // private readonly List<ItemTarefa> itens = [];

    public Tarefa() { }
    public Tarefa(string titulo, PrioridadeTarefa prioridade)
    {
        Titulo = titulo;
        Prioridade = prioridade;
        DataCriacao = DateOnly.FromDateTime(DateTime.Now);
        Status = StatusConclusao.Pendente;
        PercentualConcluido = 0;
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
}
