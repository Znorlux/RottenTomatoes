namespace RottenTomatoes.Models
{
    public class Serie : Show
    {
        public List<string> mainActors = new List<string>();
        public string Creator { get ; set; }
        public string tvNetwork { get; set; }

        public Serie(string Title, string ImageUrl, string TomatometerScore, string
            AudienceScore, string Platforms, string Synopsis,
            string Genre, string Creator, List<string> mainActors, string ReleaseDate,
            string criticReview, string audienceReview)
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
            this.criticReview = criticReview;
            this.audienceReview = audienceReview;
        }
    }
}
