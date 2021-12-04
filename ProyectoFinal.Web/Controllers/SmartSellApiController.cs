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

        /* Métodos para los usuarios */

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

        [HttpPost]
        public IHttpActionResult CreateAccount([FromBody] CreateAccountDto dto)
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




    }
}
