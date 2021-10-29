using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Index()
        {
            var oferta = db.Oferta.Include(o => o.Subasta).Include(o => o.Usuario);
            return View(oferta.ToList());
        }

        // GET: Ofertas/Details/5
        public ActionResult Details(int? id)
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

        // GET: Ofertas/Create
        public ActionResult Create(int subastaID)
        {
            Subasta subasta = db.Subasta.Find(subastaID);
            if (DateTime.Compare(DateTime.Now, subasta.FechaLimite) >= 0)
            {
                return RedirectToAction("Index", "Subastas");
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

            if (!ModelState.IsValid)
            {
                return View(oferta);
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
            return RedirectToAction("Index", "Subastas");
        }
        // GET: Ofertas/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.SubastaID = new SelectList(db.Subasta, "SubastaID", "NombreProducto", oferta.SubastaID);
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", oferta.UsuarioID);
            return View(oferta);
        }

        // POST: Ofertas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OfertaID,UsuarioID,SubastaID,Monto,FechaCreacion")] Oferta oferta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oferta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubastaID = new SelectList(db.Subasta, "SubastaID", "NombreProducto", oferta.SubastaID);
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", oferta.UsuarioID);
            return View(oferta);
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
