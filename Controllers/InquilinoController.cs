using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationPrueba.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace WebApplicationPrueba.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly IRepositorioInquilino repositorio;
        private readonly IConfiguration configuration;

        public InquilinoController(IConfiguration configuration, IRepositorioInquilino repositorio)
        {
            this.configuration = configuration;
            this.repositorio = repositorio;
        }

        // GET: Inquilino
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilino/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inquilino/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                TempData["Nombre"] = inquilino.Nombre;
                if (ModelState.IsValid)
                {
                    repositorio.Alta(inquilino);
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(inquilino);
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            try
            {
                var p = repositorio.ObtenerPorId(id);
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View(p);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Inquilino/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino p = null;
            try
            {
                p = repositorio.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Email = collection["Email"];
                p.Telefono = collection["Telefono"];
                repositorio.Modificacion(p);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // POST: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }
    }
}
