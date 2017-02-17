namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The PageRatingFilter class exposes a set of properties by which
    /// page ratings may be filtered.
    /// </summary>
    public class PageRatingFilter
    {
        /// <summary>
        /// Gets the reference of the target the rating applies to.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets the reference of the rater the rating applies to.
        /// </summary>
        public string Rater { get; set; }
    }
}