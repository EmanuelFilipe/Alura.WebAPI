using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.WebApp.Models;
using Alura.ListaLeitura.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly LivroApiClient _api;

        public HomeController(LivroApiClient api)
        {
            _api = api;
        }

        private async Task<IEnumerable<LivroApi>> ListaDoTipo(TipoListaLeitura tipo)
        {
            var lista = await _api.GetListaLeituraAsync(tipo);
            return lista.Livros;
        }

        public async Task<IActionResult> Index()
        {

            //mas como recuperar?


            //problema é que não tenho a propriedade HttpContext na classe LivroApiClient. E agora?



            var model = new HomeViewModel
            {
                ParaLer = await ListaDoTipo(TipoListaLeitura.ParaLer),
                Lendo = await ListaDoTipo(TipoListaLeitura.Lendo),
                Lidos = await ListaDoTipo(TipoListaLeitura.Lidos)
            };
            return View(model);
        }
    }
}