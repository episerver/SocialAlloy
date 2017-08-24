using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of a rating being submitted on a page in the site.
    /// </summary>
    [ExtensionData(TypeId)]
    public class PageRatingActivity : PageActivity
    {
        /// <summary>
        /// The extension data class TypeId.
        /// </summary>
        /// <remarks>
        /// The social platform also allows using basic strings, not just GUIDs, for a TypeId.
        /// </remarks>
        public const string TypeId = "bf41f9e2-50f1-4ee4-bed3-e03f1914f720";

        /// <summary>
        /// The rating value that was submitted by a user rating a page.
        /// </summary>
        public int Value { get; set; }
    }
}