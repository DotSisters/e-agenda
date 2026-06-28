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
  public ActionResult Listar(string? filtro, PrioridadeTarefa? prioridade)
  {
    StatusConclusao? status = filtro switch
    {
      "pendentes" => StatusConclusao.Pendente,
      "concluidas" => StatusConclusao.Concluida,
      _ => null
    };

    PrioridadeTarefa? filtroPrioridade = filtro == "prioridade" ? prioridade : null;

    List<ListarTarefasDto> dtos = servicoTarefa.Listar(status, filtroPrioridade);
    List<ListarTarefasViewModel> tarefas = mapeador.Map<List<ListarTarefasViewModel>>(dtos);

    ListarTarefasPaginaViewModel pagina = new(filtro ?? string.Empty, filtroPrioridade, tarefas);

    return View(pagina);
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


  [HttpGet]
  public ActionResult Editar(Guid id)
  {
    Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(id);

    if (resultado.IsFailed)
    {
      TempData.AddErrorMessage(resultado);

      return RedirectToAction(nameof(Listar));
    }

    EditarTarefaViewModel editarVm =
        mapeador.Map<EditarTarefaViewModel>(resultado.Value);

    return View(editarVm);
  }


  [HttpPost]
  public ActionResult Editar(EditarTarefaViewModel editarVm)
  {
    if (!ModelState.IsValid)
      return View(editarVm);

    EditarTarefaDto dto = mapeador.Map<EditarTarefaDto>(editarVm);

    Result resultado = servicoTarefa.Editar(dto);

    if (resultado.IsFailed)
    {
      ModelState.AddModelError(resultado);

      return View(editarVm);
    }

    return RedirectToAction(nameof(Listar));
  }


  [HttpGet]
  public ActionResult Excluir(Guid id)
  {
    Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(id);

    if (resultado.IsFailed)
    {
      TempData.AddErrorMessage(resultado);

      return RedirectToAction(nameof(Listar));
    }

    ExcluirTarefaViewModel excluirVm = mapeador.Map<ExcluirTarefaViewModel>(resultado.Value);

    return View(excluirVm);
  }


  [HttpPost]
  public ActionResult Excluir(ExcluirTarefaViewModel excluirVm)
  {
    Result resultado = servicoTarefa.Excluir(excluirVm.Id);

    if (resultado.IsFailed)
      TempData.AddErrorMessage(resultado);

    return RedirectToAction(nameof(Listar));
  }
}
