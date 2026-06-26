
using EAgenda.WebApp.Compartilhado.Dominio;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;

public class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateOnly DataOcorrencia { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public Contato? Contato { get; set; } = null;

    public Compromisso() { }
    public Compromisso(string assunto, DateOnly dataOcorrencia, TimeOnly inicio, TimeOnly termino, bool isRemote, string local, string link, Contato? contato) : this()
    {
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = inicio;
        HoraTermino = termino;
        Tipo = isRemote ? TipoCompromisso.Remoto : TipoCompromisso.Presencial;
        Local = local;
        Link = link;
        Contato = contato;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Assunto) || Assunto.Length < 2 || Assunto.Length > 100)
            erros.Add("O campo \"Assunto\" deve conter entre 2 e 100 caracteres.");

        if (DataOcorrencia < DateOnly.FromDateTime(DateTime.Today))
            erros.Add("O campo \"Data de Ocorrência\" não pode ser anterior à data atual.");

        if (!Enum.IsDefined(typeof(TipoCompromisso), Tipo))
            erros.Add("O campo \"Tipo de Compromisso\" deve ser Remoto ou Presencial.");

        if (Tipo == TipoCompromisso.Presencial && string.IsNullOrWhiteSpace(Local))
            erros.Add("O campo \"Local\" é obrigatório para compromissos presenciais.");

        if (Tipo == TipoCompromisso.Remoto && string.IsNullOrWhiteSpace(Link))
            erros.Add("O campo \"Link\" é obrigatório para compromissos remotos.");

        return erros;
    }

    public override void Atualizar(Compromisso entidadeAtualizada)
    {
        Assunto = entidadeAtualizada.Assunto;
        DataOcorrencia = entidadeAtualizada.DataOcorrencia;
        HoraInicio = entidadeAtualizada.HoraInicio;
        HoraTermino = entidadeAtualizada.HoraTermino;
        Tipo = entidadeAtualizada.Tipo;
        Local = entidadeAtualizada.Local;
        Link = entidadeAtualizada.Link;
        Contato = entidadeAtualizada.Contato;
    }
}

