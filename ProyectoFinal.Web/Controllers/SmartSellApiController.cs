using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Models;
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
            foreach (var oferta in filteredOfertas)
            {
                ofertas.Add(new OfertaDto(
                    oferta.UsuarioID,
                    oferta.OfertaID,
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
                        UsuarioCalificadorID = dto.UsuarioCalificadorID,
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

        // GET Subastas/{id}?page=1&searchString={value}&sortOrder={value}&hideEnded={value}&hideMySubastas={value}&showAll={value}
        [HttpGet]
        public IHttpActionResult Subastas(int id, int page = 1, string searchString = null, string sortOrder = null, string hideEnded = null, string hideMySubastas = null, string showAll = null)
        {
            /* Manejar configuración de filtrado y de ocultamiento */
            showAll = String.IsNullOrEmpty(showAll) ? "true" : showAll;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            hideEnded = String.IsNullOrEmpty(hideEnded) ? "true" : hideEnded;
            hideMySubastas = string.IsNullOrEmpty(hideMySubastas) && String.Compare(showAll, "true") == 0 ? "true" : hideMySubastas;

            /* Manejar configuración de búsqueda y paginación */
            if (searchString != null)
            {
                page = 1;
            }

            /* Consultar subastas junto con su mayor oferta */
            var subastasQuery = db.Subasta.Where(s => s.Usuario.Activo).GroupJoin(db.Oferta, s => s.SubastaID, o => o.SubastaID, (s, o) => new
            {
                Subasta = s,
                OfertaActual = o.OrderByDescending(x => x.Monto).FirstOrDefault()
            }
            ).ToList();

            /* Manejar despliegue para mis ofertas/todas las ofertas */
            if (showAll == "false")
            {
                subastasQuery = subastasQuery.Where(s => s.Subasta.UsuarioID == id).ToList();
            }

            /* Manejar cadena de búsqueda */
            if (!String.IsNullOrEmpty(searchString))
            {
                subastasQuery = subastasQuery.Where(s => s.Subasta.NombreProducto.ToLower().Contains(searchString.ToLower())).ToList();
            }


            /* Generar modelos de subastas */
            var subastasDto = new List<SubastaItemDto>();
            foreach (var subastaQuery in subastasQuery)
            {
                subastasDto.Add(new SubastaItemDto(
                    subastaQuery.Subasta.SubastaID,
                    (int)subastaQuery.Subasta.UsuarioID,
                    subastaQuery.Subasta.FotoUrlProducto,
                    subastaQuery.Subasta.NombreProducto,
                    subastaQuery.OfertaActual == null ? subastaQuery.Subasta.PrecioInicial : (float)subastaQuery.OfertaActual.Monto, // Si existen ofertas, obtiene el monto actual de la oferta más alta. Caso contrario, obtiene el precio inicial
                    subastaQuery.Subasta.FechaLimite,
                    DateTime.Compare(DateTime.Now, subastaQuery.Subasta.FechaLimite) <= 0 // Si la fecha y hora actuales son anteriores a la fecha límite, la subasta se encuentra vigente
                ));
            }

            if (subastasQuery.Count() != 0)
            {
                /* Manejar ocultamiento de subastas pasadas/finalizadas */
                if (hideEnded == "true")
                {
                    subastasDto = subastasDto.Where(s => s.Vigente).ToList();
                }
                if (hideMySubastas == "true" && showAll == "true")
                {
                    subastasDto = subastasDto.Where(s => s.UsuarioID != id).ToList();
                }

                /* Manejar filtros */
                switch (sortOrder)
                {
                    case "price_asc":
                        subastasDto = subastasDto.OrderBy(s => s.MontoActual).ToList();
                        break;
                    case "price_desc":
                        subastasDto = subastasDto.OrderByDescending(s => s.MontoActual).ToList();
                        break;
                    case "name_asc":
                        subastasDto = subastasDto.OrderBy(s => s.NombreProducto).ToList();
                        break;
                    case "name_desc":
                        subastasDto = subastasDto.OrderByDescending(s => s.NombreProducto).ToList();
                        break;
                    default:
                        subastasDto = subastasDto.OrderBy(s => s.Fecha).ToList();
                        break;
                }
            }

            /* Configurar paginación */
            int pageSize = 6; // Numero de elementos por página
            int totalResults = subastasDto.Count();
            int totalPages = Convert.ToInt32(totalResults / pageSize) + 1;
            if (page < 1 || page > totalPages)
            {
                page = 1;
            }

            var data = subastasDto.Skip(pageSize * (page - 1)).Take(pageSize).ToList();

            return Ok(new SubastasPagedData(
                data,
                page,
                pageSize,
                totalPages,
                totalResults,
                searchString,
                sortOrder,
                hideEnded == "true",
                hideMySubastas == "true",
                showAll == "true"
            ));
        }
        // SubastasPagedData

        // GET Subasta/{id}
        [HttpGet]
        public IHttpActionResult Subasta(int id)
        {
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null || !subasta.Usuario.Activo)
            {
                return BadRequest("No se encontró el detalle de subasta.");
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

            ICollection<OfertaDto> ofertasDto = new List<OfertaDto>();
            foreach(var oferta in ofertas)
            {
                ofertasDto.Add(new OfertaDto(
                    oferta.UsuarioID,
                    oferta.OfertaID,
                    oferta.SubastaID,
                    $"{oferta.Usuario.Nombres} {oferta.Usuario.Apellidos}",
                    oferta.Monto,
                    oferta.FechaCreacion
                ));
            }

            ICollection<ComentarioDto> comentariosDto = new List<ComentarioDto>();
            foreach(var comentario in subasta.Comentarios)
            {
                comentariosDto.Add(new ComentarioDto(
                    comentario.ComentarioID,
                    comentario.UsuarioID,
                    $"{comentario.Usuario.Nombres} {comentario.Usuario.Apellidos}",
                    comentario.Descripcion,
                    comentario.FechaCreacion
                ));
            }
            comentariosDto = comentariosDto.OrderByDescending(o => o.FechaCreacion).ToList();

            return Ok(new SubastaDto(
                subasta.SubastaID,
                (int)subasta.UsuarioID,
                subasta.NombreProducto,
                subasta.DescripcionProducto,
                $"{subasta.Usuario.Nombres} {subasta.Usuario.Apellidos}",
                montoActual,
                subasta.FechaLimite,
                DateTime.Compare(DateTime.Now, subasta.FechaLimite) <= 0,
                subasta.FotoUrlProducto,
                ofertasDto,
                comentariosDto
            ));
        }
        // SubastaDto -> should also contain a list of Comentarios
        //
        // ofertaActual and viewMode should be check at UWP & Xamarin app 

        // POST CreateSubasta
        [HttpPost]
        public IHttpActionResult CreateSubasta([FromBody] CreateSubastaDto dto)
        { 
            db.Subasta.Add(new Subasta
            {
                UsuarioID = dto.UsuarioID,
                NombreProducto = dto.NombreProducto,
                DescripcionProducto = dto.DescripcionProducto,
                FotoUrlProducto = dto.UriImagen,
                PrecioInicial = dto.PrecioInicial,
                FechaLimite = dto.FechaLimite
            });
            db.SaveChanges();
            return Ok();
        }

        // PUT EditSubasta/{id}
        [HttpPut]
        public IHttpActionResult EditSubasta(int id, [FromBody] EditSubastaDto dto)
        {
            Subasta subasta = db.Subasta.Find(id);

            if (subasta == null)
            {
                return BadRequest("No se encontró la subasta.");
            }

            subasta.NombreProducto = dto.NombreProducto.Trim();
            subasta.DescripcionProducto = dto.DescripcionProducto.Trim();
            if (dto.UriImagen != null)
            {
                subasta.FotoUrlProducto = dto.UriImagen;
            }
            
            db.Entry(subasta).State = EntityState.Modified;
            db.SaveChanges();

            return Ok();
        }

        // DELETE DeleteSubasta/{id}
        [HttpDelete]
        public IHttpActionResult DeleteSubasta(int id)
        {
            Subasta subasta = db.Subasta.Find(id);
            if (subasta == null)
            {
                return BadRequest("No se encontró la subasta.");
            }

            var ofertas = db.Oferta.Where(o => o.SubastaID == id).ToList();
            db.Oferta.RemoveRange(ofertas);
            db.Subasta.Remove(subasta);
            db.SaveChanges();
           
            return Ok();
        }




        /**** Métodos para los comentarios ****/

        // GET Comentario/{id}
        [HttpGet]
        public IHttpActionResult Comentario(int id)
        {
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return BadRequest("No se encontró el comentario.");
            }
            return Ok(new ComentarioDto(
                comentario.ComentarioID,
                comentario.UsuarioID,
                $"{comentario.Usuario.Nombres} {comentario.Usuario.Apellidos}",
                comentario.Descripcion,
                comentario.FechaCreacion
            ));
        }
        // ComentarioDto

        // POST CreateComentario
        [HttpPost]
        public IHttpActionResult CreateComentario([FromBody] CreateComentarioDto dto)
        {
            db.Comentario.Add(new Comentario
            {
                UsuarioID = dto.UsuarioID,
                SubastaID = dto.SubastaID,
                Descripcion = dto.Descripcion,
                FechaCreacion = DateTime.Now
            });
            db.SaveChanges();
            return Ok();
        }

        // PUT EditComentario/{id}
        [HttpPut]
        public IHttpActionResult EditComentario(int id, [FromBody] EditComentarioDto dto)
        {
            Comentario comentario = db.Comentario.Find(id);

            if (comentario == null)
            {
                return BadRequest("No se encontró el comentario.");
            }

            comentario.Descripcion = dto.Descripcion;

            db.Entry(comentario).State = EntityState.Modified;
            db.SaveChanges();

            return Ok();
        }

        // DELETE DeleteComentario/{id}
        [HttpDelete]
        public IHttpActionResult DeleteComentario(int id)
        {
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return BadRequest("No se encontró el comentario.");
            }

            db.Comentario.Remove(comentario);
            db.SaveChanges();

            return Ok();
        }




        /**** Métodos para las ofertas ****/

        // GET Ofertas/{id}?page=1&searchString={value}
        [HttpGet]
        public IHttpActionResult Ofertas(int id, int page = 1, string searchString = null)
        {
            /* Manejar configuración de búsqueda y paginación */
            if (searchString != null)
            {
                page = 1;
            }
            var ofertasQuery = db.Oferta.Where(o => o.UsuarioID == id).OrderByDescending(o => o.FechaCreacion).ToList();
            
            /* Manejar cadena de búsqueda */
            if (!String.IsNullOrEmpty(searchString))
            {
                ofertasQuery = ofertasQuery.Where(s => s.Subasta.NombreProducto.ToLower().Contains(searchString.ToLower())).ToList();
            }

            /* Configurar paginación */
            int pageSize = 10; // Numero de elementos por página
            int totalResults = ofertasQuery.Count();
            int totalPages = Convert.ToInt32(totalResults / pageSize) + 1;
            if (page < 1 || page > totalPages)
            {
                page = 1;
            }

            var data = ofertasQuery.Skip(pageSize * (page - 1)).Take(pageSize).ToList();

            ICollection<OfertaDto> ofertasDto = new List<OfertaDto>();
            foreach(var oferta in data)
            {
                ofertasDto.Add(new OfertaDto(
                    oferta.UsuarioID,
                    oferta.OfertaID,
                    oferta.SubastaID,
                    oferta.Subasta.NombreProducto,
                    oferta.Monto,
                    oferta.FechaCreacion
                ));
            }

            return Ok(new PagedData<OfertaDto>(
                ofertasDto,
                page,
                pageSize,
                totalPages,
                totalResults,
                searchString
            ));
        }
        // PagedData<OfertaDto> -> Nombre corresponde al NombreProducto

        // POST CreateOferta
        [HttpPost]
        public IHttpActionResult CreateOferta([FromBody] CreateOfertaDto dto)
        {
            Oferta ofertaActual = db.Oferta.Where(m => m.SubastaID == dto.SubastaID).OrderByDescending(o => o.Monto).FirstOrDefault();
            if (ofertaActual == null)
            {
                Subasta subasta = db.Subasta.Find(dto.SubastaID);
                if (dto.Monto < subasta.PrecioInicial)
                {
                    return BadRequest("El monto es inferior al precio inicial.");
                }
            }
            else if (dto.Monto <= ofertaActual.Monto)
            {
                return BadRequest("El monto es inferior o igual al monto actual");
            }
            db.Oferta.Add(new Oferta
            {
                UsuarioID = dto.UsuarioID,
                SubastaID = dto.SubastaID,
                Monto = dto.Monto,
                FechaCreacion = DateTime.Now
            });
            db.SaveChanges();
            return Ok();
        }

        // DELETE DeleteOferta/{id}
        [HttpDelete]
        public IHttpActionResult DeleteOferta(int id)
        {
            Oferta oferta = db.Oferta.Find(id);
            if (oferta == null)
            {
                return BadRequest("No se encontró la oferta.");
            }

            db.Oferta.Remove(oferta);
            db.SaveChanges();

            return Ok();
        }

    }
}
