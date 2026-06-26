using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;
using FluentResults;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;

public class ServicoCompromisso
{
    private readonly IRepositorioCompromisso repositorioCompromisso;
    private readonly IRepositorioContato repositorioContato;

    public ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso, IRepositorioContato repositorioContato)
    {
        this.repositorioCompromisso = repositorioCompromisso;
        this.repositorioContato = repositorioContato;
    }

    public Result Cadastrar(CadastrarCompromissoDto dto)
    {
        Contato? contatoSelecionado = null;

        if (dto.ContatoId.HasValue)
        {
            contatoSelecionado = repositorioContato.SelecionarPorId(dto.ContatoId.Value);

            if (contatoSelecionado is null)
                return Falha(nameof(dto.ContatoId), "Selecione um contato válido.");
        }

        if (dto.HoraTermino <= dto.HoraInicio)
            return Falha(nameof(dto.HoraInicio), "O horário de término deve ser após o horário de início.");

        if (ExisteCompromissoComMesmoHorario(dto.HoraInicio, dto.DataOcorrencia))
            return Falha(nameof(dto.HoraInicio), "Você já possui um compromisso neste horário.");

        bool isRemote = false;
        if (dto.Tipo == TipoCompromisso.Remoto) isRemote = true;

        Compromisso novoCompromisso = new Compromisso(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            isRemote,
            dto.Local ?? string.Empty,
            dto.Link ?? string.Empty,
            contatoSelecionado
        );

        Result resultadoValidacao = ValidarEntidade(novoCompromisso);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCompromisso.Cadastrar(novoCompromisso);

        return Result.Ok();
    }


    public Result Editar(EditarCompromissoDto dto)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(dto.Id);

        if (compromisso == null)
            return Result.Fail("Compromisso não encontrado.");

        Contato? contatoSelecionado = null;

        if (dto.ContatoId.HasValue)
        {
            contatoSelecionado = repositorioContato.SelecionarPorId(dto.ContatoId.Value);

            if (contatoSelecionado is null)
                return Falha(nameof(dto.ContatoId), "Selecione um contato válido.");
        }

        if (dto.HoraTermino <= dto.HoraInicio)
            return Falha(nameof(dto.HoraInicio), "O horário de término deve ser após o horário de início.");

        if (ExisteCompromissoComMesmoHorario(dto.HoraInicio, dto.DataOcorrencia))
            return Falha(nameof(dto.HoraInicio), "Você já possui um compromisso neste horário.");

        bool isRemote = false;
        if (dto.Tipo == TipoCompromisso.Remoto) isRemote = true;

        Compromisso compromissoAtualizado = new Compromisso(
            dto.Assunto,
            dto.DataOcorrencia,
            dto.HoraInicio,
            dto.HoraTermino,
            isRemote,
            dto.Local ?? string.Empty,
            dto.Link ?? string.Empty,
            contatoSelecionado
        );

        Result resultadoValidacao = ValidarEntidade(compromissoAtualizado);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCompromisso.Editar(dto.Id, compromissoAtualizado);

        return Result.Ok();
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
                m.Link,
                m.Contato?.Nome
            ))
            .ToList();
    }

    public Result<DetalhesCompromissoDto> SelecionarPorId(Guid id)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Compromisso nao encontrado.");

        return Result.Ok(new DetalhesCompromissoDto(
            compromisso.Id,
            compromisso.Assunto,
            compromisso.DataOcorrencia,
            compromisso.HoraInicio,
            compromisso.HoraTermino,
            compromisso.Tipo,
            compromisso.Local,
            compromisso.Link,
            compromisso.Contato?.Id,
            compromisso.Contato?.Nome
        ));
    }

    public List<OpcaoContatoDto> SelecionarContatos()
    {
        return repositorioContato
            .SelecionarTodos()
            .Select(f => new OpcaoContatoDto(f.Id, f.Nome))
            .ToList();
    }

    private bool ExisteCompromissoComMesmoHorario(TimeOnly horario, DateOnly data, Guid? idIgnorado = null)
    {
        return repositorioCompromisso
            .SelecionarTodos()
            .Any(c => c.Id != idIgnorado && c.DataOcorrencia == data && c.HoraInicio < horario && c.HoraTermino > horario
        );
    }

    private static Result ValidarEntidade(Compromisso compromisso)
    {
        List<string> erros = compromisso.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}
