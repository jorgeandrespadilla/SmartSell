using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ProyectoFinal.Web.Models;

// TODO: Eliminar este controlador de prueba

namespace ProyectoFinal.Web.Controllers
{
    public class SubastasApiController : ApiController
    {
        private SmartSell db = new SmartSell();

        // GET: api/SubastasApi
        public IQueryable<Subasta> GetSubasta()
        {
            return db.Subasta;
        }

        // GET: api/SubastasApi/5
        [ResponseType(typeof(Subasta))]
        public IHttpActionResult GetSubasta(int id)
        {
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null)
            {
                return NotFound();
            }

            return Ok(subasta);
        }

        // PUT: api/SubastasApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubasta(int id, Subasta subasta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subasta.SubastaID)
            {
                return BadRequest();
            }

            db.Entry(subasta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubastaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SubastasApi
        [ResponseType(typeof(Subasta))]
        public IHttpActionResult PostSubasta(Subasta subasta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subasta.Add(subasta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subasta.SubastaID }, subasta);
        }

        // DELETE: api/SubastasApi/5
        [ResponseType(typeof(Subasta))]
        public IHttpActionResult DeleteSubasta(int id)
        {
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null)
            {
                return NotFound();
            }

            db.Subasta.Remove(subasta);
            db.SaveChanges();

            return Ok(subasta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubastaExists(int id)
        {
            return db.Subasta.Count(e => e.SubastaID == id) > 0;
        }
    }
}