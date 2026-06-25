
using EAgenda.WebApp.Compartilhado.Dominio;

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
    // public Contato? Contato { get; set; } = null;

    public Compromisso() { }
    public Compromisso(string assunto, DateOnly dataOcorrencia, TimeOnly inicio, TimeOnly termino, bool isRemote, string local, string link) : this()
    {
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = inicio;
        HoraTermino = termino;
        Tipo = isRemote ? TipoCompromisso.Remoto : TipoCompromisso.Presencial;
        Local = local;
        Link = link;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Assunto) || Assunto.Length < 2 || Assunto.Length > 100)
            erros.Add("O campo \"Assunto\" deve conter entre 2 e 100 caracteres.");

        if (!Enum.IsDefined(typeof(TipoCompromisso), Tipo))
            erros.Add("O campo \"Tipo de Compromisso\" deve ser Remoto ou Presencial.");

        return erros;
    }

    public override void Atualizar(Compromisso registroEditado)
    {
        Assunto = registroEditado.Assunto;
        DataOcorrencia = registroEditado.DataOcorrencia;
        HoraInicio = registroEditado.HoraInicio;
        HoraTermino = registroEditado.HoraTermino;
        Tipo = registroEditado.Tipo;
        Local = registroEditado.Local;
        Link = registroEditado.Link;
    }
}

