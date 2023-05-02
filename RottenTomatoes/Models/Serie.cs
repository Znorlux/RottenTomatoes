namespace RottenTomatoes.Models
{
    public class Serie : Show
    {
        public List<string> mainActors = new List<string>();
        public string Creator { get ; set; }
        public string tvNetwork { get; set; }

        public Serie(string Title, string ImageUrl, string TomatometerScore, string
            AudienceScore, List<string> Platforms, string Synopsis,
            string Genre, string Creator, List<string> mainActors, string ReleaseDate)
        {

            this.Title = Title;
            this.ImageURL = ImageUrl;
            this.TomatometerScore = TomatometerScore;
            this.AudienceScore = AudienceScore;
            this.Platforms = Platforms;
            this.Synopsis = Synopsis;
            this.Genre = Genre;
            this.Creator = Creator;
            this.mainActors = mainActors;
            this.ReleaseDate = ReleaseDate;
            //quité dos atributos porque no habian criticas en las series
        }
    }
}
