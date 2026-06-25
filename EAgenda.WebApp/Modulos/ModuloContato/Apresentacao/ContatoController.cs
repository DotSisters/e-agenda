using AutoMapper;
using EAgenda.WebApp.Compartilhado.Apresentacao.Extensions;
using EAgenda.WebApp.Modulos.ModuloContato.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EAgenda.WebApp.Modulos.ModuloContato.Apresentacao
{
    public class ContatoController(ServicoContato servicoContato, IMapper mapeador) : Controller
    {
        [HttpGet]
        public ActionResult Listar()
        {
            List<ListarContatosDto> dtos = servicoContato.SelecionarTodos();
            List<ListarContatosViewModels> listarVms = mapeador.Map<List<ListarContatosViewModels>>(dtos);

            return View(listarVms);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            CadastrarContatoViewModels cadastrarVm = new CadastrarContatoViewModels(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            );

            return View(cadastrarVm);
        }

        [HttpPost]
        public ActionResult Cadastrar(CadastrarContatoViewModels cadastrarVm)
        {
            if (!ModelState.IsValid)
                return View(cadastrarVm);

            CadastrarContatoDto dto = mapeador.Map<CadastrarContatoDto>(cadastrarVm);

            Result resultado = servicoContato.Cadastrar(dto);


            if (resultado.IsFailed)
            {
                ModelState.AddModelError(resultado);

                return View(cadastrarVm);
            }

            return RedirectToAction(nameof(Listar));
        }

    }
}
