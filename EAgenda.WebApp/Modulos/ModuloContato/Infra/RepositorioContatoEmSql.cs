using EAgenda.WebApp.Compartilhado.Infra.Sql;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;
using Microsoft.Data.SqlClient;
using Dapper;

namespace EAgenda.WebApp.Modulos.ModuloContato.Infra;

public sealed class RepositorioContatoEmSql(ISqlConnectionFactory connectionFactory)
    : IRepositorioContato
{
    private const string InserirSql = """
        INSERT INTO dbo.TBContato (Id, Nome, Email, Telefone, Cargo, Empresa)
        VALUES (@Id, @Nome, @Email, @Telefone, @Cargo, @Empresa);
    """;

    private const string AtualizarSql = """
        UPDATE dbo.TBContato
        SET Nome = @Nome,
            Email = @Email,
            Telefone = @Telefone,
            Cargo = @Cargo,
            Empresa = @Empresa
        WHERE Id = @Id;
    """;

    private const string ExcluirSql = """
        DELETE FROM dbo.TBContato
        WHERE Id = @Id;
    """;

    private const string SelecionarTodosSql = """
        SELECT Id, Nome, Email, Telefone, Cargo, Empresa
        FROM dbo.TBContato
        ORDER BY Nome;
    """;

    private const string SelecionarPorIdSql = """
        SELECT Id, Nome, Email, Telefone, Cargo, Empresa
        FROM dbo.TBContato
        WHERE Id = @Id;
    """;

    public void Cadastrar(Contato entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirSql, entidade);
    }

    public bool Editar(Guid idSelecionado, Contato entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(AtualizarSql, entidadeAtualizada) > 0;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(ExcluirSql, new { Id = idSelecionado }) > 0;
    }

    public Contato? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.QuerySingleOrDefault<Contato>(SelecionarPorIdSql, new { Id = idSelecionado });
    }

    public List<Contato> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Query<Contato>(SelecionarTodosSql).ToList();
    }

    public List<Contato> Filtrar(Predicate<Contato> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }
}