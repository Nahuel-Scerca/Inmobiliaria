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
    public class PagoController : Controller
    {
        private readonly RepositorioPago repositorio;
        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repoPropietario;
        private readonly RepositorioInquilino repoInquilinos;

        public PagoController(IConfiguration configuration)
        {
            this.repositorio = new RepositorioPago(configuration);
            this.repositorioContrato = new RepositorioContrato(configuration);
            this.repoPropietario = new RepositorioPropietario(configuration);
            this.repositorioInmueble = new RepositorioInmueble(configuration);
            this.repoInquilinos = new RepositorioInquilino(configuration);
        }




        // GET: PagoController
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PagoController/Create
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

        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PagoController/Edit/5
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

        // GET: PagoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagoController/Delete/5
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
