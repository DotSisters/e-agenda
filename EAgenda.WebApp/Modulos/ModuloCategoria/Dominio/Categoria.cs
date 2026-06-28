using EAgenda.WebApp.Compartilhado.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;

public class Categoria : EntidadeBase<Categoria>
{
    public string Titulo { get; set; } = string.Empty;

    public Categoria()
    {
    }

    public Categoria(string titulo)
    {
        Titulo = titulo;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        ValidarTitulo(erros);

        return erros;
    }

    private void ValidarTitulo(List<string> erros)
    {
        if (Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O campo \"Titulo\" deve conter entre 2 e 100 caracteres.");
    }

    public override void Atualizar(Categoria entidadeAtualizada)
    {
        Categoria categoriaAtualizada = entidadeAtualizada;

        Titulo = categoriaAtualizada.Titulo;
    }
}
