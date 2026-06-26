using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;

public record OpcaoContatoDto(
    Guid Id,
    string Nome
);
public record ListarCompromissosDto(
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

public record CadastrarCompromissoDto(
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId
);

public record EditarCompromissoDto(

);

public record DetalhesCompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string Local,
    string Link,
    Guid? ContatoId,
    string? ContatoNome
);
