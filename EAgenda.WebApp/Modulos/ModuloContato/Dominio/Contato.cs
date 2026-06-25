using System;
using System.Net.Mail;
using EAgenda.WebApp.Compartilhado.Dominio;

namespace EAgenda.WebApp.Modulos.ModuloContato.Dominio;

public class Contato : EntidadeBase<Contato>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;

    public Contato()
    {
    }

    public Contato(string nome, string email, string telefone, string cargo, string empresa)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        ValidarNome(erros);
        ValidarEmail(erros);
        ValidarTelefone(erros);
        ValidarCargo(erros);
        ValidarEmpresa(erros);

        return erros;
    }

    private void ValidarNome(List<string> erros)
    {
        if (Nome.Length < 2 || Nome.Length > 100)
            erros.Add("O campo \"Nome\" deve conter entre 3 e 100 caracteres.");
    }

    private void ValidarEmail(List<string> erros)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            erros.Add("O campo \"Email\" é obrigatório.");
            return;
        }

        try
        {
            MailAddress endereco = new MailAddress(Email);

            // opcional: garantir que o endereço informado seja exatamente igual ao normalizado
            if (endereco.Address != Email)
                erros.Add("O campo \"Email\" possui formato inválido.");
        }
        catch
        {
            erros.Add("O campo \"Email\" possui formato inválido.");
        }
    }

    private void ValidarTelefone(List<string> erros)
    {
        string telefoneEncurtado = RemoverFormatacao(Telefone);

        if (telefoneEncurtado.StartsWith("0"))
            telefoneEncurtado = telefoneEncurtado.Substring(1);

        bool telefoneValido = true;

        if (telefoneEncurtado.Length < 10 || telefoneEncurtado.Length > 11)
        {
            erros.Add("O campo \"Telefone\" deve conter entre 10 e 11 dígitos.");
            telefoneValido = false;
        }

        if (!ContemSomenteDigitos(telefoneEncurtado))
        {
            erros.Add("O campo \"Telefone\" deve conter apenas dígitos.");
            telefoneValido = false;
        }

        if (telefoneValido)
        {
            if (telefoneEncurtado.Length == 10)
            {
                Telefone = Convert.ToUInt64(telefoneEncurtado)
                    .ToString(@"\(00\) 0000\-0000");
            }
            else
            {
                Telefone = Convert.ToUInt64(telefoneEncurtado)
                    .ToString(@"\(00\) 00000\-0000");
            }
        }
    }

    private void ValidarCargo(List<string> erros)
    {
        if (!string.IsNullOrWhiteSpace(Cargo) && Cargo.Length > 100)
            erros.Add("O campo \"Cargo\" deve conter no máximo 100 caracteres.");
    }

    private void ValidarEmpresa(List<string> erros)
    {
        if (!string.IsNullOrWhiteSpace(Empresa) && Empresa.Length > 100)
            erros.Add("O campo \"Empresa\" deve conter no máximo 100 caracteres.");
    }

    public override void Atualizar(Contato entidadeAtualizada)
    {
        Contato contatoAtualizado = entidadeAtualizada;

        Nome = contatoAtualizado.Nome;
        Email = contatoAtualizado.Email;
        Telefone = contatoAtualizado.Telefone;
    }

    private bool ContemSomenteDigitos(string valor)
    {
        for (int i = 0; i < valor.Length; i++)
        {
            if (!char.IsDigit(valor[i]))
                return false;
        }

        return true;
    }

    public static string RemoverFormatacao(string valor)
    {
        return valor
            .Replace(" ", "")
            .Replace("-", "")
            .Replace(".", "")
            .Replace("/", "")
            .Replace("(", "")
            .Replace(")", "");
    }

}
