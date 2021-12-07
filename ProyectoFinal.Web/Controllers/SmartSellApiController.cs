using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ProyectoFinal.Web.Controllers
{
    public class SmartSellApiController : ApiController
    {

        private SmartSell db = new SmartSell();


        /**** Métodos para la autenticación ****/

        // POST Authorize
        [HttpPost]
        public IHttpActionResult Authorize([FromBody] CredencialesUsuarioDto dto)
        {
            Usuario usuario = db.Usuario.Where(u => u.Correo == dto.Correo.ToLower() && u.Clave == dto.Clave).FirstOrDefault();
            if (usuario == null)
            {
                return BadRequest("Las credenciales ingresadas no son válidas.");
            }
            if (!usuario.Activo)
            {
                return BadRequest("La cuenta ya no se encuentra disponible.");
            }
            return Ok(new AuthorizedUsuarioDto(
                    usuario.UsuarioID,
                    $"{usuario.Nombres} {usuario.Apellidos}"
            ));
        }
        // AuthorizedUsuarioDto

        // POST CreateAccount
        [HttpPost]
        public IHttpActionResult CreateAccount([FromBody] CreateUsuarioDto dto)
        {
            Usuario userQuery = db.Usuario.Where(u => u.Correo == dto.Correo).FirstOrDefault();
            if (userQuery != null)
            {
                return BadRequest("Ya existe un usuario con este correo.");
            }
            db.Usuario.Add(new Usuario
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Correo = dto.Correo.ToLower(),
                Clave = dto.Clave,
                Activo = true
            });
            db.SaveChanges();
            return Ok();
        }




        /**** Métodos para los usuarios ****/

        // GET Perfil/{id}
        [HttpGet]
        public IHttpActionResult Perfil(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return BadRequest("No se encontró el perfil del usuario.");
            }

            var ratings = db.RatingUsuario.Where(u => u.UsuarioCalificadoID == id).ToList();
            double avgRating = 0;
            if (ratings.Count != 0)
            {
                avgRating = ratings.Average(ru => ru.Rating);
            }

            return Ok(new PerfilDto(
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Correo,
                (float)avgRating
            ));
        }
        // PerfilDto

        // GET PerfilOfertas/{id}?showOfertas={value}
        // showOfertas: "PARTICIPACION", "GANADAS"
        [HttpGet]
        public IHttpActionResult PerfilOfertas(int id, string showOfertas = null)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return BadRequest("No se encontraron las ofertas del usuario.");
            }

            showOfertas = String.IsNullOrEmpty(showOfertas) ? "PARTICIPACION" : showOfertas.ToUpper();
            var ofertasQuery = db.Oferta.Where(o => o.UsuarioID == id).GroupBy(o => o.SubastaID).Select(g => new
            {
                OfertaActual = g.OrderByDescending(x => x.Monto).Select(x => x).FirstOrDefault()
            }).ToList();
            ICollection<Oferta> filteredOfertas = new List<Oferta>();
            if (showOfertas == "PARTICIPACION")
            {
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) > 0).ToList();
                foreach (var oferta in ofertasQuery)
                {
                    filteredOfertas.Add(oferta.OfertaActual);
                }
                filteredOfertas = filteredOfertas.OrderByDescending(o => o.Monto).ToList();
            }
            else if (showOfertas == "GANADAS")
            {
                ofertasQuery = ofertasQuery.Where(o => DateTime.Compare(o.OfertaActual.Subasta.FechaLimite, DateTime.Now) <= 0).ToList();
                foreach (var oferta in ofertasQuery)
                {
                    var subasta = oferta.OfertaActual.Subasta;
                    var highestOferta = subasta.Ofertas.OrderByDescending(o => o.Monto).FirstOrDefault();
                    if (highestOferta != null && highestOferta.OfertaID == oferta.OfertaActual.OfertaID)
                    {
                        filteredOfertas.Add(oferta.OfertaActual);
                    }
                }
                filteredOfertas = filteredOfertas.OrderByDescending(o => o.Subasta.FechaLimite).ToList();
            }

            ICollection<OfertaDto> ofertas = new List<OfertaDto>();
            foreach(var oferta in filteredOfertas)
            {
                ofertas.Add(new OfertaDto(
                    oferta.SubastaID,
                    oferta.Subasta.NombreProducto,
                    oferta.Monto,
                    oferta.FechaCreacion
                ));
            }
            return Ok(ofertas);
        }
        // ICollection<OfertaDto> -> Nombre contiene el NombreProducto

        // PUT EditPerfil/{id}
        [HttpPut]
        public IHttpActionResult EditPerfil(int id, [FromBody] EditPerfilDto dto)
        {
            Usuario usuario = db.Usuario.Where(u => u.Correo == dto.Correo.Trim().ToLower()).FirstOrDefault();
            if (usuario != null && usuario.UsuarioID != id)
            {
                return BadRequest("Ya existe un usuario con este correo.");
            }
            usuario = db.Usuario.Find(id);

            usuario.Nombres = dto.Nombres.Trim();
            usuario.Apellidos = dto.Apellidos.Trim();
            usuario.Correo = dto.Correo.Trim().ToLower();
            if (!String.IsNullOrEmpty(dto.Clave))
            {
                usuario.Clave = dto.Clave;
            }
            db.Entry(usuario).State = EntityState.Modified;
            db.SaveChanges();

            var ratings = db.RatingUsuario.Where(u => u.UsuarioCalificadoID == id).ToList();
            double avgRating = 0;
            if (ratings.Count != 0)
            {
                avgRating = ratings.Average(ru => ru.Rating);
            }

            return Ok(new PerfilDto(
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Correo,
                (float)avgRating
            ));
        } 
        // PerfilDto

        // DELETE DeletePerfil/{id}
        [HttpDelete]
        public IHttpActionResult DeletePerfil(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return BadRequest("No se encontró el perfil del usuario.");
            }
            usuario.Activo = false;
            db.Entry(usuario).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        // GET PerfilVendedor/{id}?idUsuarioActual={value}
        // 
        // idUsuarioActual: id del usuario actualmente logeado
        [HttpGet]
        public IHttpActionResult PerfilVendedor(int id, int idUsuarioActual)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null || !usuario.Activo)
            {
                return BadRequest("No se encontró el perfil del usuario.");
            }
            var currentRating = db.RatingUsuario.Where(ru => ru.UsuarioCalificadorID == idUsuarioActual && ru.UsuarioCalificadoID == id).FirstOrDefault();
            int rating = currentRating != null ? currentRating.Rating : 0;
            var ratings = db.RatingUsuario.Where(u => u.UsuarioCalificadoID == id).ToList();
            double avgRating = 0;
            if (ratings.Count != 0)
            {
                avgRating = ratings.Average(ru => ru.Rating);
            }

            return Ok(new PerfilVendedorDto(
                usuario.UsuarioID,
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Correo,
                (float)avgRating,
                rating
            ));
        }
        // PerfilVendedorDto

        // POST RatingUsuario
        [HttpPost]
        public IHttpActionResult RatingUsuario([FromBody] RatingUsuarioDto dto)
        {
            if (dto.Rating != 0)
            {
                var currentRating = db.RatingUsuario.Where(ru => ru.UsuarioCalificadorID == dto.UsuarioCalificadorID && ru.UsuarioCalificadoID == dto.UsuarioCalificadoID).FirstOrDefault();
                if (currentRating == null)
                {
                    db.RatingUsuario.Add(new RatingUsuario
                    {
                        UsuarioCalificadorID =dto.UsuarioCalificadorID,
                        UsuarioCalificadoID = dto.UsuarioCalificadoID,
                        Rating = dto.Rating
                    });
                    db.SaveChanges();
                }
                else
                {
                    currentRating.Rating = dto.Rating;
                    db.Entry(currentRating).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Ok();
        }




        /**** Métodos para las subastas ****/

        // GET Subastas?page=1&searchString={value}&sortOrder={value}&hideEnded={value}&hideMySubastas={value}&showAll={value}
        [HttpGet]
        public IHttpActionResult Subastas(int page = 1, string searchString = null, string sortOrder = null, string hideEnded = null, string hideMySubastas = null, string showAll = null)
        {
            const int size = 6;
            return BadRequest("Not implemented");
        }
        // PagedData<SubastaItemDto>

        // GET Subasta/{id}
        [HttpGet]
        public IHttpActionResult Subasta(int id)
        {
            return BadRequest("Not implemented");
        }
        // SubastaDto -> should also contain a list of Comentarios

        // POST CreateSubasta
        [HttpPost]
        public IHttpActionResult CreateSubasta([FromBody] CreateSubastaDto dto)
        {
            return BadRequest("Not implemented");
        }

        // PUT EditSubasta/{id}
        [HttpPut]
        public IHttpActionResult EditSubasta(int id, [FromBody] EditSubastaDto dto)
        {
            return BadRequest("Not implemented");
        }

        // DELETE DeleteSubasta/{id}
        [HttpDelete]
        public IHttpActionResult DeleteSubasta(int id)
        {
            return BadRequest("Not implemented");
        }




        /**** Métodos para los comentarios ****/

        // GET Comentario/{id}
        [HttpGet]
        public IHttpActionResult Comentario(int id)
        {
            return BadRequest("Not implemented");
        }
        // ComentarioDto

        // POST CreateComentario
        [HttpPost]
        public IHttpActionResult CreateComentario([FromBody] CreateComentarioDto dto)
        {
            return BadRequest("Not implemented");
        }

        // PUT EditComentario/{id}
        [HttpPut]
        public IHttpActionResult EditComentario(int id, [FromBody] EditComentarioDto dto)
        {
            return BadRequest("Not implemented");
        }

        // DELETE DeleteComentario/{id}
        [HttpDelete]
        public IHttpActionResult DeleteComentario(int id)
        {
            return BadRequest("Not implemented");
        }




        /**** Métodos para las ofertas ****/

        // GET Ofertas?page=1&searchString={value}
        [HttpGet]
        public IHttpActionResult Ofertas(int page = 1, string searchString = null)
        {
            const int size = 10;
            return BadRequest("Not implemented");
        }
        // PagedData<OfertaDto>

        // POST CreateOferta
        [HttpPost]
        public IHttpActionResult CreateOferta([FromBody] CreateOfertaDto dto)
        {
            return BadRequest("Not implemented");
        }

        // DELETE DeleteOferta/{id}
        [HttpDelete]
        public IHttpActionResult DeleteOferta(int id)
        {
            return BadRequest("Not implemented");
        }

    }
}
