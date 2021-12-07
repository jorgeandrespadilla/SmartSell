using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Web.Models;
using System;
using System.Collections.Generic;
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
            return BadRequest("Not implemented");
        }
        // PerfilDto

        // GET PerfilOfertas/{id}?showOfertas={value}
        // showOfertas: "PARTICIPACION", "GANADAS"
        [HttpGet]
        public IHttpActionResult PerfilOfertas(int id, string showOfertas = null)
        {
            return BadRequest("Not implemented");
        }
        // OfertaDto

        // PUT EditPerfil/{id}
        [HttpPut]
        public IHttpActionResult EditPerfil(int id, [FromBody] EditPerfilDto dto)
        {
            return BadRequest("Not implemented");
        }

        // DELETE DeletePerfil/{id}
        [HttpDelete]
        public IHttpActionResult DeletePerfil(int id)
        {
            return BadRequest("Not implemented");
        }

        // GET PerfilVendedor/{id}
        [HttpGet]
        public IHttpActionResult PerfilVendedor(int id)
        {
            return BadRequest("Not implemented");
        }
        // PerfilVendedorDto

        // POST RatingUsuario
        [HttpPost]
        public IHttpActionResult RatingUsuario([FromBody] RatingUsuarioDto dto)
        {
            return BadRequest("Not implemented");
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
