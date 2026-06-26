using System;
using Microsoft.Extensions.Options;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

public record ListarCategoriasDto(
  Guid Id,
  string Titulo //,
//   string DespesaDescricao
);

public record CadastrarCategoriaDto(
    string Titulo
// Guid DespesaId
);

public record DetalhesCategoriasDto(
    Guid Id,
    string Titulo //,
                  // Guid DespesaId,
                  //string DespesaDescricao
);

// public record OpcaoDespesaDto(
//     Guid Id,
//     string Descricao,
//     DataOcorrencia,
//     Valor,
//     FormaPagamento
// ); ?????