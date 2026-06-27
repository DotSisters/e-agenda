
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
    string Titulo,
    PrioridadeTarefa Prioridade
// List<string> Itens
);

public record EditarTarefaDto(
// Guid Id,

);

public record DetalhesTarefaDto(
// Guid Id,

);
