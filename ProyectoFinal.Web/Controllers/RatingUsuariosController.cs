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
    public class RatingUsuariosController : Controller
    {
        private SmartSell db = new SmartSell();

        // GET: RatingUsuarios
        public ActionResult Index()
        {
            var ratingUsuario = db.RatingUsuario.Include(r => r.UsuarioCalificado).Include(r => r.UsuarioCalificador);
            return View(ratingUsuario.ToList());
        }

        // GET: RatingUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingUsuario ratingUsuario = db.RatingUsuario.Find(id);
            if (ratingUsuario == null)
            {
                return HttpNotFound();
            }
            return View(ratingUsuario);
        }

        // GET: RatingUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioCalificadoID = new SelectList(db.Usuario, "UsuarioID", "Nombres");
            ViewBag.UsuarioCalificadorID = new SelectList(db.Usuario, "UsuarioID", "Nombres");
            return View();
        }

        // POST: RatingUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RatingUsuarioID,UsuarioCalificadoID,UsuarioCalificadorID,Rating")] RatingUsuario ratingUsuario)
        {
            if (ModelState.IsValid)
            {
                db.RatingUsuario.Add(ratingUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioCalificadoID = new SelectList(db.Usuario, "UsuarioID", "Nombres", ratingUsuario.UsuarioCalificadoID);
            ViewBag.UsuarioCalificadorID = new SelectList(db.Usuario, "UsuarioID", "Nombres", ratingUsuario.UsuarioCalificadorID);
            return View(ratingUsuario);
        }

        // GET: RatingUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingUsuario ratingUsuario = db.RatingUsuario.Find(id);
            if (ratingUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioCalificadoID = new SelectList(db.Usuario, "UsuarioID", "Nombres", ratingUsuario.UsuarioCalificadoID);
            ViewBag.UsuarioCalificadorID = new SelectList(db.Usuario, "UsuarioID", "Nombres", ratingUsuario.UsuarioCalificadorID);
            return View(ratingUsuario);
        }
       // public ActionResult Rate()
        //{
            //toma un valor 
          //  int suma=0;
            /*double prom;
            int total;
            //foreach (//valor in elvalor){
                // suma +=valor
            //(double)prom = suma / total;
        }*/

        // POST: RatingUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RatingUsuarioID,UsuarioCalificadoID,UsuarioCalificadorID,Rating")] RatingUsuario ratingUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ratingUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioCalificadoID = new SelectList(db.Usuario, "UsuarioID", "Nombres", ratingUsuario.UsuarioCalificadoID);
            ViewBag.UsuarioCalificadorID = new SelectList(db.Usuario, "UsuarioID", "Nombres", ratingUsuario.UsuarioCalificadorID);
            return View(ratingUsuario);
        }

        // GET: RatingUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingUsuario ratingUsuario = db.RatingUsuario.Find(id);
            if (ratingUsuario == null)
            {
                return HttpNotFound();
            }
            return View(ratingUsuario);
        }

        // POST: RatingUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RatingUsuario ratingUsuario = db.RatingUsuario.Find(id);
            db.RatingUsuario.Remove(ratingUsuario);
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
