using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.Controllers
{
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repositorio;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repoPropietario;

        public ContratoController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioContrato(configuration);
            this.repoPropietario = new RepositorioPropietario(configuration);
            this.repositorioInmueble = new RepositorioInmueble(configuration);
        }



        // GET: ContratoController
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
