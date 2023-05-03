using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace RottenTomatoes.Models
{
    public abstract class Show
    {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image_url")]
        public string ImageURL { get; set; }

        public string TomatometerScore { get; set; }

        public string AudienceScore { get;set; }

        public string Platforms { get; set; }

        public string Synopsis { get; set; }

        public string Genre { get; set; }

        public string ReleaseDate { get; set; }

    }
}
