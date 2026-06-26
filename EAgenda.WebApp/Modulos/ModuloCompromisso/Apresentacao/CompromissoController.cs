using AutoMapper;
using EAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao.Views;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao;

public class CompromissoController(ServicoCompromisso servicoCompromisso, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCompromissosDto> dtos = servicoCompromisso.SelecionarTodos();
        List<ListarCompromissosViewModel> listarVms = mapeador.Map<List<ListarCompromissosViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCompromissoViewModel cadastrarVm = new CadastrarCompromissoViewModel(
            string.Empty,
            DateOnly.FromDateTime(DateTime.Today),
            TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
            TipoCompromisso.Presencial,
            string.Empty,
            string.Empty,
            SelecionarContatos()
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVm)
    {

        if (!ModelState.IsValid)
            return View(cadastrarVm with { Contatos = SelecionarContatos() });

        CadastrarCompromissoDto dto = mapeador.Map<CadastrarCompromissoDto>(cadastrarVm);
        Result resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm with { Contatos = SelecionarContatos() });
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesCompromissoDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        EditarCompromissoViewModel editarVm =
            mapeador.Map<EditarCompromissoViewModel>(resultado.Value) with { Contatos = SelecionarContatos() };

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCompromissoViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm with { Contatos = SelecionarContatos() });

        EditarCompromissoDto dto = mapeador.Map<EditarCompromissoDto>(editarVm);
        Result resultado = servicoCompromisso.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(editarVm with { Contatos = SelecionarContatos() });
        }

        return RedirectToAction(nameof(Listar));
    }

    private List<OpcaoContatoViewModel> SelecionarContatos()
    {
        List<OpcaoContatoDto> dtos = servicoCompromisso.SelecionarContatos();

        return mapeador.Map<List<OpcaoContatoViewModel>>(dtos);
    }
}
