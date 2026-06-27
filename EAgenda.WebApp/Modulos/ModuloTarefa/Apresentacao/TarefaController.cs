using AutoMapper;
using EAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloTarefa.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Apresentacao;

public class TarefaController(ServicoTarefa servicoTarefa, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarTarefasDto> dtos = servicoTarefa.SelecionarTodos();
        List<ListarTarefasViewModel> listarVms = mapeador.Map<List<ListarTarefasViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarTarefaViewModel cadastrarVm = new CadastrarTarefaViewModel(
            string.Empty,
            PrioridadeTarefa.Normal
        // []
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarTarefaViewModel cadastrarVm)
    {
        // List<string> itens = cadastrarVm.Itens
        //     .Where(i => !string.IsNullOrWhiteSpace(i))
        //     .ToList();

        // if (!ModelState.IsValid)
        //     return View(cadastrarVm with { Itens = itens });

        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarTarefaDto dto = mapeador.Map<CadastrarTarefaDto>(cadastrarVm);

        Result resultado = servicoTarefa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            // return View(cadastrarVm with { Itens = itens });
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }
}
