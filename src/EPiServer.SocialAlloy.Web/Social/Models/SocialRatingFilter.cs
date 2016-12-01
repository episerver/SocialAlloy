namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialRatingFilter class exposes a set of properties by which
    /// social ratings may be filtered.
    /// </summary>
    public class SocialRatingFilter
    {
        /// <summary>
        /// Gets the target reference the rating applies to.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets the rater user reference the rating applies to.
        /// </summary>
        public string Rater { get; set; }
    }
}