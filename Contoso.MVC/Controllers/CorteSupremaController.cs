using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contoso.MVC.WebServiceAccess.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.MVC.Controllers
{
    public class CorteSupremaController : Controller
    {
        private readonly IWebApCalls _webApCalls;

        public CorteSupremaController(IWebApCalls webApCalls)
        {
            _webApCalls = webApCalls;
        }
        // GET: CorteSuprema
        public async Task<ActionResult> Index()
        {
            List<int> listadoId = new List<int>();
            List<Models.DatosAbogadoDto> datosAbogadoDtos = new List<Models.DatosAbogadoDto>();
            listadoId.Add(28839);
            listadoId.Add(3762);
            listadoId.Add(6290);
            listadoId.Add(4588);
            listadoId.Add(8077);
            listadoId.Add(24293);
            listadoId.Add(8397);
            listadoId.Add(23548);
            listadoId.Add(18132);
            listadoId.Add(30153);
            //listadoId.Add(30221);

            foreach (int item in listadoId)
            {
                var retorno = await _webApCalls.datosAbogadoDto(item);
                datosAbogadoDtos.Add(retorno.FirstOrDefault());
            }

            
            return View(datosAbogadoDtos);
        }

        // GET: CorteSuprema/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CorteSuprema/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CorteSuprema/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CorteSuprema/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CorteSuprema/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CorteSuprema/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CorteSuprema/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}