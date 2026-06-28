using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Aplicacao;

public record ListarDespesasDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    string CategoriaNomes
);

public record CadastrarDespesaDto(
    string Descricao,
    DateTime? Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    List<Guid> CategoriaIds
);

public record EditarDespesaDto(
    Guid Id,
    string Descricao,
    DateTime? Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    List<Guid> CategoriaIds
);

public record ExcluirDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    string CategoriaNomes
);

public record DetalhesDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    List<Guid> CategoriaIds,
    string CategoriaNomes
);
