using EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;
using FluentResults;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Aplicacao;

public class ServicoDespesa
{
    private readonly IRepositorioDespesa repositorioDespesa;
    private readonly IRepositorioCategoria repositorioCategoria;

    public ServicoDespesa(
        IRepositorioDespesa repositorioDespesa,
        IRepositorioCategoria repositorioCategoria
    )
    {
        this.repositorioDespesa = repositorioDespesa;
        this.repositorioCategoria = repositorioCategoria;
    }

    public Result Cadastrar(CadastrarDespesaDto dto)
    {
        Categoria? categoriaSelecionada = repositorioCategoria.SelecionarPorId(dto.CategoriaId);

        if (categoriaSelecionada == null)
            return Falha(nameof(dto.CategoriaId), "Selecione uma categoria válida.");

        DateTime dataOcorrencia = dto.Ocorrencia ?? DateTime.Now;

        Despesa novaDespesa = new Despesa(
            dto.Descricao,
            dataOcorrencia,
            dto.Valor,
            dto.Pagamento,
            categoriaSelecionada
        );

        Result resultadoValidacao = ValidarEntidade(novaDespesa);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioDespesa.Cadastrar(novaDespesa);

        return Result.Ok();
    }

    public List<ListarDespesasDto> SelecionarTodos()
    {
        return repositorioDespesa
            .SelecionarTodos()
            .Select(d => new ListarDespesasDto(
                d.Id,
                d.Descricao,
                d.Ocorrencia,
                d.Valor,
                d.Pagamento,
                d.Categoria.Id,
                d.Categoria.Titulo
            ))
            .ToList();
    }

    public Result<DetalhesDespesaDto> SelecionarPorId(Guid id)
    {
        Despesa? despesa = repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail("Despesa não encontrada.");

        return Result.Ok(new DetalhesDespesaDto(
            despesa.Id,
            despesa.Descricao,
            despesa.Ocorrencia,
            despesa.Valor,
            despesa.Pagamento,
            despesa.Categoria.Id
        ));
    }

    private static Result ValidarEntidade(Despesa despesa)
    {
        List<string> erros = despesa.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    public List<ListarCategoriasDto> SelecionarCategorias()
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Select(c => new ListarCategoriasDto(c.Id, c.Titulo))
            .ToList();
    }
}
