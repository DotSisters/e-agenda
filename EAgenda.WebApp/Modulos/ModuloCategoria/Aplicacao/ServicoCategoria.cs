using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using FluentResults;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

public class ServicoCategoria
{
    private readonly IRepositorioCategoria repositorioCategoria;

    // private radonly IRepositorioDespesa repositorioDespesa;

    public ServicoCategoria(
        IRepositorioCategoria repositorioCategoria //, IRepositorioDespesa repositorioDespesa,
    )
    {
        this.repositorioCategoria = repositorioCategoria;
        // this.repositorioDespesa = repositorioDespesa;
    }

    public Result Cadastrar(CadastrarCategoriaDto dto)
    {
        // Despesa? despesaSelecionada = repositorioDespesa.SelecionarPorId(dto.DespesaId);

        // if (despesaSelecionada == null)
        //     return Falha(nameof(dto.DespesaId), "Selecione uma despesa válida.");

        if (ExisteCategoriaTitulo(dto.Titulo))
            return Falha(nameof(dto.Titulo), "Já existe uma categoria com esse título.");

        Categoria novaCategoria = new Categoria(
            dto.Titulo
        // despesaSelecionada
        );

        Result resultadoValidacao = ValidarEntidade(novaCategoria);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCategoria.Cadastrar(novaCategoria);

        return Result.Ok();
    }

    public List<ListarCategoriasDto> SelecionarTodos()
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Select(c => new ListarCategoriasDto(
            c.Id,
            c.Titulo
            // c.Despesa.Descricao,
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
                categoria.Titulo //,
                                 // categoria.Despesa.Id,
                                 // categoria.Despesa.Descricao
            )
        );
    }

    // public List<OpcaoDespesaDto> SelecionarDespesas()
    // {
    //     return repositorioDespesa
    //         .SelecionarTodos()
    //         .Select(d => new OpcaoDespesaDto(d.Id, d.Descricao, d.DataOcorrencia, d.Valor, d.FormaPagamento))
    //         .ToList();
    // }

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
