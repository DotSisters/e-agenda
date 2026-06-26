using AutoMapper;
using EAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;

public class CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCategoriasDto> dtos = servicoCategoria.SelecionarTodos();
        List<ListarCategoriasViewModels> listarVms = mapeador.Map<List<ListarCategoriasViewModels>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCategoriaViewModels cadastrarVm = new CadastrarCategoriaViewModels(
            string.Empty
        // SelecionarDespesas()

        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCategoriaViewModels cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarCategoriaDto dto = mapeador.Map<CadastrarCategoriaDto>(cadastrarVm);

        Result resultado = servicoCategoria.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
        // if (!ModelState.IsValid)
        //     return View(cadastrarVm with { Despesas = SelecionarDespesas() });
        // CadastrarCategoriaDto dto = mapeador.Map<CadastrarCategoriaDto>(cadastrarVm);

        // Result resultado = servicoCategoria.Cadastrar(dto);

        // if (resultado.IsFailed)
        // {
        //     ModelState.AddModelError(resultado);

        //     return View(cadastrarVm with { Despesas = SelecionarDespesas() });
        // }

        // return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesCategoriasDto> resultado = servicoCategoria.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        EditarCategoriaViewModels editarVm =
            mapeador.Map<EditarCategoriaViewModels>(resultado.Value);

        return View(editarVm);
    }
    // {
    //     Result<DetalhesCategoriaDto> resultado = servicoCategoria.SelecionarPorId(id);

    //     if (resultado.IsFailed)
    //     {
    //         TempData.AddErrorMessage(resultado);

    //         return RedirectToAction(nameof(Listar));
    //     }

    //     // EditarCategoriaViewModels editarVm =
    //     //     mapeador.Map<EditarCategoriaViewModels>(resultado.Value) with { Despesas = SelecionarDespesas() };

    //     return View(editarVm);
    // }

    [HttpPost]
    public ActionResult Editar(EditarCategoriaViewModels editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        EditarCategoriaDto dto = mapeador.Map<EditarCategoriaDto>(editarVm);

        Result resultado = servicoCategoria.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    // {
    //     // if (!ModelState.IsValid)
    //     //     return View(editarVm with { Despesas = SelecionarDespesas() });

    //     EditarCategoriaDto dto = mapeador.Map<EditarCategoriaDto>(editarVm);

    //     Result resultado = servicoCategoria.Editar(dto);

    //     if (resultado.IsFailed)
    //     {
    //         ModelState.AddModelError(resultado);

    //         return View(editarVm with { Despesas = SelecionarDespesas() });
    //     }

    //     return RedirectToAction(nameof(Listar));
    // }


}
