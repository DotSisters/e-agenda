using Microsoft.AspNetCore.Mvc;

namespace EAgenda.WebApp.Compartilhado.Apresentacao;

public class HomeController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }
}