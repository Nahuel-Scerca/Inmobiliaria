using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using WebApplicationPrueba.Models;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IRepositorioPropietario repositorio;
        private readonly IConfiguration config;

        public PropietariosController(IConfiguration configuration , IRepositorioPropietario repositorio)
        {
            this.repositorio = repositorio;
            this.config = configuration;
        }

        // GET: Propietario
        public ActionResult Index()
        {
            try{
                var lista = repositorio.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            }
            catch(Exception ex){

                throw;
            }
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Propietario/Buscar/5
        [Route("[controller]/Buscar/{q}", Name = "Buscar")]
        
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = repositorio.BuscarPorNombre(q);
                return Json(new { Datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }
        

        // GET: Propietario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    /*propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));*/
                    repositorio.Alta(propietario);
                    TempData["Id"] = propietario.Id;
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(propietario);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(propietario);
            }
        }

        // GET: Propietario/Edit/5
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

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                p = repositorio.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Email = collection["Email"];
                p.Clave = collection["Clave"];
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

       

        // GET: Propietario/Delete/5
        public ActionResult Eliminar(int id)
        {
            /*
            var p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
            */
            repositorio.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Propietario entidad)
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