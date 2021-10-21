using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Web.Models;

namespace ProyectoFinal.Web.Controllers
{
    public class SubastasController : Controller
    {
        private SmartSell db = new SmartSell();

        // GET: Subastas
        public ActionResult Index()
        {
            var subasta = db.Subasta.Include(s => s.Usuario);
            return View(subasta.ToList());
        }

        // GET: Subastas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null)
            {
                return HttpNotFound();
            }
            return View(subasta);
        }

        // GET: Subastas/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres");
            return View();
        }

        // POST: Subastas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubastaID,UsuarioID,NombreProducto,DescripcionProducto,FotoUrlProducto,PrecioInicial,FechaLimite")] Subasta subasta)
        {
            if (ModelState.IsValid)
            {
                db.Subasta.Add(subasta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", subasta.UsuarioID);
            return View(subasta);
        }

        // GET: Subastas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", subasta.UsuarioID);
            return View(subasta);
        }

        // POST: Subastas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubastaID,UsuarioID,NombreProducto,DescripcionProducto,FotoUrlProducto,PrecioInicial,FechaLimite")] Subasta subasta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subasta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", subasta.UsuarioID);
            return View(subasta);
        }

        // GET: Subastas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null)
            {
                return HttpNotFound();
            }
            return View(subasta);
        }

        // POST: Subastas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subasta subasta = db.Subasta.Find(id);
            db.Subasta.Remove(subasta);
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
