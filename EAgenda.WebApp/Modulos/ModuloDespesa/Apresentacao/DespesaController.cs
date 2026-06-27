using AutoMapper;
using EAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using EAgenda.WebApp.Modulos.ModuloDespesa.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloDespesa.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Apresentacao;

public class DespesaController(ServicoDespesa servicoDespesa, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarDespesasDto> dtos = servicoDespesa.SelecionarTodos();
        List<ListarDespesasViewModels> listarVms = mapeador.Map<List<ListarDespesasViewModels>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        ViewBag.Categorias = new SelectList(
            servicoDespesa.SelecionarCategorias(),
            "Id",
            "Titulo"
        );

        CadastrarDespesaViewModels cadastrarVm = new CadastrarDespesaViewModels(
            string.Empty,
            DateTime.Now,
            0m,
            FormaPagamento.AVista,
            Guid.Empty
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarDespesaViewModels cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarDespesaDto dto = mapeador.Map<CadastrarDespesaDto>(cadastrarVm);

        Result resultado = servicoDespesa.Cadastrar(dto);


        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesDespesaDto> resultado = servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
            return RedirectToAction(nameof(Listar));

        EditarDespesaViewModels editarVm = mapeador.Map<EditarDespesaViewModels>(resultado.Value);

        if (!editarVm.Ocorrencia.HasValue)
            editarVm = editarVm with { Ocorrencia = DateTime.Today };

        ViewBag.Categorias = new SelectList(
            servicoDespesa.SelecionarCategorias(),
            "Id",
            "Titulo",
            editarVm.CategoriaId
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarDespesaViewModels editarVm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categorias = new SelectList(
                servicoDespesa.SelecionarCategorias(),
                "Id",
                "Titulo",
                editarVm.CategoriaId
            );

            return View(editarVm);
        }

        EditarDespesaDto dto = mapeador.Map<EditarDespesaDto>(editarVm);

        Result resultado = servicoDespesa.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            ViewBag.Categorias = new SelectList(
                servicoDespesa.SelecionarCategorias(),
                "Id",
                "Titulo",
                editarVm.CategoriaId
            );

            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

}
