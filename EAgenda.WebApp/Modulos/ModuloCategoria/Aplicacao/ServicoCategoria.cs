using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;
using FluentResults;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

public class ServicoCategoria
{
    private readonly IRepositorioCategoria repositorioCategoria;

    private readonly IRepositorioDespesa repositorioDespesa;

    public ServicoCategoria(
        IRepositorioCategoria repositorioCategoria,
        IRepositorioDespesa repositorioDespesa
    )
    {
        this.repositorioCategoria = repositorioCategoria;
        this.repositorioDespesa = repositorioDespesa;
    }

    public Result Cadastrar(CadastrarCategoriaDto dto)
    {
        if (ExisteCategoriaTitulo(dto.Titulo))
            return Falha(nameof(dto.Titulo), "Já existe uma categoria com esse título.");

        Categoria novaCategoria = new Categoria(dto.Titulo);

        Result resultadoValidacao = ValidarEntidade(novaCategoria);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCategoria.Cadastrar(novaCategoria);

        return Result.Ok();
    }

    public Result Editar(EditarCategoriaDto dto)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(dto.Id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        if (ExisteCategoriaTitulo(dto.Titulo, dto.Id))
            return Falha(nameof(dto.Titulo), "Já existe uma categoria com esse título.");

        Categoria categoriaAtualizada = new Categoria(dto.Titulo);

        Result resultadoValidacao = ValidarEntidade(categoriaAtualizada);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCategoria.Editar(dto.Id, categoriaAtualizada);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        bool existeDespesa = repositorioDespesa
            .Filtrar(d => d.Categorias.Any(categoria => categoria.Id == id))
            .Any();

        if (existeDespesa)
            return Result.Fail("Não é permitido excluir categorias relacionadas a uma despesa.");

        repositorioCategoria.Excluir(id);

        return Result.Ok();
    }

    public List<ListarCategoriasDto> SelecionarTodos()
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Select(c => new ListarCategoriasDto(
                c.Id,
                c.Titulo,
                repositorioDespesa
                    .Filtrar(d => d.Categorias.Any(categoria => categoria.Id == c.Id))
                    .Sum(d => d.Valor)
            ))
            .ToList();
    }

    public Result<DetalhesCategoriasDto> SelecionarPorId(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        return Result.Ok(
            new DetalhesCategoriasDto(
                categoria.Id,
                categoria.Titulo
            )
        );
    }

    public Result<DetalhesCategoriaComDespesasDto> SelecionarPorIdComDespesas(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        List<DespesaPorCategoriaDto> despesas = repositorioDespesa
            .Filtrar(d => d.Categorias.Any(categoria => categoria.Id == id))
            .Select(d => new DespesaPorCategoriaDto(
                d.Id,
                d.Descricao,
                d.Ocorrencia,
                d.Valor,
                d.Pagamento
            ))
            .ToList();

        return Result.Ok(
            new DetalhesCategoriaComDespesasDto(
                categoria.Id,
                categoria.Titulo,
                despesas
            )
        );
    }

    private static Result ValidarEntidade(Categoria categoria)
    {
        List<string> erros = categoria.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    private string NormalizarTitulo(string titulo)
    {
        return titulo.Trim().ToLower();
    }

    private bool ExisteCategoriaTitulo(string titulo, Guid? idIgnorado = null)
    {
        string tituloNormalizado = NormalizarTitulo(titulo);

        return repositorioCategoria
            .SelecionarTodos()
            .Any(c =>
                c.Id != idIgnorado &&
                NormalizarTitulo(c.Titulo) == tituloNormalizado
            );
    }

}
