using System.Reflection;
using Dapper;
using EAgenda.WebApp.Compartilhado.Infra.Sql;
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;
using Microsoft.Data.SqlClient;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Infra;

public sealed class RepositorioTarefaEmSql(ISqlConnectionFactory connectionFactory)
    : IRepositorioTarefa
{
    private Tarefa? tarefaEmEdicao;

    private const string InserirTarefaSql = """
        INSERT INTO dbo.TBTarefa (Id, Titulo, Prioridade, DataCriacao, DataConclusao, Status, PercentualConcluido)
        VALUES (@Id, @Titulo, @Prioridade, @DataCriacao, @DataConclusao, @Status, @PercentualConcluido);
    """;

    private const string AtualizarTarefaSql = """
        UPDATE dbo.TBTarefa
        SET Titulo = @Titulo,
            Prioridade = @Prioridade
        WHERE Id = @Id;
    """;

    private const string AtualizarTarefaCompletaSql = """
        UPDATE dbo.TBTarefa
        SET Titulo = @Titulo,
            Prioridade = @Prioridade,
            DataCriacao = @DataCriacao,
            DataConclusao = @DataConclusao,
            Status = @Status,
            PercentualConcluido = @PercentualConcluido
        WHERE Id = @Id;
    """;

    private const string ExcluirTarefaSql = """
        DELETE FROM dbo.TBTarefa
        WHERE Id = @Id;
    """;

    private const string ExcluirItensPorTarefaSql = """
        DELETE FROM dbo.TBItemTarefa
        WHERE TarefaId = @TarefaId;
    """;

    private const string InserirItemSql = """
        INSERT INTO dbo.TBItemTarefa (Id, Titulo, Status, TarefaId)
        VALUES (@Id, @Titulo, @Status, @TarefaId);
    """;

    private const string SelecionarTodasTarefasSql = """
        SELECT
            t.Id AS TarefaId,
            t.Titulo,
            t.Prioridade,
            t.DataCriacao,
            t.DataConclusao,
            t.Status,
            t.PercentualConcluido,
            i.Id AS ItemId,
            i.Titulo AS ItemTitulo,
            i.Status AS ItemStatus
        FROM dbo.TBTarefa AS t
        LEFT JOIN dbo.TBItemTarefa AS i
            ON i.TarefaId = t.Id
        ORDER BY t.DataCriacao DESC, i.Titulo;
    """;

    private const string SelecionarTarefaPorIdSql = """
        SELECT
            t.Id AS TarefaId,
            t.Titulo,
            t.Prioridade,
            t.DataCriacao,
            t.DataConclusao,
            t.Status,
            t.PercentualConcluido,
            i.Id AS ItemId,
            i.Titulo AS ItemTitulo,
            i.Status AS ItemStatus
        FROM dbo.TBTarefa AS t
        LEFT JOIN dbo.TBItemTarefa AS i
            ON i.TarefaId = t.Id
        WHERE t.Id = @Id
        ORDER BY i.Titulo;
    """;

    public void Cadastrar(Tarefa entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirTarefaSql, CriarParametros(entidade));
    }

    public bool Editar(Guid idSelecionado, Tarefa entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(AtualizarTarefaSql, CriarParametrosEdicao(entidadeAtualizada)) == 1;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(ExcluirTarefaSql, new { Id = idSelecionado }) == 1;
    }

    public Tarefa? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        List<TarefaRow> rows = conexao
            .Query<TarefaRow>(SelecionarTarefaPorIdSql, new { Id = idSelecionado })
            .ToList();

        if (rows.Count == 0)
            return null;

        Tarefa tarefa = MapearTarefa(rows);
        tarefaEmEdicao = tarefa;

        return tarefa;
    }

    public List<Tarefa> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return MapearTarefas(conexao.Query<TarefaRow>(SelecionarTodasTarefasSql));
    }

    public List<Tarefa> Filtrar(Predicate<Tarefa> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }

    public void Salvar()
    {
        if (tarefaEmEdicao == null)
            return;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            conexao.Execute(
                AtualizarTarefaCompletaSql,
                CriarParametros(tarefaEmEdicao),
                transacao
            );

            conexao.Execute(
                ExcluirItensPorTarefaSql,
                new { TarefaId = tarefaEmEdicao.Id },
                transacao
            );

            foreach (ItemTarefa item in tarefaEmEdicao.Itens)
            {
                conexao.Execute(
                    InserirItemSql,
                    CriarParametrosItem(item, tarefaEmEdicao.Id),
                    transacao
                );
            }

            transacao.Commit();
        }
        catch
        {
            transacao.Rollback();
            throw;
        }
    }

    private static object CriarParametros(Tarefa entidade)
    {
        return new
        {
            entidade.Id,
            entidade.Titulo,
            Prioridade = entidade.Prioridade.ToString(),
            DataCriacao = entidade.DataCriacao.ToDateTime(TimeOnly.MinValue),
            DataConclusao = entidade.DataConclusao?.ToDateTime(TimeOnly.MinValue),
            Status = entidade.Status.ToString(),
            entidade.PercentualConcluido
        };
    }

    private static object CriarParametrosEdicao(Tarefa entidade)
    {
        return new
        {
            entidade.Id,
            entidade.Titulo,
            Prioridade = entidade.Prioridade.ToString()
        };
    }

    private static object CriarParametrosItem(ItemTarefa item, Guid tarefaId)
    {
        return new
        {
            item.Id,
            item.Titulo,
            Status = item.Status.ToString(),
            TarefaId = tarefaId
        };
    }

    private static List<Tarefa> MapearTarefas(IEnumerable<TarefaRow> rows)
    {
        return rows
            .GroupBy(row => row.TarefaId)
            .Select(MapearTarefa)
            .ToList();
    }

    private static Tarefa MapearTarefa(IEnumerable<TarefaRow> rows)
    {
        TarefaRow primeira = rows.First();

        List<ItemTarefa> itens = rows
            .Where(row => row.ItemId.HasValue)
            .Select(row => new ItemTarefa
            {
                Id = row.ItemId!.Value,
                Titulo = row.ItemTitulo ?? string.Empty,
                Status = Enum.Parse<StatusConclusao>(row.ItemStatus!)
            })
            .ToList();

        Tarefa tarefa = new()
        {
            Id = primeira.TarefaId,
            Titulo = primeira.Titulo,
            Prioridade = Enum.Parse<PrioridadeTarefa>(primeira.Prioridade),
            DataCriacao = DateOnly.FromDateTime(primeira.DataCriacao),
            DataConclusao = primeira.DataConclusao.HasValue
                ? DateOnly.FromDateTime(primeira.DataConclusao.Value)
                : null,
            Status = Enum.Parse<StatusConclusao>(primeira.Status),
            PercentualConcluido = primeira.PercentualConcluido
        };

        DefinirItens(tarefa, itens);

        return tarefa;
    }

    private static void DefinirItens(Tarefa tarefa, List<ItemTarefa> itens)
    {
        FieldInfo? campoItens = typeof(Tarefa).GetField(
            "itens",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

        campoItens?.SetValue(tarefa, itens);
    }
}

public sealed class TarefaRow
{
    public Guid TarefaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Prioridade { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PercentualConcluido { get; set; }
    public Guid? ItemId { get; set; }
    public string? ItemTitulo { get; set; }
    public string? ItemStatus { get; set; }
}
