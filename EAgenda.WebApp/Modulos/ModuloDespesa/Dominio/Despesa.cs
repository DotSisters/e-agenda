using EAgenda.WebApp.Compartilhado.Dominio;
using EAgenda.WebApp.Modulos.ModuloCategoria.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;

public class Despesa : EntidadeBase<Despesa>
{
    public string Descricao { get; set; } = string.Empty;
    public DateTime Ocorrencia { get; set; }
    public decimal Valor { get; set; }
    public FormaPagamento Pagamento { get; set; }
    public List<Categoria> Categorias { get; set; } = new List<Categoria>();

    public Despesa()
    {
    }

    public Despesa(
        string descricao,
        DateTime ocorrencia,
        decimal valor,
        FormaPagamento pagamento,
        List<Categoria> categorias)
    {
        Descricao = descricao;
        Ocorrencia = ocorrencia;
        Valor = valor;
        Pagamento = pagamento;
        Categorias = categorias;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        ValidarDescricao(erros);
        ValidarValor(erros);
        ValidarCategoria(erros);

        return erros;
    }

    private void ValidarDescricao(List<string> erros)
    {
        if (string.IsNullOrWhiteSpace(Descricao))
            erros.Add("O campo \"Descrição\" deve ser preenchido.");

        else if (Descricao.Length < 2 || Descricao.Length > 100)
            erros.Add("O campo \"Descrição\" deve conter entre 2 e 100 caracteres.");
    }

    private void ValidarValor(List<string> erros)
    {
        if (Valor <= 0)
        {
            erros.Add("O campo \"Valor\" deve ser preenchido.");
        }
    }
    private void ValidarCategoria(List<string> erros)
    {
        if (Categorias == null || Categorias.Count == 0)
            erros.Add("É necessário selecionar pelo menos uma categoria.");
    }
    public override void Atualizar(Despesa entidadeAtualizada)
    {
        Despesa despesaAtualizada = entidadeAtualizada;

        Descricao = despesaAtualizada.Descricao;
        Ocorrencia = despesaAtualizada.Ocorrencia;
        Valor = despesaAtualizada.Valor;
        Pagamento = despesaAtualizada.Pagamento;
        Categorias = despesaAtualizada.Categorias;
    }
}
