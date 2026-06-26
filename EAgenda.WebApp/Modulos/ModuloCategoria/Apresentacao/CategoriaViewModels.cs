using System.ComponentModel.DataAnnotations;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;

// public record OpcaoDespesaViewModels(
//     string Id,
//     string Descricao
// );

public record ListarCategoriasViewModels(
  Guid Id,
  string Titulo //,
//   string DespesaDescricao
);

public record CadastrarCategoriaViewModels(
    [Required(ErrorMessage = "O campo \"Categoria\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo \"Categoria\" deve conter entre 3 e 100 caracteres.")]
    string Titulo

// [Required(ErrorMessage = "O campo \"Despesa\" deve ser preenchido.")]
// string DespesaId,

// [ValidateNever] List<OpcaoDespesaViewModels> Despesas
);