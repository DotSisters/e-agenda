using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao.Views;
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
}
