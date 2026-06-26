using System.ComponentModel.DataAnnotations;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao.Views;

public record OpcaoContatoViewModel(
    Guid Id,
    string Nome
);

public record ListarCompromissosViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string Local,
    string Link,
    string? ContatoNome
);

public record CadastrarCompromissoViewModel(
    [Required(ErrorMessage = "O campo \"Assunto\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Assunto\" precisa ter no mínimo 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Assunto\" precisa ter no máximo 100 caracteres.")]
    string Assunto,

    [Required(ErrorMessage = "O campo \"Data de Ocorrência\" é obrigatório.")]
    DateOnly DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Hora de Início\" é obrigatório.")]
    TimeOnly HoraInicio,

    [Required(ErrorMessage = "O campo \"Hora de Término\" é obrigatório.")]
    TimeOnly HoraTermino,

    [Required(ErrorMessage = "O campo \"Tipo do Compromisso\" é obrigatório.")]
    TipoCompromisso Tipo,

    string? Local,

    string? Link,

    [ValidateNever]
    List<OpcaoContatoViewModel> Contatos,

    Guid? ContatoId = null
);

public record EditarCompromissoViewModel(

);

public record ExcluirCompromissoViewModel(

);
