
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;

public record ItemTarefaDto(
    Guid Id,
    string Titulo,
    StatusConclusao Status
);

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
);

public record EditarTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    StatusConclusao? Status
);

public record DetalhesTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    StatusConclusao? Status,
    decimal PercentualConcluido
);

public record ListarItensTarefaDto(
    Guid TarefaId,
    string TarefaTitulo,
    StatusConclusao TarefaStatus,
    decimal PercentualConcluido,
    DateOnly? DataConclusao,
    List<ItemTarefaDto> Itens
);

public record AdicionarItemTarefaDto(
    Guid TarefaId,
    string Titulo
);
