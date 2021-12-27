using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Net.Http;
using ProyectoFinal.Shared.Dto;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ProyectoFinal.Shared.Helpers;
using ProyectoFinal.UWP.Models;
using ProyectoFinal.Shared.Models;
using ProyectoFinal.UWP.Infrastructure.Helpers;

namespace ProyectoFinal.UWP.Infrastructure
{
    //Delete user: DELETE FROM Usuarios WHERE UsuarioID = *
    public sealed class SmartSell
    {


        private static HttpClient client;
        public AuthorizedUsuarioDto CurrentUser { get; set; }


        private static readonly SmartSell instance = new SmartSell();
        static SmartSell()
        {
            // Se permite la validación de cualquier certificado SSL, dado que HttpClient no acepta certificados SSL autogenerados por defecto
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44338/api/SmartSellApi/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private SmartSell() { }

        public static SmartSell Instance
        {
            get
            {
                return instance;
            }
        }



        /**** Métodos para la autenticación ****/

        public async Task<AuthorizedUsuarioDto> Login(string correo, string clave)
        {
            // Validar credenciales
            if (Validator.IsEmpty(correo) || Validator.IsEmpty(clave))
            {
                throw new Exception("Las credenciales de usuario no pueden contener campos vacíos.");
            }
            if (!Validator.IsValidEmail(correo))
            {
                throw new Exception("El correo electrónico no es válido.");
            }
            if (!Validator.IsValidPassword(clave))
            {
                throw new Exception("La contraseña no es válida.");
            }

            var body = JsonConvert.SerializeObject(new CredencialesUsuarioDto(
                correo,
                Hasher.ToSHA256(clave.ToString())
            ));

            string url = "Authorize";
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<AuthorizedUsuarioDto>(content);
                CurrentUser = data;
                return data;
            }
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsAuthorized()
        {
            return CurrentUser != null;
        }

        public async Task CreateAccount(string nombres, string apellidos, string correo, string clave)
        {
            // Validar información
            if (Validator.IsEmpty(nombres) || Validator.IsEmpty(apellidos) || Validator.IsEmpty(correo) || String.IsNullOrEmpty(clave))
            {
                throw new Exception("La información de usuario no puede contener campos vacíos.");
            }
            if (!Validator.IsValidEmail(correo))
            {
                throw new Exception("El correo electrónico no es válido.");
            }
            if (!Validator.IsValidPassword(clave))
            {
                throw new Exception("La contraseña no es válida (debe contener al menos 8 caracteres).");
            }

            var body = JsonConvert.SerializeObject(new CreateUsuarioDto(
                nombres,
                apellidos,
                correo,
                Hasher.ToSHA256(clave.ToString())
            ));

            string url = "CreateAccount";
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }



        /**** Métodos para los usuarios ****/

        public async Task<PerfilDto> GetPerfil()
        {
            string url = $"Perfil/{CurrentUser.ID}";
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<PerfilDto>(content);
                return data;
            }
        }

        // showOfertas: "PARTICIPACION", "GANADAS"
        public async Task<ICollection<OfertaDto>> GetPerfilOfertas(string showOfertas = null)
        {
            string url = $"PerfilOfertas/{CurrentUser.ID}";
            if (!string.IsNullOrEmpty(showOfertas))
            {
                url += $"?showOfertas={showOfertas}";
            }
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<ICollection<OfertaDto>>(content);
                return data;
            }
        }

