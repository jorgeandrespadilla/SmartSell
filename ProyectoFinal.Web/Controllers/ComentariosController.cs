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
            if (subastaID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subasta subasta = db.Subasta.Find(subastaID);
            return View(new ComentarioCreateViewModel
            {
                DescripcionComentario = "",
                SubastaId = subastaID.Value
            }) ;
        }


        // POST: Comentarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ComentarioCreateViewModel comentario)
        {
            if (!ModelState.IsValid)
            {
                return View(comentario);
            }
            db.Comentario.Add(new Comentario
            {
                UsuarioID = Convert.ToInt32(HttpContext.Session["UserID"]),
                SubastaID = comentario.SubastaId,
                Descripcion = comentario.DescripcionComentario,
                FechaCreacion = DateTime.Now
            });
            db.SaveChanges();
            return RedirectToAction("Details", "Subastas", new { id = comentario.SubastaId});
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
            int userId = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (comentario.UsuarioID != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ComentarioEditViewModel
            {
                SubastaId = comentario.SubastaID,
                ComentarioId = comentario.ComentarioID,
                DescripcionComentario = comentario.Descripcion
            });
        }

        // POST: Comentarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ComentarioEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comentario = db.Comentario.Find(model.ComentarioId);
                comentario.Descripcion = model.DescripcionComentario;
                comentario.FechaCreacion = DateTime.Now;
                db.Entry(comentario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Subastas", new { id = model.SubastaId });
            }
            return View(model);
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
        public ActionResult DeleteConfirmed(int id)
        {
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userId = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (comentario.UsuarioID != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Comentario.Remove(comentario);
            db.SaveChanges();
            return RedirectToAction("Details","Subastas",new { id = comentario.SubastaID});
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
