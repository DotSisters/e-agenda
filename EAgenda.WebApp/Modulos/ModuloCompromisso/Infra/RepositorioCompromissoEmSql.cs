using Dapper;
using EAgenda.WebApp.Compartilhado.Infra.Sql;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;
using Microsoft.Data.SqlClient;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Infra;

public sealed class RepositorioCompromissoEmSql(ISqlConnectionFactory connectionFactory)
    : IRepositorioCompromisso
{
    private const string InserirCompromissoSql = """
        INSERT INTO dbo.TBCompromisso (Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino, Tipo, Local, Link, ContatoId)
        VALUES (@Id, @Assunto, @DataOcorrencia, @HoraInicio, @HoraTermino, @Tipo, @Local, @Link, @ContatoId);
    """;

    private const string AtualizarCompromissoSql = """
        UPDATE dbo.TBCompromisso
        SET Assunto = @Assunto,
            DataOcorrencia = @DataOcorrencia,
            HoraInicio = @HoraInicio,
            HoraTermino = @HoraTermino,
            Tipo = @Tipo,
            Local = @Local,
            Link = @Link,
            ContatoId = @ContatoId
        WHERE Id = @Id;
    """;

    private const string ExcluirCompromissoSql = """
        DELETE FROM dbo.TBCompromisso
        WHERE Id = @Id;
    """;

    private const string SelecionarTodosCompromissosSql = """
        SELECT
            c.Id AS CompromissoId,
            c.Assunto,
            c.DataOcorrencia,
            c.HoraInicio,
            c.HoraTermino,
            c.Tipo,
            c.Local,
            c.Link,
            ct.Id AS ContatoId,
            ct.Nome AS ContatoNome,
            ct.Email AS ContatoEmail,
            ct.Telefone AS ContatoTelefone,
            ct.Cargo AS ContatoCargo,
            ct.Empresa AS ContatoEmpresa
        FROM dbo.TBCompromisso AS c
        LEFT JOIN dbo.TBContato AS ct
            ON ct.Id = c.ContatoId
        ORDER BY c.DataOcorrencia, c.HoraInicio;
    """;

    private const string SelecionarCompromissoPorIdSql = """
        SELECT
            c.Id AS CompromissoId,
            c.Assunto,
            c.DataOcorrencia,
            c.HoraInicio,
            c.HoraTermino,
            c.Tipo,
            c.Local,
            c.Link,
            ct.Id AS ContatoId,
            ct.Nome AS ContatoNome,
            ct.Email AS ContatoEmail,
            ct.Telefone AS ContatoTelefone,
            ct.Cargo AS ContatoCargo,
            ct.Empresa AS ContatoEmpresa
        FROM dbo.TBCompromisso AS c
        LEFT JOIN dbo.TBContato AS ct
            ON ct.Id = c.ContatoId
        WHERE c.Id = @Id;
    """;

    public void Cadastrar(Compromisso entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirCompromissoSql, CriarParametros(entidade));
    }

    public bool Editar(Guid idSelecionado, Compromisso entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(AtualizarCompromissoSql, CriarParametros(entidadeAtualizada)) == 1;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(ExcluirCompromissoSql, new { Id = idSelecionado }) == 1;
    }

    public Compromisso? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        CompromissoRow? compromissoRow = conexao.QuerySingleOrDefault<CompromissoRow>(
            SelecionarCompromissoPorIdSql,
            new { Id = idSelecionado }
        );

        if (compromissoRow == null)
            return null;

        return MapearCompromisso(compromissoRow);
    }

    public List<Compromisso> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao
            .Query<CompromissoRow>(SelecionarTodosCompromissosSql)
            .Select(MapearCompromisso)
            .ToList();
    }

    public List<Compromisso> Filtrar(Predicate<Compromisso> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }

    private static object CriarParametros(Compromisso entidade)
    {
        return new
        {
            entidade.Id,
            entidade.Assunto,
            DataOcorrencia = entidade.DataOcorrencia.ToDateTime(TimeOnly.MinValue),
            HoraInicio = entidade.HoraInicio.ToTimeSpan(),
            HoraTermino = entidade.HoraTermino.ToTimeSpan(),
            Tipo = entidade.Tipo.ToString(),
            entidade.Local,
            entidade.Link,
            ContatoId = entidade.Contato?.Id
        };
    }

    private static Compromisso MapearCompromisso(CompromissoRow compromissoRow)
    {
        Contato? contato = compromissoRow.ContatoId.HasValue
            ? new Contato
            {
                Id = compromissoRow.ContatoId.Value,
                Nome = compromissoRow.ContatoNome ?? string.Empty,
                Email = compromissoRow.ContatoEmail ?? string.Empty,
                Telefone = compromissoRow.ContatoTelefone ?? string.Empty,
                Cargo = compromissoRow.ContatoCargo ?? string.Empty,
                Empresa = compromissoRow.ContatoEmpresa ?? string.Empty
            }
            : null;

        return new Compromisso
        {
            Id = compromissoRow.CompromissoId,
            Assunto = compromissoRow.Assunto,
            DataOcorrencia = DateOnly.FromDateTime(compromissoRow.DataOcorrencia),
            HoraInicio = TimeOnly.FromTimeSpan(compromissoRow.HoraInicio),
            HoraTermino = TimeOnly.FromTimeSpan(compromissoRow.HoraTermino),
            Tipo = Enum.Parse<TipoCompromisso>(compromissoRow.Tipo),
            Local = compromissoRow.Local ?? string.Empty,
            Link = compromissoRow.Link ?? string.Empty,
            Contato = contato
        };
    }
}

public sealed class CompromissoRow
{
    public Guid CompromissoId { get; set; }
    public string Assunto { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraTermino { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? Local { get; set; }
    public string? Link { get; set; }
    public Guid? ContatoId { get; set; }
    public string? ContatoNome { get; set; }
    public string? ContatoEmail { get; set; }
    public string? ContatoTelefone { get; set; }
    public string? ContatoCargo { get; set; }
    public string? ContatoEmpresa { get; set; }
}
