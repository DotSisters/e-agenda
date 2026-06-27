using System.ComponentModel.DataAnnotations;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;

public record ListarCategoriasViewModels(
  Guid Id,
  string Titulo,
  decimal TotalDespesas
);

public record CadastrarCategoriaViewModels(
    [Required(ErrorMessage = "O campo \"Categoria\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Categoria\" deve conter entre 2 e 100 caracteres.")]
    string Titulo
);

public record EditarCategoriaViewModels(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Categoria\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Categoria\" deve conter entre 2 e 100 caracteres.")]
    string Titulo
);

public record ExcluirCategoriaViewModels(
    Guid Id,
    string Titulo
);

public record DespesaPorCategoriaViewModels(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento
);

public record CategoriaComDespesasViewModels(
    Guid Id,
    string Titulo,
    List<DespesaPorCategoriaViewModels> Despesas
);
