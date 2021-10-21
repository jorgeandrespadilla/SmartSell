// https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/adding-validation
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Web.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "correo")]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "La {0} debe tener una longitud mínima de 8 caracteres.")]
        [Required]
        [Display(Name = "contraseña")]
        public string Password { get; set; }
    }
}