using Microsoft.Data.SqlClient;

namespace EAgenda.WebApp.Compartilhado.Infra.Sql;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}

public sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    private const string NomeConnectionString = "EAgenda";


    // ConnectionString = Endereço do banco de dados local/remoto que vamos usar
    public SqlConnection CreateConnection()
    {
        string? connectionString = configuration.GetConnectionString(NomeConnectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"A connection string {NomeConnectionString} não foi encontrada."
            );
        }

        return new SqlConnection(connectionString);
    }
}