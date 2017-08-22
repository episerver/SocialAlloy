using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of a page rating being submitted in the site.
    /// </summary>
    [ExtensionData("bf41f9e2-50f1-4ee4-bed3-e03f1914f720")]
    public class PageRatingActivity : PageActivity
    {
        /// <summary>
        /// The rating value that was submitted by a user rating a page.
        /// </summary>
        public int Value { get; set; }
    }
}