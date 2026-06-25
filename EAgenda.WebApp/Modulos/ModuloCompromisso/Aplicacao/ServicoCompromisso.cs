
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;

public class ServicoCompromisso
{
    private readonly IRepositorioCompromisso repositorioCompromisso;

    public ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso)
    {
        this.repositorioCompromisso = repositorioCompromisso;
    }

    public List<ListarCompromissosDto> SelecionarTodos()
    {
        return repositorioCompromisso
            .SelecionarTodos()
            .Select(m => new ListarCompromissosDto(
                m.Id,
                m.Assunto,
                m.DataOcorrencia,
                m.HoraInicio,
                m.HoraTermino,
                m.Tipo,
                m.Local,
                m.Link
            ))
            .ToList();
    }
}
