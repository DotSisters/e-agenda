using System.ComponentModel.DataAnnotations;

namespace EAgenda.WebApp.Modulos.ModuloContato.Apresentacao;

public record ListarContatosViewModels(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Cargo,
    string Empresa
);

public record CadastrarContatoViewModels(
    [Required(ErrorMessage = "O campo \"Contato\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Contato\" deve conter entre 2 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O campo \"Email\" deve ser preenchido.")]
    [EmailAddress(ErrorMessage = "O campo \"Email\" possui formato inválido.")]
    string Email,

    [Required(ErrorMessage = "O campo \"Telefone\" deve ser preenchido.")]
    [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$", ErrorMessage = "O campo \"Telefone\" deve conter entre 10 e 11 dígitos.")]
    string Telefone,

    [StringLength(100, ErrorMessage = "O campo \"Cargo\" deve conter no máximo 100 caracteres.")]
    string? Cargo,

    [StringLength(100, ErrorMessage = "O campo \"Empresa\" deve conter no máximo 100 caracteres.")]
    string? Empresa
);

public record EditarContatoViewModels(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Contato\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Contato\" deve conter entre 2 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O campo \"Email\" deve ser preenchido.")]
    [EmailAddress(ErrorMessage = "O campo \"Email\" possui formato inválido.")]
    string Email,

    [Required(ErrorMessage = "O campo \"Telefone\" deve ser preenchido.")]
    [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$", ErrorMessage = "O campo \"Telefone\" deve conter entre 10 e 11 dígitos.")]
    string Telefone,

    [StringLength(100, ErrorMessage = "O campo \"Cargo\" deve conter no máximo 100 caracteres.")]
    string? Cargo,

    [StringLength(100, ErrorMessage = "O campo \"Empresa\" deve conter no máximo 100 caracteres.")]
    string? Empresa
);

public record ExcluirContatoViewModels(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Cargo,
    string Empresa
);