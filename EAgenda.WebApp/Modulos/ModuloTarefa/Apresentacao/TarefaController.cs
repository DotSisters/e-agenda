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
    );

    return View(cadastrarVm);
  }


  [HttpPost]
  public ActionResult Cadastrar(CadastrarTarefaViewModel cadastrarVm)
  {
    if (!ModelState.IsValid)
      return View(cadastrarVm);

    CadastrarTarefaDto dto = mapeador.Map<CadastrarTarefaDto>(cadastrarVm);

    Result resultado = servicoTarefa.Cadastrar(dto);

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


  [HttpGet]
  public ActionResult Itens(Guid id)
  {
    Result<ListarItensTarefaDto> resultado = servicoTarefa.ListarItens(id);

    if (resultado.IsFailed)
    {
      TempData.AddErrorMessage(resultado);

      return RedirectToAction(nameof(Listar));
    }

    ListarItensTarefaPaginaViewModel pagina = MapearItensPagina(resultado.Value);

    return View(pagina);
  }


  [HttpPost]
  public ActionResult AdicionarItem([Bind(Prefix = "NovoItem")] CadastrarItemTarefaViewModel cadastrarVm)
  {
    if (!ModelState.IsValid)
    {
      Result<ListarItensTarefaDto> resultado = servicoTarefa.ListarItens(cadastrarVm.TarefaId);

      if (resultado.IsFailed)
      {
        TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
      }

      ListarItensTarefaPaginaViewModel pagina = MapearItensPagina(resultado.Value);

      pagina = pagina with { NovoItem = cadastrarVm };

      return View(nameof(Itens), pagina);
    }

    AdicionarItemTarefaDto dto = mapeador.Map<AdicionarItemTarefaDto>(cadastrarVm);

    Result resultadoAdicionar = servicoTarefa.AdicionarItem(dto);

    if (resultadoAdicionar.IsFailed)
    {
      Result<ListarItensTarefaDto> resultado = servicoTarefa.ListarItens(cadastrarVm.TarefaId);

      ListarItensTarefaPaginaViewModel pagina = MapearItensPagina(resultado.Value);

      pagina = pagina with { NovoItem = cadastrarVm };

      ModelState.AddModelError(resultadoAdicionar);

      return View(nameof(Itens), pagina);
    }

    return RedirectToAction(nameof(Itens), new { id = cadastrarVm.TarefaId });
  }


  [HttpPost]
  public ActionResult RemoverItem(Guid tarefaId, Guid itemId)
  {
    Result resultado = servicoTarefa.RemoverItem(tarefaId, itemId);

    if (resultado.IsFailed)
      TempData.AddErrorMessage(resultado);

    return RedirectToAction(nameof(Itens), new { id = tarefaId });
  }


  [HttpPost]
  public ActionResult ConcluirItem(Guid tarefaId, Guid itemId)
  {
    Result resultado = servicoTarefa.ConcluirItem(tarefaId, itemId);

    if (resultado.IsFailed)
      TempData.AddErrorMessage(resultado);

    return RedirectToAction(nameof(Itens), new { id = tarefaId });
  }


  [HttpPost]
  public ActionResult ReabrirItem(Guid tarefaId, Guid itemId)
  {
    Result resultado = servicoTarefa.ReabrirItem(tarefaId, itemId);

    if (resultado.IsFailed)
      TempData.AddErrorMessage(resultado);

    return RedirectToAction(nameof(Itens), new { id = tarefaId });
  }

  private ListarItensTarefaPaginaViewModel MapearItensPagina(ListarItensTarefaDto dto)
  {
    return new ListarItensTarefaPaginaViewModel(
        dto.TarefaId,
        dto.TarefaTitulo,
        dto.TarefaStatus,
        dto.PercentualConcluido,
        dto.DataConclusao,
        mapeador.Map<List<ItemTarefaViewModel>>(dto.Itens),
        new CadastrarItemTarefaViewModel(dto.TarefaId, string.Empty)
    );
  }
}
