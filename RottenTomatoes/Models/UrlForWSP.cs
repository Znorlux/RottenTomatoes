using System.ComponentModel.DataAnnotations;
namespace RottenTomatoes.Models
{
    public class UrlForWSP
    {
        public int urlId { get; set; }
        [Required(ErrorMessage = "El campo Url es obligatorio.")]
        public string Url { get; set; }
    }
}
