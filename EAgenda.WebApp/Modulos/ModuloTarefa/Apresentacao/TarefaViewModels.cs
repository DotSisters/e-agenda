using System.ComponentModel.DataAnnotations;
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Apresentacao;

public record ItemTarefaViewModel(
    Guid Id,
    string Titulo,
    StatusConclusao Status
);

public record ListarTarefasViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateOnly DataCriacao,
    DateOnly? DataConclusao,
    StatusConclusao Status,
    decimal PercentualConcluido
);

public record ListarTarefasPaginaViewModel(
    string Filtro,
    PrioridadeTarefa? Prioridade,
    List<ListarTarefasViewModel> Tarefas
);

public record CadastrarTarefaViewModel(
    [Required(ErrorMessage = "O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Titulo\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Required(ErrorMessage = "O campo \"Prioridade\" deve ser preenchido.")]
    PrioridadeTarefa Prioridade
);

public record EditarTarefaViewModel(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Titulo\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Required(ErrorMessage = "O campo \"Prioridade\" deve ser preenchido.")]
    PrioridadeTarefa Prioridade,

    StatusConclusao? Status
);

public record ExcluirTarefaViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    StatusConclusao? Status,
    decimal PercentualConcluido
);

public record CadastrarItemTarefaViewModel(
    Guid TarefaId,

    [Required(ErrorMessage = "O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Titulo\" deve conter entre 2 e 100 caracteres.")]
    string Titulo
);

public record ListarItensTarefaPaginaViewModel(
    Guid TarefaId,
    string TarefaTitulo,
    StatusConclusao TarefaStatus,
    decimal PercentualConcluido,
    DateOnly? DataConclusao,
    List<ItemTarefaViewModel> Itens,
    CadastrarItemTarefaViewModel NovoItem
);
