using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;
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
}
