using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Web.Infrastructure;
using ProyectoFinal.Web.Infrastructure.Helpers;
using ProyectoFinal.Web.Models;
using ProyectoFinal.Web.ViewModels;

namespace ProyectoFinal.Web.Controllers
{
    [AuthenticationFilter]
    public class UsuarioController : Controller
    {
        private SmartSell db = new SmartSell();

        // GET: Usuarios
        public ActionResult Index()
        {
            return RedirectToAction("Perfil", "Usuario");
        }

        public ActionResult Perfil(string showInfo)
        {
            showInfo = String.IsNullOrEmpty(showInfo) ? "PARTICIPACION" : showInfo;
            ViewBag.CurrentInfo = showInfo;
            int id = Convert.ToInt32(HttpContext.Session["UserID"]);
            ICollection<Oferta> ofertas=new List<Oferta>();
            var ofertasQuery = db.Oferta.Where( o=> o.UsuarioID == id ).GroupBy(o => o.SubastaID).Select(g => new
            {                
                OfertaActual = g.OrderByDescending(x => x.Monto).Select(x => x).FirstOrDefault()
            }).ToList();

            if (ViewBag.CurrentInfo == "PARTICIPACION")
            {
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) > 0).ToList();                
            } else if (ViewBag.CurrentInfo == "GANADAS")
            {
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) <= 0).ToList();                               
            }
            foreach (var oferta in ofertasQuery)
            {
                ofertas.Add(oferta.OfertaActual);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(new PerfilViewModel
            {
                Usuario = usuario, Rating = 1, Ofertas = ofertas
            }) ;
        }
        public ActionResult PerfilVendedor(int id)
        {
            int idUsuarioLogeado = Convert.ToInt32(HttpContext.Session["UserID"]);
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {               
                return HttpNotFound();
            }
            var currentRating = db.RatingUsuario.Where(ru => ru.UsuarioCalificadorID == idUsuarioLogeado && ru.UsuarioCalificadoID == id).FirstOrDefault();
            var rating = Convert.ToString(currentRating != null ? currentRating.Rating : 0);
            var avgRating = db.RatingUsuario.Where(u => u.UsuarioCalificadoID == id ).Average(ru => ru.Rating);
            return View(new PerfilVendedorViewModel
            {
                UsuarioCalificadoId = usuario.UsuarioID,
                Usuario = usuario,
                AvgRating = (float)avgRating,                
                Rating =  rating
            }) ;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PerfilVendedor(PerfilVendedorViewModel model)
        {
            int rating = Convert.ToInt32(model.Rating);
            var usuarioCalificado = db.Usuario.Find(model.UsuarioCalificadoId);
            model.Usuario = usuarioCalificado;
            if (rating != 0)
            {
                int idUsuarioCalificador = Convert.ToInt32(HttpContext.Session["UserID"]);
                var currentRating = db.RatingUsuario.Where(ru => ru.UsuarioCalificadorID == idUsuarioCalificador && ru.UsuarioCalificadoID == model.UsuarioCalificadoId).FirstOrDefault();
                if (currentRating == null)
                {
                    db.RatingUsuario.Add(new RatingUsuario
                    {
                        UsuarioCalificadorID = idUsuarioCalificador,
                        UsuarioCalificadoID = model.UsuarioCalificadoId,
                        Rating = rating
                    });
                    db.SaveChanges();
                }
                else
                {
                    currentRating.Rating = rating;
                    db.Entry(currentRating).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var avgRating = db.RatingUsuario.Where(u => u.UsuarioCalificadoID == model.UsuarioCalificadoId).Average(ru => ru.Rating);
                model.AvgRating = (float)avgRating;
            }
            return View(model);
        }

        public ActionResult Editar()
        {
            int id = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(new UserEditViewModel
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Clave = null
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Nombres,Apellidos,Correo,Clave")] UserEditViewModel usuario)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("generalError", "La contraseña debe ser de más de 8 caracteres.");
                return View(usuario);
            }
            Usuario userQuery = db.Usuario.Where(u => u.Correo == usuario.Correo).FirstOrDefault();
            if (userQuery != null && userQuery.UsuarioID != Convert.ToInt32(HttpContext.Session["UserID"]))
            {
                ModelState.AddModelError("generalError", "Ya existe un usuario con este correo.");
                return View(usuario);
            }
            string passwordHash;
            userQuery = db.Usuario.Find(Convert.ToInt32(HttpContext.Session["UserID"]));
            if (!String.IsNullOrEmpty(usuario.Clave))
            {
                passwordHash = Hasher.ToSHA256(usuario.Clave);
            }
            else
            {
                passwordHash = userQuery.Clave;
            }

            userQuery.Nombres = usuario.Nombres;
            userQuery.Apellidos = usuario.Apellidos;
            userQuery.Correo = usuario.Correo;
            userQuery.Clave = passwordHash;

            db.Entry(userQuery).State = EntityState.Modified;
            db.SaveChanges();
            HttpContext.Session["Username"] =$"{usuario.Nombres} {usuario.Apellidos}";
            return RedirectToAction("Perfil", "Usuario");
        }

        public ActionResult DeleteConfirmed()
        {
            int id = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            db.Usuario.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Logout", "Account");
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
