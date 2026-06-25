using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;

public record ListarCompromissosDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string Local,
    string Link
);

public record CadastrarCompromissoDto(

);

public record EditarCompromissoDto(

);

public record DetalhesCompromissoDto(

);
