
namespace EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

public record ListarCategoriasDto(
  Guid Id,
  string Titulo,
  decimal TotalDespesas = 0m
);

public record CadastrarCategoriaDto(
    string Titulo
);

public record EditarCategoriaDto(
    Guid Id,
    string Titulo
);

public record DetalhesCategoriasDto(
    Guid Id,
    string Titulo
);

public record DespesaPorCategoriaDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    EAgenda.WebApp.Modulos.ModuloDespesa.Dominio.FormaPagamento Pagamento
);

public record DetalhesCategoriaComDespesasDto(
    Guid Id,
    string Titulo,
    List<DespesaPorCategoriaDto> Despesas
);
