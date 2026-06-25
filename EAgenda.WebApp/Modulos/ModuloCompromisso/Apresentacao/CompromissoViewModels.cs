
using System.ComponentModel.DataAnnotations;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao.Views;

public record ListarCompromissosViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string Local,
    string Link
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

    [Required(ErrorMessage = "O campo \"Local\" é obrigatório.")]
    string Local,

    [Required(ErrorMessage = "O campo \"Local ou Link\" é obrigatório.")]
    string Link
);

public record EditarCompromissoViewModel(

);

public record ExcluirCompromissoViewModel(

);
