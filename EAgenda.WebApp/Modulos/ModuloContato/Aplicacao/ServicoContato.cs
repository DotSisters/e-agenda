using System;
using EAgenda.WebApp.Modulos.ModuloContato.Dominio;
using FluentResults;

namespace EAgenda.WebApp.Modulos.ModuloContato.Aplicacao;

public class ServicoContato
{
    private readonly IRepositorioContato repositorioContato;

    public ServicoContato(
        IRepositorioContato repositorioContato
    )
    {
        this.repositorioContato = repositorioContato;
    }

    public Result Cadastrar(CadastrarContatoDto dto)
    {
        string telefoneNormalizado = NormalizarTelefone(dto.Telefone);
        string emailNormalizado = NormalizarEmail(dto.Email);

        if (ExisteContatoTelefone(telefoneNormalizado))
            return Falha(nameof(dto.Telefone), "Já existe um contato com esse telefone.");

        if (ExisteContatoEmail(emailNormalizado))
            return Falha(nameof(dto.Email), "Já existe um contato com esse e-mail.");

        Contato novoContato = new Contato(dto.Nome, emailNormalizado, telefoneNormalizado, dto.Cargo, dto.Empresa);

        Result resultadoValidacao = ValidarEntidade(novoContato);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioContato.Cadastrar(novoContato);

        return Result.Ok();
    }

    public Result Editar(EditarContatoDto dto)
    {
        Contato? contato = repositorioContato.SelecionarPorId(dto.Id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        string telefoneNormalizado = NormalizarTelefone(dto.Telefone);
        string emailNormalizado = NormalizarEmail(dto.Email);

        if (ExisteContatoTelefone(telefoneNormalizado, dto.Id))
            return Falha(nameof(dto.Telefone), "Já existe um contato com esse telefone.");

        if (ExisteContatoEmail(emailNormalizado, dto.Id))
            return Falha(nameof(dto.Email), "Já existe um contato com esse e-mail.");

        Contato contatoAtualizado = new Contato(
            dto.Nome,
            emailNormalizado,
            telefoneNormalizado,
            dto.Cargo,
            dto.Empresa
        );

        Result resultadoValidacao = ValidarEntidade(contatoAtualizado);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioContato.Editar(dto.Id, contatoAtualizado);

        return Result.Ok();
    }

    public List<ListarContatosDto> SelecionarTodos()
    {
        return repositorioContato
            .SelecionarTodos()
            .Select(c => new ListarContatosDto(c.Id, c.Nome, c.Email, c.Telefone, c.Cargo, c.Empresa))
            .ToList();
    }

    public Result<DetalhesContatoDto> SelecionarPorId(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        return Result.Ok(
            new DetalhesContatoDto(
                contato.Id,
                contato.Nome,
                contato.Email,
                contato.Telefone,
                contato.Cargo,
                contato.Empresa
            )
        );
    }
    private static Result ValidarEntidade(Contato contato)
    {
        List<string> erros = contato.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    private string NormalizarEmail(string email)
    {
        return email.Trim().ToLower();
    }

    private bool ExisteContatoEmail(string email, Guid? idIgnorado = null)
    {
        string emailNormalizado = NormalizarEmail(email);

        return repositorioContato
            .SelecionarTodos()
            .Any(c =>
                c.Id != idIgnorado &&
                NormalizarEmail(c.Email) == emailNormalizado
            );
    }


    private string NormalizarTelefone(string telefone)
    {
        return Contato.RemoverFormatacao(telefone);
    }

    private bool ExisteContatoTelefone(string telefone, Guid? idIgnorado = null)
    {
        string telefoneNormalizado = NormalizarTelefone(telefone);

        return repositorioContato
            .SelecionarTodos()
            .Any(c =>
                c.Id != idIgnorado &&
                NormalizarTelefone(c.Telefone) == telefoneNormalizado
            );
    }

}