        public async Task<PerfilDto> EditPerfil(string nombres, string apellidos, string correo, string clave)
        {
            // Validar información
            if (Validator.IsEmpty(nombres) || Validator.IsEmpty(apellidos) || Validator.IsEmpty(correo))
            {
                throw new Exception("La información de usuario no puede contener campos vacíos.");
            }
            if (!Validator.IsValidEmail(correo))
            {
                throw new Exception("El correo electrónico no es válido.");
            }
            if (!string.IsNullOrEmpty(clave) && !Validator.IsValidPassword(clave))
            {
                throw new Exception("La contraseña no es válida (debe contener al menos 8 caracteres).");
            }

            var body = JsonConvert.SerializeObject(new EditPerfilDto(
                nombres,
                apellidos,
                correo,
                string.IsNullOrEmpty(clave) ? null : Hasher.ToSHA256(clave)
            ));

            string url = $"EditPerfil/{CurrentUser.ID}";
            HttpResponseMessage response = await client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<PerfilDto>(content);
                return data;
            }
        }

        public async Task DeletePerfil()
        {
            string url = $"DeletePerfil/{CurrentUser.ID}";
            HttpResponseMessage response = await client.DeleteAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<PerfilVendedorDto> GetPerfilVendedor(int id)
        {
            string url = $"PerfilVendedor/{id}?idUsuarioActual={CurrentUser.ID}";
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<PerfilVendedorDto>(content);
                return data;
            }
        }

        public async Task SetRatingUsuario(int id, int rating)
        {
            var body = JsonConvert.SerializeObject(new RatingUsuarioDto(
                id, // UsuarioCalificadoID
                CurrentUser.ID, // UsuarioCalificadorID
                rating
            ));

            string url = "RatingUsuario";
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }



        /**** Métodos para las subastas ****/

        public async Task<SubastasPagedData> GetSubastas(int page = 1, string searchString = null, string sortOrder = null, string hideEnded = null, string hideMySubastas = null, string showAll = null)
        {
            string url = $"Subastas/{CurrentUser.ID}?page={page}";
            if (!string.IsNullOrEmpty(searchString))
            {
                url += $"&searchString={searchString}";
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                url += $"&sortOrder={sortOrder}";
            }
            if (!string.IsNullOrEmpty(hideEnded))
            {
                url += $"&hideEnded={hideEnded}";
            }
            if (!string.IsNullOrEmpty(hideMySubastas))
            {
                url += $"&hideMySubastas={hideMySubastas}";
            }
            if (!string.IsNullOrEmpty(showAll))
            {
                url += $"&showAll={showAll}";
            }
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<SubastasPagedData>(content);
                return data;
            }
        }

        public async Task<ICollection<SubastaItem>> ConvertToSubastaItems(IEnumerable<SubastaItemDto> itemsDto)
        {
            ICollection<SubastaItem> items = new List<SubastaItem>();
            foreach (var itemDto in itemsDto)
            {
                var image = await UriImage.UriToBitmapImage(itemDto.UriImagen);
                items.Add(new SubastaItem(
                    itemDto.ID,
                    itemDto.UsuarioID,
                    image,
                    itemDto.NombreProducto,
                    itemDto.MontoActual,
                    itemDto.Fecha,
                    itemDto.Vigente
                ));
            }
            return items;
        }

        public async Task<SubastaDto> GetSubasta(int id)
        {
            string url = $"Subasta/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<SubastaDto>(content);
                return data;
            }
        }

        // Observación: Validar peso de la imagen
        public async Task CreateSubasta(string nombreProducto, string descripcionProducto, string uriImagen, float precioInicial, DateTime fechaLimite)
        {
            if (Validator.IsEmpty(nombreProducto) || Validator.IsEmpty(descripcionProducto) || float.IsNaN(precioInicial))
            {
                throw new Exception("La información de subasta no puede contener campos vacíos.");
            }
            if (precioInicial <= 0)
            {
                throw new Exception("El precio inicial debe ser positivo.");
            }
            if (DateTime.Compare(fechaLimite, DateTime.Now) <= 0)
            {
                throw new Exception("La fecha seleccionada ya ha pasado.");
            }

            var body = JsonConvert.SerializeObject(new CreateSubastaDto(
                CurrentUser.ID,
                nombreProducto,
                descripcionProducto,
                uriImagen,
                precioInicial,
                fechaLimite
            ));

            string url = "CreateSubasta";
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task EditSubasta(int id, string nombreProducto, string descripcionProducto, string uriImagen = null)
        {
            if (Validator.IsEmpty(nombreProducto) || Validator.IsEmpty(descripcionProducto))
            {
                throw new Exception("La información de subasta no puede contener campos vacíos.");
            }

            var body = JsonConvert.SerializeObject(new EditSubastaDto(
                nombreProducto,
                descripcionProducto,
                uriImagen
            ));

            string url = $"EditSubasta/{id}";
            HttpResponseMessage response = await client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteSubasta(int id)
        {
            string url = $"DeleteSubasta/{id}";
            HttpResponseMessage response = await client.DeleteAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }




        /******* Métodos para los comentarios ********/

        public async Task<ComentarioDto> GetComentario(int id)
        {
            string url = $"PerfilOfertas/{id}";
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<ComentarioDto>(content);
                return data;
            }
        }

        public async Task CreateComentario(int subastaID, string descripcion)
        {
            // Validar información
            if (Validator.IsEmpty(descripcion))
            {
                throw new Exception("El comentario debe contener una descripción.");
            }

            var body = JsonConvert.SerializeObject(new CreateComentarioDto(
                CurrentUser.ID,
                subastaID,
                descripcion.Trim()
            ));

            string url = $"CreateComentario";
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task EditComentario(int id, string descripcion)
        {
            // Validar información
            if (Validator.IsEmpty(descripcion))
            {
                throw new Exception("El comentario debe contener una descripción.");
            }

            var body = JsonConvert.SerializeObject(new EditComentarioDto(
                descripcion.Trim()
            ));

            string url = $"EditComentario/{id}";
            HttpResponseMessage response = await client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteComentario(int id)
        {
            string url = $"DeleteComentario/{id}";
            HttpResponseMessage response = await client.DeleteAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }




        /**** Métodos para las ofertas ****/

        public async Task<PagedData<OfertaDto>> GetOfertas(int page = 1, string searchString = null)
        {
            string url = $"Subastas/{CurrentUser.ID}?page={page}";
            if (!string.IsNullOrEmpty(searchString))
            {
                url += $"&searchString={searchString}";
            }
            HttpResponseMessage response = await client.GetAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else
            {
                var data = JsonConvert.DeserializeObject<PagedData<OfertaDto>>(content);
                return data;
            }
        }

        public async Task CreateOferta(int subastaID, float monto)
        {
            // Validar información
            if (float.IsNaN(monto))
            {
                throw new Exception("El monto no puede estar vacío.");
            }
            if (monto <= 0)
            {
                throw new Exception("El monto de la oferta debe ser positivo.");
            }

            var body = JsonConvert.SerializeObject(new CreateOfertaDto(
                CurrentUser.ID,
                subastaID,
                monto
            ));

            string url = $"CreateOferta";
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
            string content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeleteOferta(int id)
        {
            string url = $"DeleteOferta/{id}";
            HttpResponseMessage response = await client.DeleteAsync(url);
            string content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<MessageDto>(content);
                    throw new Exception(error.Message);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }




    }
}
