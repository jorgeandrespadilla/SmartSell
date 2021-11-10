using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ProyectoFinal.Web.Infrastructure;
using ProyectoFinal.Web.Models;
using ProyectoFinal.Web.ViewModels;

namespace ProyectoFinal.Web.Controllers
{
    [AuthenticationFilter]
    public class OfertasController : Controller
    {
        private SmartSell db = new SmartSell();

        // GET: Ofertas
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            /* Manejar configuración de búsqueda y paginación */
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            int usuarioId = Convert.ToInt32(HttpContext.Session["UserID"]);
            var ofertasQuery = db.Oferta.Where(o => o.UsuarioID == usuarioId).OrderByDescending(o => o.FechaCreacion).ToList();
            /* Manejar cadena de búsqueda */
            if (!String.IsNullOrEmpty(searchString))
            {
                ofertasQuery = ofertasQuery.Where(s => s.Subasta.NombreProducto.ToLower().Contains(searchString.ToLower())).ToList();
            }

            int pageSize = 10;
            int paginasTotales = Convert.ToInt32(ofertasQuery.Count() / pageSize) + 1;
            if (page != null && (page < 1 || page > paginasTotales))
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);

            return View(ofertasQuery.ToPagedList(pageNumber, pageSize));
        }

        // GET: Ofertas/Create
        public ActionResult Create(int subastaID)
        {
            Subasta subasta = db.Subasta.Find(subastaID);
            if (!subasta.Usuario.Activo)
            {
                return HttpNotFound();
            }
            if (DateTime.Compare(DateTime.Now, subasta.FechaLimite) >= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userID = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (userID == subasta.UsuarioID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new OfertaCreateViewModel
            {
                SubastaID = subastaID
            });
        }

        // POST: Ofertas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OfertaCreateViewModel oferta)
        {
            if (ModelState.IsValid)
            {
                if (oferta.Monto <= 0)
                {
                    ModelState.AddModelError("generalError", "El monto debe ser positivo.");
                }
                Oferta ofertaActual = db.Oferta.Where(m => m.SubastaID == oferta.SubastaID).OrderByDescending(o => o.Monto).FirstOrDefault();
                if (ofertaActual == null)
                {
                    Subasta subasta = db.Subasta.Find(oferta.SubastaID);
                    if (oferta.Monto < subasta.PrecioInicial)
                    {
                        ModelState.AddModelError("generalError", "El monto es inferior al precio inicial");
                        return View(oferta);
                    }
                }
                else if (oferta.Monto <= ofertaActual.Monto)
                {
                    ModelState.AddModelError("generalError", "El monto es inferior o igual al monto actual");
                    return View(oferta);
                }
                db.Oferta.Add(new Oferta
                {
                    UsuarioID = Convert.ToInt32(HttpContext.Session["UserID"]),
                    SubastaID = oferta.SubastaID,
                    Monto = oferta.Monto,
                    FechaCreacion = DateTime.Now

                });
                db.SaveChanges();
                return RedirectToAction("Details", "Subastas", new { id = oferta.SubastaID });
            }
            return View();
        }

        // GET: Ofertas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oferta oferta = db.Oferta.Find(id);
            if (oferta == null)
            {
                return HttpNotFound();
            }
            return View(oferta);
        }

        // POST: Ofertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Oferta oferta = db.Oferta.Find(id);
            db.Oferta.Remove(oferta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
