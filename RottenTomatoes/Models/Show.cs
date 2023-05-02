namespace RottenTomatoes.Models
{
    public abstract class Show
    {
        public int IdShow { get; set; }
        public string Title { get; set; }

        public string ImageURL { get; set; }

        public string TomatometerScore { get; set; }

        public string AudienceScore { get;set; }

        public string criticReview { get; set; }

        public string audienceReview { get; set; }

        public string Platforms { get; set; }

        public string Synopsis { get; set; }

        public string Clasification { get; set; }

        public string Genre { get; set; }

        public string OriginalLanguage { get; set; }

        public string Director { get; set; }

        public string ReleaseDate { get; set; }

        public string Runtime { get; set; }

                  //actor : rol
        public Dictionary<string, string> actorRoles = new Dictionary<string, string>();

    }
}
