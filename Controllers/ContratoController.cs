using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.Controllers
{
    public class ContratoController : Controller
    {
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioInmueble repositorioInmueble;
        private readonly IRepositorioPropietario repoPropietario;
        private readonly IRepositorioInquilino repoInquilinos;

        public ContratoController(IConfiguration configuration, IRepositorioContrato repositorio, IRepositorioInmueble repositorioInmueble, IRepositorioPropietario repoPropietario, IRepositorioInquilino repoInquilinos)
        {
            this.repositorio = repositorio;
            this.repoPropietario = repoPropietario;
            this.repositorioInmueble = repositorioInmueble;
            this.repoInquilinos = repoInquilinos;
        }



        // GET: ContratoController
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }


        [Route("[controller]/BuscarPorFecha/{desde}/{hasta}", Name ="Buscar")]
        public IActionResult BuscarPorFecha(DateTime desde, DateTime hasta)
        {
            try
            {
                var res = repositorio.ObtenerPorFecha(desde,hasta);
                return Json(new {Datos=res});
            }
            catch (Exception ex)
            {
                return Json(new {Error= ex.Message });
            }
        }


            // GET: ContratoController/Details/5
            [Authorize]
        public ActionResult Details(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: ContratoController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repoInquilinos.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View();
        }

        // POST: ContratoController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(contrato);
                    TempData["Id"] = contrato.Id;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Inquilinos = repoInquilinos.ObtenerTodos();
                    ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                    return View(contrato);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(contrato);
            }
        }

        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            ViewBag.Inquilinos = repoInquilinos.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: ContratoController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato entidad)
        {
            try
            {
                entidad.Id = id;
                repositorio.Modificacion(entidad);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Inquilinos = repoInquilinos.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }
    }
}
