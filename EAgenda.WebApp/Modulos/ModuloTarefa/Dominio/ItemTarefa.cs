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
    }
}
