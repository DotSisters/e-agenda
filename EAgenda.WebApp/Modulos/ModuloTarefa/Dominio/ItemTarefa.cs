namespace EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

public class ItemTarefa
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Titulo { get; set; } = string.Empty;
    public StatusConclusao Status { get; set; }

    public ItemTarefa() { }

    public ItemTarefa(string titulo)
    {
        Titulo = titulo;
        Status = StatusConclusao.Pendente;
    }

    public List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O campo \"Título\" deve conter entre 2 e 100 caracteres.");

        if (!Enum.IsDefined(typeof(StatusConclusao), Status))
            erros.Add("O campo \"Status de Conclusão\" deve ser Pendente ou Concluída.");

        return erros;
    }
}
