using Dapper;
using EAgenda.WebApp.Compartilhado.Infra.Sql;
using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;
using Microsoft.Data.SqlClient;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Infra;

public sealed class RepositorioDespesaEmSql(ISqlConnectionFactory connectionFactory)
    : IRepositorioDespesa
{
    private const string InserirDespesaSql = """
        INSERT INTO dbo.TBDespesa (Id, Descricao, Ocorrencia, Valor, Pagamento)
        VALUES (@Id, @Descricao, @Ocorrencia, @Valor, @Pagamento);
    """;

    private const string InserirDespesaCategoriaSql = """
        INSERT INTO dbo.TBDespesaCategoria (DespesaId, CategoriaId)
        VALUES (@DespesaId, @CategoriaId);
    """;

    private const string AtualizarDespesaSql = """
        UPDATE dbo.TBDespesa
        SET Descricao = @Descricao,
            Ocorrencia = @Ocorrencia,
            Valor = @Valor,
            Pagamento = @Pagamento
        WHERE Id = @Id;
    """;

    private const string ExcluirDespesaCategoriasSql = """
        DELETE FROM dbo.TBDespesaCategoria
        WHERE DespesaId = @DespesaId;
    """;

    private const string ExcluirDespesaSql = """
        DELETE FROM dbo.TBDespesa
        WHERE Id = @Id;
    """;

    private const string SelecionarTodasDespesasSql = """
        SELECT
            d.Id AS DespesaId,
            d.Descricao,
            d.Ocorrencia,
            d.Valor,
            d.Pagamento,
            c.Id AS CategoriaId,
            c.Titulo AS CategoriaTitulo
        FROM dbo.TBDespesa AS d
        LEFT JOIN dbo.TBDespesaCategoria AS dc
            ON dc.DespesaId = d.Id
        LEFT JOIN dbo.TBCategoria AS c
            ON c.Id = dc.CategoriaId
        ORDER BY d.Ocorrencia DESC, d.Descricao, c.Titulo;
    """;

    private const string SelecionarDespesaPorIdSql = """
        SELECT
            d.Id AS DespesaId,
            d.Descricao,
            d.Ocorrencia,
            d.Valor,
            d.Pagamento,
            c.Id AS CategoriaId,
            c.Titulo AS CategoriaTitulo
        FROM dbo.TBDespesa AS d
        LEFT JOIN dbo.TBDespesaCategoria AS dc
            ON dc.DespesaId = d.Id
        LEFT JOIN dbo.TBCategoria AS c
            ON c.Id = dc.CategoriaId
        WHERE d.Id = @Id
        ORDER BY c.Titulo;
    """;

    public void Cadastrar(Despesa entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            conexao.Execute(InserirDespesaSql, CriarParametros(entidade), transacao);

            foreach (Categoria categoria in entidade.Categorias)
            {
                conexao.Execute(
                    InserirDespesaCategoriaSql,
                    new { DespesaId = entidade.Id, CategoriaId = categoria.Id },
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

    public bool Editar(Guid idSelecionado, Despesa entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            int linhasAfetadas = conexao.Execute(
                AtualizarDespesaSql,
                CriarParametros(entidadeAtualizada),
                transacao
            );

            if (linhasAfetadas != 1)
            {
                transacao.Rollback();
                return false;
            }

            conexao.Execute(
                ExcluirDespesaCategoriasSql,
                new { DespesaId = idSelecionado },
                transacao
            );

            foreach (Categoria categoria in entidadeAtualizada.Categorias)
            {
                conexao.Execute(
                    InserirDespesaCategoriaSql,
                    new { DespesaId = idSelecionado, CategoriaId = categoria.Id },
                    transacao
                );
            }

            transacao.Commit();
            return true;
        }
        catch
        {
            transacao.Rollback();
            throw;
        }
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            conexao.Execute(
                ExcluirDespesaCategoriasSql,
                new { DespesaId = idSelecionado },
                transacao
            );

            int linhasAfetadas = conexao.Execute(
                ExcluirDespesaSql,
                new { Id = idSelecionado },
                transacao
            );

            transacao.Commit();
            return linhasAfetadas == 1;
        }
        catch
        {
            transacao.Rollback();
            throw;
        }
    }

    public Despesa? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        List<DespesaRow> rows = conexao
            .Query<DespesaRow>(SelecionarDespesaPorIdSql, new { Id = idSelecionado })
            .ToList();

        if (rows.Count == 0)
            return null;

        return MapearDespesa(rows);
    }

    public List<Despesa> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return MapearDespesas(conexao.Query<DespesaRow>(SelecionarTodasDespesasSql));
    }

    public List<Despesa> Filtrar(Predicate<Despesa> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }

    private static object CriarParametros(Despesa entidade)
    {
        return new
        {
            entidade.Id,
            entidade.Descricao,
            Ocorrencia = entidade.Ocorrencia.Date,
            entidade.Valor,
            Pagamento = entidade.Pagamento.ToString()
        };
    }

    private static List<Despesa> MapearDespesas(IEnumerable<DespesaRow> rows)
    {
        return rows
            .GroupBy(row => row.DespesaId)
            .Select(MapearDespesa)
            .ToList();
    }

    private static Despesa MapearDespesa(IEnumerable<DespesaRow> rows)
    {
        DespesaRow primeira = rows.First();

        List<Categoria> categorias = rows
            .Where(row => row.CategoriaId.HasValue)
            .Select(row => new Categoria
            {
                Id = row.CategoriaId!.Value,
                Titulo = row.CategoriaTitulo ?? string.Empty
            })
            .ToList();

        return new Despesa
        {
            Id = primeira.DespesaId,
            Descricao = primeira.Descricao,
            Ocorrencia = primeira.Ocorrencia,
            Valor = primeira.Valor,
            Pagamento = Enum.Parse<FormaPagamento>(primeira.Pagamento ?? FormaPagamento.AVista.ToString()),
            Categorias = categorias
        };
    }
}

public sealed class DespesaRow
{
    public Guid DespesaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime Ocorrencia { get; set; }
    public decimal Valor { get; set; }
    public string? Pagamento { get; set; }
    public Guid? CategoriaId { get; set; }
    public string? CategoriaTitulo { get; set; }
}
