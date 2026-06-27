using System.ComponentModel.DataAnnotations;
using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Apresentacao
{
    public record ListarDespesasViewModels(
        Guid Id,
        string Descricao,
        DateTime Ocorrencia,
        decimal Valor,
        FormaPagamento Pagamento,
        Guid CategoriaId,
        string CategoriaNome
    );

    public record CadastrarDespesaViewModels(
            [Required(ErrorMessage = "O campo \"Descrição\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Descrição\" deve conter entre 2 e 100 caracteres.")]
    string Descricao,

            DateTime? Ocorrencia,

            [Required(ErrorMessage = "O campo \"Valor\" deve ser preenchido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O campo \"Valor\" deve ser maior que zero.")]
    decimal Valor,

            [Required(ErrorMessage = "O campo \"Forma de Pagamento\" deve ser preenchido.")]
    FormaPagamento Pagamento,

            [Required(ErrorMessage = "É necessário selecionar pelo menos uma categoria.")]
    Guid CategoriaId
        );
}
