using System.ComponentModel.DataAnnotations;

namespace RottenTomatoes.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo Tipo de usuario es obligatorio.")]
        public string UserType { get; set; }
    }
}
