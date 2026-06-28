using System;
using EAgenda.WebApp.Compartilhado.Infra.Arquivos;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Infra;

public class RepositorioDespesaEmArquivo : RepositorioBaseEmArquivo<Despesa>, IRepositorioDespesa
{
    public RepositorioDespesaEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Despesa> CarregarRegistros()
    {
        return contexto.Despesas;
    }
}
