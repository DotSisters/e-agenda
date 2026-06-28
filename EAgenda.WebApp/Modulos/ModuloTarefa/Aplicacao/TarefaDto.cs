
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;

// public record ItemTarefaDto(
//     Guid Id,
//     string Titulo,
//     StatusConclusao Status
// );
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
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    StatusConclusao? Status
// List<string> Itens
);

public record DetalhesTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    StatusConclusao? Status,
    decimal PercentualConcluido
// List<ItemTarefaDto> Itens
);
