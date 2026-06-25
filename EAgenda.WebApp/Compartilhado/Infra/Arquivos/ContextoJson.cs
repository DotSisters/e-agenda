using System.Text.Json;
using System.Text.Json.Serialization;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;

namespace EAgenda.WebApp.Compartilhado.Infra.Arquivos;

public sealed class ContextoJson
{
    public List<Contato> Contatos { get; set; } = new List<Contato>();
    // public List<Compromisso> Compromissos { get; set; } = new List<Categoria>(); -- VERIFICAR
    // public List<Categoria> Categorias { get; set; } = new List<Categoria>();
    // public List<Despesa> Despesas { get; set; } = new List<Despesa>();
    // public List<Tarefa> Tarefa { get; set; } = new List<Tarefa>();

    private readonly string caminhoArquivo;

    public ContextoJson()
    {
        string caminhoAppData = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string caminhoDiretorio = Path.Combine(caminhoAppData, "EAgenda");

        Directory.CreateDirectory(caminhoDiretorio);

        caminhoArquivo = Path.Combine(caminhoDiretorio, "dados.json");
    }

    public void Salvar()
    {
        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.WriteIndented = true;
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;
        opcoesJson.Converters.Add(new JsonStringEnumConverter());

        string jsonString = JsonSerializer.Serialize(this, opcoesJson);

        File.WriteAllText(caminhoArquivo, jsonString);
    }

    public void Carregar()
    {
        if (!File.Exists(caminhoArquivo))
            return;

        string jsonString = File.ReadAllText(caminhoArquivo);

        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;
        opcoesJson.Converters.Add(new JsonStringEnumConverter());

        ContextoJson? contextoSalvo = JsonSerializer
            .Deserialize<ContextoJson>(jsonString, opcoesJson);

        if (contextoSalvo == null)
            return;

        // Compromissos = contextoSalvo.Compromissos;
        Contatos = contextoSalvo.Contatos;
        // Categorias = contextoSalvo.Categorias;
        // Despesas = contextoSalvo.Despesas;
        // Tarefa = contextoSalvo.Tarefa;
    }
}