using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Apresentacao;

public record ListarTarefasViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateOnly DataCriacao,
    DateOnly? DataConclusao,
    StatusConclusao Status,
    decimal PercentualConcluido
);

public record CadastrarTarefaViewModel(

);

public record EditarTarefaViewModel(
// Guid Id,

);

public record ExcluirTarefaViewModel(
// Guid Id,

);
