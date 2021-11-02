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
            showInfo = String.IsNullOrEmpty(showInfo) ? "" : showInfo;
            ViewBag.CurrentInfo = showInfo;
            int id = Convert.ToInt32(HttpContext.Session["UserID"]);
            ICollection<Oferta> ofertas=new List<Oferta>();
            if (showInfo=="")
            {
                ViewBag.CurrentInfo = "PARTICIPACION";
            }
            var subastasQuery = db.Subasta.GroupJoin(db.Oferta, s => s.SubastaID, o => o.SubastaID, (s, o) => new
            {
                Subasta = s,
                Ofertas= o.OrderByDescending(x => x.Monto).ToList()
            }
            ).ToList();            
            if (ViewBag.CurrentInfo == "PARTICIPACION")
            {
                subastasQuery = subastasQuery.Where(o => DateTime.Compare(o.Subasta.FechaLimite, DateTime.Now) > 0).ToList();
                foreach (var subasta in subastasQuery)
                {
                    if (subasta.Ofertas.Count!=0)
                    {
                        var subastaUsuario = subasta.Ofertas.Where(o => o.UsuarioID==id).ToList();
                        if (subastaUsuario.Count()!=0)
                        {
                            ofertas.Add(subasta.Ofertas.Where(o => o.UsuarioID == id).FirstOrDefault());
                        }
                    }
                }
            } else if (ViewBag.CurrentInfo == "GANADAS")
            {
                subastasQuery = subastasQuery.Where(o => DateTime.Compare(o.Subasta.FechaLimite, DateTime.Now) <= 0).ToList();
                foreach (var subasta in subastasQuery)
                {
                    if (subasta.Ofertas.ElementAtOrDefault(0).UsuarioID==id)
                    {
                        ofertas.Add(subasta.Ofertas.ElementAtOrDefault(0));
                    }
                }
            }
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(new PerfilViewModel
            {
                Usuario = usuario, Rating = 1, misSubastas = ofertas
            }) ;
        }
        public ActionResult PerfilVendedor(int id)
        {
            
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rating = db.RatingUsuario.Where(u => u.UsuarioCalificadoID == id ).Average(ru => ru.Rating);
            return View(new PerfilViewModel
            {
                Usuario = usuario,
                Rating = (float)rating
            }) ;
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
