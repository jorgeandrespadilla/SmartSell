using ProyectoFinal.Web.Infrastructure.Helpers;
using ProyectoFinal.Web.Models;
using ProyectoFinal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProyectoFinal.Web.Controllers
{
    public class AccountController : Controller
    {
        private SmartSell db = new SmartSell();

        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombres,Apellidos,Correo,Clave")] UserCreateViewModel usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }
            Usuario userQuery = db.Usuario.Where(u => u.Correo == usuario.Correo.ToLower()).FirstOrDefault();
            if (userQuery != null)
            {
                ModelState.AddModelError("generalError", "Ya existe un usuario con este correo.");
                return View(usuario);
            }
            string passwordHash = Hasher.ToSHA256(usuario.Clave);
            db.Usuario.Add(new Usuario
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo.ToLower(),
                Clave = passwordHash,
                Activo = true
            });
            db.SaveChanges();
            return RedirectToAction("Index","Account");
        }

        public ActionResult Login()
        {
            if (HttpContext.Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Subastas");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string passwordHash = Hasher.ToSHA256(model.Password);
            Usuario usuario = db.Usuario.Where(u => u.Correo == model.Email.ToLower() && u.Clave == passwordHash).FirstOrDefault();
            if (usuario == null)
            {
                ModelState.AddModelError("loginError", "Las credenciales ingresadas no son válidas.");
                return View();
            }
            if (!usuario.Activo)
            {
                ModelState.AddModelError("loginError", "La cuenta ya no se encuentra disponible.");
                return View();
            }
            HttpContext.Session["UserID"] = usuario.UsuarioID;
            HttpContext.Session["Username"] = $"{usuario.Nombres} {usuario.Apellidos}";
            return RedirectToAction("Index", "Subastas");
        }

        public ActionResult Logout()
        {
            HttpContext.Session["UserID"] = null;
            HttpContext.Session["Username"] = null;
            return RedirectToAction("Index", "Home");
        }

        
    }
}