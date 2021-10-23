using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Web.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        [Display(Name = "nombres")]
        public string Nombres { get; set; }
        [Required]
        [Display(Name = "apellidos")]
        public string Apellidos { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "correo")]
        public string Correo { get; set; }
        [MinLength(8, ErrorMessage = "La {0} debe tener una longitud mínima de 8 caracteres.")]
        [Required]
        [Display(Name = "contraseña")]
        public string Clave { get; set; }
    }
}