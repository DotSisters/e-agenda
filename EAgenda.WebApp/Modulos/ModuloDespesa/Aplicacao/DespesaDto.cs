using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Aplicacao;

public record ListarDespesasDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    Guid CategoriaId,
    string CategoriaNome
);

public record CadastrarDespesaDto(
    string Descricao,
    DateTime? Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    Guid CategoriaId
);

public record EditarDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    Guid CategoriaId
);

public record DetalhesDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Ocorrencia,
    decimal Valor,
    FormaPagamento Pagamento,
    Guid CategoriaId

);
