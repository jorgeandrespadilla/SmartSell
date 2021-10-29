﻿using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProyectoFinal.Web.Models;
using ProyectoFinal.Web.ViewModels;
using PagedList;
using ProyectoFinal.Web.Helpers;

namespace ProyectoFinal.Web.Controllers
{
    public class SubastasController : Controller
    {
        private SmartSell db = new SmartSell();

        // GET: Subastas
        // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string hideEnded, int? page)
        {
            /* Manejar configuración de filtrado y de ocultamiento */
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            ViewBag.CurrentSort = sortOrder;
            hideEnded = String.IsNullOrEmpty(hideEnded) ? "true" : hideEnded;
            ViewBag.HideEnded = hideEnded;

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

            /* Manejar cadena de búsqueda */
            /* Consultar subastas junto con su mayor oferta */
            var subastasQuery = db.Subasta.GroupJoin(db.Oferta, s => s.SubastaID, o => o.SubastaID, (s, o) => new
            {
                Subasta = s,
                OfertaActual = o.OrderByDescending(x => x.Monto).FirstOrDefault()
            }
            ).ToList();
            /* var subastasActivas = db.Oferta.GroupBy(o => o.SubastaID).Select(g => new
            {
                SubastaID = g.Key,
                OfertaActual = g.OrderByDescending(x => x.Monto).Select(x => x).FirstOrDefault()
            }).ToList(); */
            if (!String.IsNullOrEmpty(searchString))
            {
                subastasQuery = subastasQuery.Where(s => s.Subasta.NombreProducto.Contains(searchString)).ToList();
            }

            /* Manejar existencia o no de resultados */
            if (subastasQuery.Count() == 0)
            {
                ViewBag.ResultsFound = false;
                return View();
            }
            ViewBag.ResultsFound = true;

            /* Generar modelos de subastas */
            var subastasModel = new List<SubastaItemViewModel>();
            foreach (var subasta in subastasQuery)
            {
                subastasModel.Add(new SubastaItemViewModel
                {
                    Subasta = subasta.Subasta,
                    MontoActual = subasta.OfertaActual == null ? subasta.Subasta.PrecioInicial : subasta.OfertaActual.Monto, // Si existen ofertas, obtiene el monto actual de la oferta más alta. Caso contrario, obtiene el precio inicial.
                    Vigente = DateTime.Compare(DateTime.Now, subasta.Subasta.FechaLimite) <= 0 // Si la fecha y hora actuales son anteriores a la fecha límite, la subasta se encuentra vigente.
                });
            }

            /* Manejar ocultamiento de subastas pasadas/finalizadas */
            if (String.Compare(hideEnded, "true") == 0)
            {
                subastasModel = subastasModel.Where(s => s.Vigente).ToList();
            }

            /* Manejar filtros */
            switch (sortOrder)
            {
                case "price_asc":
                    subastasModel = subastasModel.OrderBy(s => s.MontoActual).ToList();
                    break;
                case "price_desc":
                    subastasModel = subastasModel.OrderByDescending(s => s.MontoActual).ToList();
                    break;
                case "name_asc":
                    subastasModel = subastasModel.OrderBy(s => s.Subasta.NombreProducto).ToList();
                    break;
                case "name_desc":
                    subastasModel = subastasModel.OrderByDescending(s => s.Subasta.NombreProducto).ToList();
                    break;
                default:
                    subastasModel = subastasModel.OrderBy(s => s.Subasta.FechaLimite).ToList();
                    break;
            }

            /* Configurar paginación */
            /*
            ViewBag.PaginaActual = page;
            ViewBag.EnableNextPage = (page < paginasTotales);
            ViewBag.EnablePreviousPage = (page > 1);
            ViewBag.PaginaActual = page;
            ViewBag.PaginaMimima = paginasTotales - 4 >= 1 ? paginasTotales - 4 : 1;
            ViewBag.PaginaMaxima = paginasTotales + 3 >= 1 ? paginasTotales - 4 : 1; 
            */
            int pageSize = 6;
            int paginasTotales = Convert.ToInt32(subastasModel.Count() / pageSize) + 1;
            if (page != null && (page < 1 || page > paginasTotales))
            {
                page = 1;
            }
            int pageNumber = (page ?? 1);

            return View(subastasModel.ToPagedList(pageNumber, pageSize));
        }

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

            Oferta ofertaActual = db.Oferta.Where(m => m.SubastaID == id).OrderByDescending(o => o.Monto).FirstOrDefault();
            float montoActual;
            if (ofertaActual == null)
            {
                montoActual = subasta.PrecioInicial;
            }
            else
            {
                montoActual = ofertaActual.Monto;
            }
            ICollection<Oferta> ofertas = db.Oferta.Where(u => u.SubastaID == id).OrderByDescending(o => o.Monto).ToList();

            int userID = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (userID == subasta.UsuarioID)
            {
                ViewBag.ViewMode = "VENDEDOR";
            }
            else
            {
                ViewBag.ViewMode = "COMPRADOR";
            }

            return View(new SubastaDetailsViewModel
            {
                OfertasSubasta = ofertas,
                Subasta = subasta,
                Vigente = DateTime.Compare(DateTime.Now, subasta.FechaLimite) <= 0,
                MontoActual = montoActual
            });
        }

        // GET: Subastas/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: Subastas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubastaCreateViewModel subasta)
        {
            if (ModelState.IsValid)
            {
                if (subasta.Imagen.ContentLength > 1000000)
                {
                    ModelState.AddModelError("generalError", "El tamaño de la imagen supera el límite permitido (1 MB).");
                }
                else
                {
                    string imageUrl = Uploader.GetImageUrl(subasta.Imagen);
                    db.Subasta.Add(new Subasta
                    {
                        UsuarioID = Convert.ToInt32(HttpContext.Session["UserID"]),
                        NombreProducto = subasta.NombreProducto,
                        DescripcionProducto = subasta.DescripcionProducto,
                        FotoUrlProducto = imageUrl,
                        PrecioInicial = subasta.PrecioInicial,
                        FechaLimite = DateTime.Now
                    });
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // GET: Subastas/Edit/5
        public ActionResult Edit(int? ID)
        {
            Subasta subasta = db.Subasta.Find(ID);
            int userID = Convert.ToInt32(HttpContext.Session["UserID"]);
            if (subasta.UsuarioID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
                Subasta subastaQuery = db.Subasta.Find(subasta.SubastaID);

                subastaQuery.UsuarioID = subastaQuery.UsuarioID;
                subastaQuery.NombreProducto = subasta.NombreProducto;
                subastaQuery.DescripcionProducto = subasta.DescripcionProducto;
                subastaQuery.FotoUrlProducto = subasta.FotoUrlProducto;
                subastaQuery.PrecioInicial = subastaQuery.PrecioInicial;
                subastaQuery.FechaLimite = subastaQuery.FechaLimite;
                db.Entry(subastaQuery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
