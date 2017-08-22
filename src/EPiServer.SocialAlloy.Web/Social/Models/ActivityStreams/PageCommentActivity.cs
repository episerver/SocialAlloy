using EPiServer.Social.Common;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents the activity of a page comment being submitted in the site.
    /// </summary>
    [ExtensionData("3a774681-b10a-48f9-9792-193614a97b00")]
    public class PageCommentActivity : PageActivity
    {
        /// <summary>
        /// The body of the comment that was posted on a page.
        /// </summary>
        public string Body { get; set; }
    }
}