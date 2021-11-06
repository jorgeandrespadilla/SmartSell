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
    public class ComentariosController : Controller
    {
        private SmartSell db = new SmartSell();

        // GET: Comentarios
        public ActionResult Index()
        {
            return RedirectToAction("Subastas");
        }

        // GET: Comentarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        // GET: Comentarios/Create
        public ActionResult Create(int? subastaID)
        {
            Subasta subasta = db.Subasta.Find(subastaID);
            return View(new ComentarioCreateViewModel
            {
                Comentario = "",
                Subasta = subasta,
                
            }) ;
        }


        // POST: Comentarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ComentarioCreateViewModel comentario)
        {
            if (ModelState.IsValid)
            {
                return View(comentario);
            }
            db.Comentario.Add(new Comentario
            {
                UsuarioID = Convert.ToInt32(HttpContext.Session["UserID"]),
                SubastaID = comentario.Subasta.SubastaID,
                Descripcion = comentario.Comentario,
                FechaCreacion = DateTime.Now
            }) ;
            db.SaveChanges();
            return RedirectToAction("Index", "Subastas");
            
        }

        // GET: Comentarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubastaID = new SelectList(db.Subasta, "SubastaID", "NombreProducto", comentario.SubastaID);
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", comentario.UsuarioID);
            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComentarioID,UsuarioID,SubastaID,Descripcion,FechaCreacion")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubastaID = new SelectList(db.Subasta, "SubastaID", "NombreProducto", comentario.SubastaID);
            ViewBag.UsuarioID = new SelectList(db.Usuario, "UsuarioID", "Nombres", comentario.UsuarioID);
            return View(comentario);
        }

        // GET: Comentarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comentario comentario = db.Comentario.Find(id);
            db.Comentario.Remove(comentario);
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
