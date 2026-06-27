
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;

public record ListarTarefasDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateOnly DataCriacao,
    DateOnly? DataConclusao,
    StatusConclusao Status,
    decimal PercentualConcluido

);

public record CadastrarTarefaDto(
// Guid Id,

);

public record EditarTarefaDto(
// Guid Id,

);

public record DetalhesTarefaDto(
// Guid Id,

);
